using Godot;
using MenySystem.addons.menusystem.PropertyTypes;
using System;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
[Tool]
public partial class FadeTransition : Transition
{
    [Export]
    public FadeType FadeType { get; set; } = FadeType.In | FadeType.Out;
    [Export(PropertyHint.Range, "0,10,0.1")]
    public float TransitionTime { get; private set; } = 1f;

    public override async Task PerformTransition(Control transitionFrom, Control transitionTo)
    {
        ArgumentNullException.ThrowIfNull(transitionFrom, nameof(transitionFrom));
        ArgumentNullException.ThrowIfNull(transitionTo, nameof(transitionTo));

        transitionTo.Visible = false;
        transitionTo.GlobalPosition = Vector2.Zero;

        await MenuController.Instance.CoverScreen.FadeToAlpha(1, TransitionTime);
        transitionFrom.Visible = false;
        transitionTo.Visible = true;
        await MenuController.Instance.CoverScreen.FadeToAlpha(0, TransitionTime);

        //await FadeOut(transitionFrom);
        //await FadeIn(transitionTo);
    }

    //public async Task FadeOut(Control transitionFrom)
    //{
    //    //Action actionOut = ActionFrom(transitionFrom, fadeTransitionType);
    //    Action actionOut = null;
    //    if (FadeType.HasFlag(FadeType.Out))
    //    {
    //        await FaderAsync(FadeType.Out, TransitionTime, actionOut);
    //        transitionFrom.Visible = false;
    //        //GD.Print($"FadeTransition-FadeOut: Wait is done");
    //    }
    //    else
    //    {
    //        actionOut?.Invoke();
    //    }
    //}

    //public async Task FadeIn(Control transitionTo)
    //{
    //    //Action actionIn = ActionTo(transitionTo);
    //    Action actionIn = null;
    //    if (FadeType.HasFlag(FadeType.In))
    //    {
    //        transitionTo.Visible = true;
    //        await FaderAsync(FadeType.In, TransitionTime, actionIn);
    //        //GD.Print($"FadeTransition-FadeIn: FadeIn is done");
    //    }
    //    else
    //    {
    //        actionIn?.Invoke();
    //    }
    //}

    //private async Task FaderAsync(FadeType fadeType, float transitionTime, Action action = null)
    //{
    //    Tween tween = CreateTween();
    //    //GD.Print($"FadeTransition-Fade: {fadeType} tweening");
    //    int finalVal = fadeType == FadeType.In ? 0 : 1;
    //    tween.TweenProperty(_fadeScreen, Constants.PROPERTY_MODULATE_A, finalVal, transitionTime);
    //    tween.TweenCallback(Callable.From(action));
    //    await ToSignal(tween, Constants.EVENT_FINISHED);
    //}
}
