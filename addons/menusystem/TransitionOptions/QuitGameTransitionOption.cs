namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class QuitGameTransitionOption : TransitionOption
{
    public override void SetAction() => GetTree().Quit();
    public override void UnsetAction() { }
}
