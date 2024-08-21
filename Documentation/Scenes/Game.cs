using Godot;
using MenySystem.addons.menusystem;
using MenySystem.Documentation;

public partial class Game : Node
{
    public override void _Ready()
    {
        // Set startscene 
        PackedScene packedScene = GD.Load<PackedScene>("res://Documentation/Scenes/MainMenu.tscn");
        MenuController.Instance.SetInitialMenu(packedScene);

        // Subscribe to when the GameLevelStart
        GameEvents.OnGameLevelStart += GameStart;
    }

    public override void _ExitTree()
    {
        // UnSubscribe to when the GameLevelStart
        GameEvents.OnGameLevelStart -= GameStart;
    }

    private void GameStart()
    {
        PackedScene gameLevel = GD.Load<PackedScene>("res://Documentation/Scenes/GameLevel.tscn");
        Node2D gameInst = gameLevel.Instantiate<Node2D>();
        AddChild(gameInst);
    }
}
