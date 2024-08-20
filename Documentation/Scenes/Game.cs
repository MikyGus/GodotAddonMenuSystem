using Godot;
using MenySystem.addons.menusystem;

public partial class Game : Node
{
    public override void _Ready()
    {
        PackedScene packedScene = GD.Load<PackedScene>("res://Documentation/Scenes/MainMenu.tscn");
        MenuController.Instance.SetInitialMenu(packedScene);
    }
}
