using Godot;
using MenySystem.addons.menusystem;

public partial class Game : Node
{
    public override void _Ready()
    {
        // Set startscene 
        PackedScene packedScene = GD.Load<PackedScene>("res://Documentation/Scenes/MainMenu.tscn");
        MenuController.Instance.SetInitialMenu(packedScene);

        // Subscribe to when the GameLevelStart
        MenuEvents.OnGameLevelStart += GameStart;
        MenuEvents.OnGameLevelEnd += GameEnd;
    }

    public override void _ExitTree()
    {
        // UnSubscribe to when the GameLevelStart
        MenuEvents.OnGameLevelStart -= GameStart;
        MenuEvents.OnGameLevelEnd -= GameEnd;
    }

    private void GameStart()
    {
        PackedScene gameLevel = GD.Load<PackedScene>("res://Documentation/Scenes/GameLevel.tscn");
        Node2D gameInst = gameLevel.Instantiate<Node2D>();

        GetNode<Node>("GameLevel").AddChild(gameInst);
    }

    private void GameEnd()
    {
        Godot.Collections.Array<Node> children = GetNode<Node>("GameLevel").GetChildren();
        foreach (Node child in children)
        {
            child.QueueFree();
        }
    }
}
