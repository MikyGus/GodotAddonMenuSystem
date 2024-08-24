namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class QuitGameTransitionOption : TransitionOption
{
    public override void ActionOnFromMenu() => GetTree().Quit();
    public override void ActionOnReturnToMenu() { }
}
