using Godot;
using System;

namespace MenySystem.addons.menusystem.GameLoad;
public partial class GameSceneLoader : Node
{
    public static GameSceneLoader Instance { get; private set; }

    public static event Action OnSceneLoaded;

    private string _loadPath;
    private Node _parentNode;

    public override void _Ready()
    {
        Instance = this;
    }

    public void LoadScene(string scenePath, Node parent)
    {
        _loadPath = scenePath;
        _parentNode = parent;

        MenuController.Instance.LoadingSpinner.Visible = true;
        // TODO: Handle errors, like invalid loadPath
        ResourceLoader.LoadThreadedRequest(_loadPath);
    }

    public override void _Process(double delta)
    {
        ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_loadPath);
        if (status == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            PackedScene packed_scene = ResourceLoader.LoadThreadedGet(_loadPath) as PackedScene;
            _parentNode.AddChild(packed_scene.Instantiate<Node2D>());
            MenuController.Instance.LoadingSpinner.Visible = false;
            OnSceneLoaded?.Invoke();
        }
    }
}
