using Godot;
using MenySystem.addons.menusystem.PropertyTypes;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
[Tool]
public partial class FadeTransition : Transition
{
    [Export]
    public FadeType FadeType { get; set; } = FadeType.In | FadeType.Out;
    [Export(PropertyHint.Range, "0,10,0.1")]
    public float TransitionTime { get; private set; } = 1f;

    protected override async Task PerformTransition(Control transitionFrom, Control transitionTo)
    {
        transitionTo.Visible = false;
        transitionTo.GlobalPosition = Vector2.Zero;

        RaiseOnPreTransition(transitionFrom, transitionTo);
        if (FadeType.HasFlag(FadeType.Out))
        {
            await MenuController.Instance.CoverScreen.FadeToAlpha(1, TransitionTime);
            transitionFrom.Visible = false;
            RaiseOnPostPageFromTransition(transitionFrom);
        }
        if (FadeType.HasFlag(FadeType.In))
        {
            transitionTo.Visible = true;
            await MenuController.Instance.CoverScreen.FadeToAlpha(0, TransitionTime);
            RaiseOnPostPageToTransition(transitionTo);
        }
        RaiseOnPostTransition(transitionFrom, transitionTo);
    }
}
