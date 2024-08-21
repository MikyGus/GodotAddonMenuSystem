using Godot;
using MenuControl.addons.MenuControl.Helpers;
using MenySystem.addons.menusystem.Buttons;
using MenySystem.addons.menusystem.Transitions;
using MenySystem.Documentation;
using System.Linq;

public partial class PlayButton : Button
{
    Transition _transitionNode;

    public override void _Ready()
    {
        _transitionNode = this.GetAllChildren<TransitionButton>().FirstOrDefault().TransitionNode;
        _transitionNode.OnPostPageFromTransition += LoadGameLevel;
    }

    public override void _ExitTree()
    {
        _transitionNode.OnPostPageFromTransition -= LoadGameLevel;
    }

    private void LoadGameLevel(Control control)
    {
        GameEvents.StartGame();
    }
}
