using Godot;
using MenuControl.addons.MenuControl.Helpers;
using MenySystem.addons.menusystem.PropertyTypes;
using MenySystem.addons.menusystem.Transitions;
using System.Collections.Generic;
using System.Linq;

namespace MenySystem.addons.menusystem.Buttons;
[Tool]
public partial class TransitionButton : Control
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

    [Export]
    private string _actionKey = string.Empty;

    public Transition TransitionNode { get; private set; }

    public override void _Ready()
    {
#if TOOLS
        if (Engine.IsEditorHint())
        {
            ChildEnteredTree += UpdateConfigurationWarnings;
            ChildExitingTree += UpdateConfigurationWarnings;
        }
#endif
        TransitionNode = GetNodes.GetAllChildren<Transition>(this).FirstOrDefault();

        if (Engine.IsEditorHint() == false)
        {
            List<string> errors = ValidateNode();
            if (errors.Count > 0)
            {
                GD.PushError($"{GetPath()}: Parameters not valid: {string.Join(", ", errors)}");
                return;
            }
            if (GetParent() is BaseButton)
            {
                BaseButton parent = GetParent<BaseButton>();
                parent.Pressed += OnButtonPressed;
            }
        }
    }

    public override async void _Input(InputEvent @event)
    {
        // We check if the menu is the current one due to 'DisableMenuOption'. The menu is still
        // active in the background, despite the name 'disable'. And because of this it still 
        // handles key-inputs. 
        if (string.IsNullOrEmpty(_actionKey) || Owner.Equals(MenuController.Instance.CurrentMenu) == false)
        {
            return;
        }

        if (Input.IsActionJustPressed(_actionKey))
        {
            await MenuController.Instance.TransitionToMenu(this);
        }
    }

    public bool IsValid() => ValidateNode().Count == 0;

    private async void OnButtonPressed() => await MenuController.Instance.TransitionToMenu(this);

    #region  Validation
    private void UpdateConfigurationWarnings(Node node) => UpdateConfigurationWarnings();

    public override string[] _GetConfigurationWarnings() => ValidateNode().ToArray();

    private List<string> ValidateNode()
    {
        List<string> errors = new();
        errors.AddRange(ValidateParent());
        errors.AddRange(ValidateTransitionNode());
        errors.AddRange(ValidateTransitionTo());
        errors.AddRange(ValidateActionMap());
        return errors;
    }

    private IEnumerable<string> ValidateParent()
        => GetParent() is not BaseButton
        ? new List<string>() { "Parent must be derived from 'BaseButton'" }
        : new List<string>();

    private List<string> ValidateTransitionTo()
    {
        if (string.IsNullOrEmpty(TransitionToPath))
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

    private List<string> ValidateActionMap()
    {
        if (string.IsNullOrEmpty(_actionKey))
        {
            return new List<string>();
        }

        InputMap.LoadFromProjectSettings();

        return InputMap.HasAction(_actionKey)
            ? new List<string>()
            : new List<string>() { $"The InputMap action '{_actionKey}' does not exist. Did you spell i correctly?" };
    }
    #endregion
}
