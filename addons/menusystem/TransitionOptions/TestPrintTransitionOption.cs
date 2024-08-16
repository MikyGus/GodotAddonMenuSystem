using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class TestPrintTransitionOption : TransitionOption
{
    public override void BeforeAllTransitions_Set() => GD.Print(nameof(BeforeAllTransitions_Set));
    public override void BeforeAllTransitions_UnSet() => GD.Print(nameof(BeforeAllTransitions_UnSet));
    public override void AfterAllTransitions_Set() => GD.Print(nameof(AfterAllTransitions_Set));
    public override void AfterAllTransitions_UnSet() => GD.Print(nameof(AfterAllTransitions_UnSet));
    public override void AfterPageFromTransition_Set() => GD.Print(nameof(AfterPageFromTransition_Set));
    public override void AfterPageFromTransition_UnSet() => GD.Print(nameof(AfterPageFromTransition_UnSet));
    public override void AfterPageToTransition_Set() => GD.Print(nameof(AfterPageToTransition_Set));
    public override void AfterPageToTransition_UnSet() => GD.Print(nameof(AfterPageToTransition_UnSet));
}
