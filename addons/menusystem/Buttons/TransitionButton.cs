using Godot;
using MenuControl.addons.MenuControl.Helpers;
using MenySystem.addons.menusystem.PropertyTypes;
using MenySystem.addons.menusystem.Transitions;
using System.Linq;

namespace MenySystem.addons.menusystem.Buttons;
[Tool]
public partial class TransitionButton : Button
{
    [Export]
    private StackTransitionType _transitionType;
    [Export]
    private PackedScene _transitionTo;

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            ChildEnteredTree += UpdateConfigurationWarnings;
            ChildExitingTree -= UpdateConfigurationWarnings;
        }
    }

    private void UpdateConfigurationWarnings(Node node) => UpdateConfigurationWarnings();

    public override string[] _GetConfigurationWarnings()
    {
        var transitions = GetNodes.GetAllChildren<Transition>(this).ToList();
        int count = transitions.Count;

        if (count < 1)
        {
            return new string[] { "Transition node is required as a child" };
        }
        else if (count > 1)
        {
            return new string[] { "Only one transition is allowed per button" };
        }
        return System.Array.Empty<string>();
    }
}
