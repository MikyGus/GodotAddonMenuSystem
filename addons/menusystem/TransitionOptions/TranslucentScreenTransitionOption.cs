using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class TranslucentScreenTransitionOption : TransitionOption
{
    [Export(PropertyHint.Range, "0,1,0.1")]
    public float TranslucentLevel { get; set; } = 0.5f;
    [Export(PropertyHint.Range, "0,10,0.1,or_greater")]
    public float FadeTime { get; set; } = 0.5f;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    public override void ActionOnFromMenu() => MenuController.Instance.TranslucentScreen.FadeToAlpha(TranslucentLevel, FadeTime);
    public override void ActionOnReturnToMenu() => MenuController.Instance.TranslucentScreen.FadeToAlpha(0, FadeTime);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
}
