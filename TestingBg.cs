using Godot;
using MenySystem.addons.menusystem;

public partial class TestingBg : Control
{
    public override void _Ready()
    {
        PackedScene packedScene = GD.Load<PackedScene>("res://Menu1.tscn");
        MenuController.Instance.SetInitialMenu(packedScene);
    }
}