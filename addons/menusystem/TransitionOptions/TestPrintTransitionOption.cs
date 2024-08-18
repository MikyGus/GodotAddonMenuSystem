using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class TestPrintTransitionOption : TransitionOption
{
    public override void SetAction() => GD.Print($"SET: {GetPath()}");
    public override void UnsetAction() => GD.Print($"UNSET: {GetPath()}");
}
