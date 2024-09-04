using Godot;
using MenySystem.addons.menusystem.GameLoad;
using MenySystem.addons.menusystem.PropertyTypes;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
[Tool]
public partial class FadeTransition : Transition
{
    [Signal]
    public delegate void _sceneLoadedEventHandler(Control from, Control to);
    [Signal]
    public delegate void _transitionDoneEventHandler();

    [Export]
    public FadeType FadeType { get; set; } = FadeType.In | FadeType.Out;
    [Export(PropertyHint.Range, "0,10,0.1")]
    public float TransitionTime { get; private set; } = 1f;
    [Export]
    public bool WaitForSceneLoadedEvent { get; private set; } = false;

    private Control _transitionTo;
    private Control _transitionFrom;

    protected override async Task PerformTransition(Control transitionFrom, Control transitionTo)
    {
        _transitionTo = transitionTo;
        _transitionTo.Visible = false;
        _transitionTo.GlobalPosition = Vector2.Zero;
        _transitionFrom = transitionFrom;

        RaiseOnPreTransition(transitionFrom, _transitionTo);
        if (WaitForSceneLoadedEvent)
        {
            await FadeOutTransition(transitionFrom);

            // While waiting for the game to load we have to stay in this method
            // This is to prevent us from referencing an already disposed object
            // when we are to transition back in.
            _sceneLoaded += OnSceneLoaded;
            GameSceneLoader.OnSceneLoaded += EmitSceneLoaded;
            await ToSignal(this, SignalName._transitionDone);

            // Clean up after the transition is done
            _sceneLoaded -= OnSceneLoaded;
            GameSceneLoader.OnSceneLoaded -= EmitSceneLoaded;
        }
        else
        {
            await FadeOutTransition(transitionFrom);
            await FadeInTransition(transitionTo);
            RaiseOnPostTransition(transitionFrom, transitionTo);
        }
    }

    private void EmitSceneLoaded() => EmitSignal(SignalName._sceneLoaded, _transitionFrom, _transitionTo);

    private async void OnSceneLoaded(Control from, Control to)
    {
        await FadeInTransition(to);
        RaiseOnPostTransition(from, to);
        EmitSignal(SignalName._transitionDone);
    }

    private async Task FadeOutTransition(Control transitionFrom)
    {
        if (FadeType.HasFlag(FadeType.Out))
        {
            await MenuController.Instance.CoverScreen.FadeToAlpha(1, TransitionTime);
            transitionFrom.Visible = false;
            RaiseOnPostPageFromTransition(transitionFrom);
        }
    }

    private async Task FadeInTransition(Control transitionTo)
    {
        if (FadeType.HasFlag(FadeType.In))
        {
            transitionTo.Visible = true;
            await MenuController.Instance.CoverScreen.FadeToAlpha(0, TransitionTime);
            RaiseOnPostPageToTransition(transitionTo);
        }
    }
}
