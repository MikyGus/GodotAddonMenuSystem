using Godot;
using MenuControl.addons.MenuControl.Helpers;
using MenySystem.addons.menusystem.PropertyTypes;
using MenySystem.addons.menusystem.Transitions;
using System.Collections.Generic;
using System.Linq;

namespace MenySystem.addons.menusystem.Buttons;
[Tool]
public partial class TransitionButton : Button
{
    private StackTransitionType _transitionType;
    [Export]
    public StackTransitionType TransitionType
    {
        get => _transitionType;
        private set
        {
            _transitionType = value;
            UpdateConfigurationWarnings();
        }
    }

    private string _transitionToPath;

    [Export(PropertyHint.File, "*.tscn")]
    public string TransitionToPath
    {
        get => _transitionToPath;
        private set
        {
            _transitionToPath = value;
            UpdateConfigurationWarnings();
        }
    }
    public Transition TransitionNode { get; private set; }
    public PackedScene TransitionToScene => GD.Load<PackedScene>(TransitionToPath);


    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            ChildEnteredTree += UpdateConfigurationWarnings;
            ChildExitingTree += UpdateConfigurationWarnings;
        }

        TransitionNode = GetNodes.GetAllChildren<Transition>(this).FirstOrDefault();

        Pressed += OnButtonPressed;
    }

    public bool IsValid() => ValidateNode().Count == 0;

    private async void OnButtonPressed() => await MenuController.Instance.TransitionToMenu(this);

    private void UpdateConfigurationWarnings(Node node) => UpdateConfigurationWarnings();

    public override string[] _GetConfigurationWarnings() => ValidateNode().ToArray();

    private List<string> ValidateNode()
    {
        List<string> errors = ValidateTransitionNode();
        errors.AddRange(ValidateTransitionTo());
        return errors;
    }

    private List<string> ValidateTransitionTo()
    {
        if (string.IsNullOrEmpty(TransitionToPath) && TransitionType != StackTransitionType.Pop)
        {
            return new List<string>() { "A scene to transition to is required! Or change TransitionType to Pop" };
        }
        else if (TransitionType == StackTransitionType.Pop)
        {
            return new List<string>();
        }

        PackedScene scene = GD.Load<PackedScene>(TransitionToPath);
        return scene.Instantiate() is not Control
            ? new List<string>() { "The transitionTo scene needs to derive from Control!" }
            : new List<string>();
    }

    private List<string> ValidateTransitionNode()
    {
        var transitions = GetNodes.GetAllChildren<Transition>(this).ToList();
        int count = transitions.Count;

        if (count < 1)
        {
            return new List<string> { "Transition node is required as a child" };
        }
        else if (count > 1)
        {
            return new List<string> { "Only one transition is allowed per button" };
        }
        return new List<string>();
    }
}
