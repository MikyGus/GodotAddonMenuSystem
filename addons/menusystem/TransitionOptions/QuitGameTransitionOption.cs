using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class QuitGameTransitionOption : TransitionOption
{
    public override void BeforeAllTransitions_Set() => DoPerformQuit();
    public override void BeforeAllTransitions_UnSet() => DoPerformQuit();
    public override void AfterAllTransitions_Set() => DoPerformQuit();
    public override void AfterAllTransitions_UnSet() => DoPerformQuit();
    public override void AfterPageFromTransition_Set() => DoPerformQuit();
    public override void AfterPageFromTransition_UnSet() => DoPerformQuit();
    public override void AfterPageToTransition_Set() => DoPerformQuit();
    public override void AfterPageToTransition_UnSet() => DoPerformQuit();

    private void DoPerformQuit()
    {
        GD.Print("Quiting");
        GetTree().Quit();
    }
}
