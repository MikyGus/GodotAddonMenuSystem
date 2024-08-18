namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class PauseGameTransitionOption : TransitionOption
{
    public override void SetAction() => GetTree().Paused = true;
    public override void UnsetAction() => GetTree().Paused = false;
}
