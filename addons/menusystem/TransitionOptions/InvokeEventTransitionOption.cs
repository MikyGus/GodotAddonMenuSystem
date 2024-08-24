using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;

public partial class InvokeEventTransitionOption : TransitionOption
{
    [Export]
    public EventCategories Invoke { get; set; }

    public override void ActionOnFromMenu()
    {
        switch (Invoke)
        {
            case EventCategories.GameLevelStart:
                MenuEvents.StartGame();
                break;
            case EventCategories.GameLevelEnd:
                MenuEvents.EndGame();
                break;
            case EventCategories.GamePaused:
                MenuEvents.PauseGame();
                break;
            case EventCategories.GameResumed:
                MenuEvents.ResumeGame();
                break;
            default:
                break;
        }
    }

    public override void ActionOnReturnToMenu() { }
}
