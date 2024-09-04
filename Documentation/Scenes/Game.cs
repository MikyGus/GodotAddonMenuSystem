using Godot;
using MenySystem.addons.menusystem;
using MenySystem.addons.menusystem.GameLoad;

public partial class Game : Node
{
    private string _loadPath = "res://Documentation/Scenes/GameLevel.tscn";
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

    private void GameStart() => GameSceneLoader.Instance.LoadScene(_loadPath, GetNode<Node>("GameLevel"));

    private void GameEnd()
    {
        Godot.Collections.Array<Node> children = GetNode<Node>("GameLevel").GetChildren();
        foreach (Node child in children)
        {
            child.QueueFree();
        }
    }
}
