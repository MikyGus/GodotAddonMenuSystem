using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class TestPrintTransitionOption : TransitionOption
{
    public override void ActionOnFromMenu() => GD.Print($"SET: {GetPath()}");
    public override void ActionOnReturnToMenu() => GD.Print($"UNSET: {GetPath()}");
}
