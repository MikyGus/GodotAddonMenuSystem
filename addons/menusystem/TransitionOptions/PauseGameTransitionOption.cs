namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class PauseGameTransitionOption : TransitionOption
{
    public override void ActionOnFromMenu()
    {
        GetTree().Paused = true;
        MenuEvents.PauseGame();
    }

    public override void ActionOnReturnToMenu()
    {
        GetTree().Paused = false;
        MenuEvents.ResumeGame();
    }
}
