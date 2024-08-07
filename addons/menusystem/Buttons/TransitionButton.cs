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
    [Export]
    private StackTransitionType _transitionType;
    private PackedScene _transitionTo;

    [Export]
    public PackedScene TransitionTo
    {
        get => _transitionTo;
        private set
        {
            UpdateConfigurationWarnings();
            _transitionTo = value;
        }
    }

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            ChildEnteredTree += UpdateConfigurationWarnings;
            ChildExitingTree -= UpdateConfigurationWarnings;
        }

        Pressed += OnButtonPressed;
    }

    public bool IsValid() => ValidateNode().Count == 0;

    private void OnButtonPressed()
    {
        MenuController.Instance.TransitionToMenu(this);
    }

    private void UpdateConfigurationWarnings(Node node) => UpdateConfigurationWarnings();

    public override string[] _GetConfigurationWarnings() => ValidateNode().ToArray();

    private List<string> ValidateNode()
    {
        List<string> errors = ValidateTransitionNode();
        errors.AddRange(ValidateTransitionTo());
        return errors;
    }

    private List<string> ValidateTransitionTo()
        => TransitionTo is null
            ? new List<string>() { "A scene to transition to is required!" }
            : new List<string>();

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
