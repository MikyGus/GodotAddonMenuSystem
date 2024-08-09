using Godot;
using MenySystem.addons.menusystem.Buttons;
using MenySystem.addons.menusystem.PropertyTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem;
public partial class MenuController : Node
{
    public static MenuController Instance { get; private set; }

    private Stack<Control> _menuStack = new();
    private bool _isPerformingTransition = false;

    public override void _Ready()
    {
        Instance = this;
    }

    public async Task TransitionToMenu(TransitionButton transitionButton)
    {
        if (transitionButton is null)
        {
            GD.PushError($"{nameof(transitionButton)} is NULL.");
            return;
        }
        else if (transitionButton.IsValid() == false)
        {
            GD.PushError($"{nameof(transitionButton)}: is not valid!");
            return;
        }

        if (_isPerformingTransition)
        {
            GD.PushWarning("Transition active! May not start a new at the moment");
            return;
        }
        _isPerformingTransition = true;

        (Control From, Control To) menus = (transitionButton.TransitionType) switch
        {
            StackTransitionType.Push => PushToMenuStack(transitionButton.TransitionToScene),
            StackTransitionType.Pop when _menuStack.Count >= 1 => PopFromMenuStack(),
            StackTransitionType.Switch => SwitchMenuInStackTo(transitionButton.TransitionToScene),
            _ => (new Control(), new Control())
        };

        await transitionButton.TransitionNode.PerformTransition(menus.From, menus.To);

        CleanupMenuNodes(transitionButton.TransitionType, menus);

        _isPerformingTransition = false;
    }

    public void SetInitialMenu(PackedScene initialMenu)
    {
        if (initialMenu is null || initialMenu.Instantiate() is not Control)
        {
            GD.PushError($"{nameof(MenuController)}-{nameof(initialMenu)}: Is not valid");
        }
        Control menu = initialMenu.Instantiate<Control>();
        AddChild(menu);
        _menuStack.Push(menu);
    }

    private (Control from, Control to) PushToMenuStack(PackedScene transitionTo)
    {
        Control from = _menuStack.Count > 0 ? _menuStack.Peek() : new Control();
        Control to = transitionTo.Instantiate<Control>();
        _menuStack.Push(to);
        return (from, to);
    }

    private (Control from, Control to) PopFromMenuStack()
    {
        Control from = _menuStack.Pop();
        Control to = _menuStack.Count > 0 ? _menuStack.Pop() : new Control();
        return (from, to);
    }

    private (Control from, Control to) SwitchMenuInStackTo(PackedScene transitionTo)
    {
        Control from = _menuStack.Count > 0 ? _menuStack.Pop() : new Control();
        Control to = transitionTo.Instantiate<Control>();
        _menuStack.Push(to);
        return (from, to);
    }

    private static void CleanupMenuNodes(StackTransitionType transitionType, (Control From, Control To) menus)
    {
        if (transitionType is StackTransitionType.Push or StackTransitionType.Switch || menus.From.IsInsideTree() == false)
        {
            menus.From.QueueFree();
        }
        if (menus.To.IsInsideTree() == false)
        {
            menus.To.QueueFree();
        }
    }
}
