namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class PauseGameTransitionOption : TransitionOption
{
    public override void SetAction()
    {
        GetTree().Paused = true;
        MenuEvents.PauseGame();
    }

    public override void UnsetAction()
    {
        GetTree().Paused = false;
        MenuEvents.ResumeGame();
    }
}
