using Godot;
using MenySystem.addons.menusystem.Buttons;
using MenySystem.addons.menusystem.PropertyTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem;
public partial class MenuController : CanvasLayer
{
    public static MenuController Instance { get; private set; }
    public FadeScreen CoverScreen { get; private set; }

    private Stack<Control> _menuStack = new();
    private bool _isPerformingTransition = false;
    private Control _menuControl;

    public override void _Ready()
    {
        Instance = this;

        _menuControl = new Control()
        {
            Name = "Menus",
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorBottom = 1,
            AnchorRight = 1
        };
        AddChild(_menuControl);

        var fadeScreen = new FadeScreen()
        {
            Name = "CoverScreen",
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorBottom = 1,
            AnchorRight = 1,
            Modulate = new Color(0, 0, 0, 0),
            MouseFilter = Control.MouseFilterEnum.Ignore,
        };
        CoverScreen = fadeScreen;
        AddChild(fadeScreen);
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
        CoverScreen.MouseFilter = Control.MouseFilterEnum.Stop;

        (Control From, Control To) menus = (transitionButton.TransitionType) switch
        {
            StackTransitionType.Push => PushToMenuStack(transitionButton.TransitionToScene),
            StackTransitionType.Pop when _menuStack.Count >= 1 => PopFromMenuStack(),
            StackTransitionType.Switch => SwitchMenuInStackTo(transitionButton.TransitionToScene),
            _ => (new Control(), new Control())
        };

        await transitionButton.TransitionNode.PerformTransition(menus.From, menus.To);

        CleanupMenuNodes(transitionButton.TransitionType, menus);

        // Debug
        StackDebug();
        // Debug

        CoverScreen.MouseFilter = Control.MouseFilterEnum.Ignore;
        _isPerformingTransition = false;
    }

    private void StackDebug()
    {
        GD.Print("-------------STACK-START--------");
        foreach (Control control in _menuStack)
        {
            GD.Print(control.Name);
        }
        //PrintOrphanNodes();
        GD.Print("-------------STACK-END--------");
    }

    public void SetInitialMenu(PackedScene initialMenu)
    {
        if (initialMenu is null || initialMenu.Instantiate() is not Control)
        {
            GD.PushError($"{nameof(MenuController)}-{nameof(initialMenu)}: Is not valid");
            return;
        }
        Control menu = initialMenu.Instantiate<Control>();
        _menuControl.AddChild(menu);
        _menuStack.Push(menu);
    }

    private (Control from, Control to) PushToMenuStack(PackedScene transitionTo)
    {
        Control from = _menuStack.Count > 0 ? _menuStack.Peek() : new Control();
        Control to = transitionTo.Instantiate<Control>();
        _menuStack.Push(to);
        _menuControl.AddChild(to);
        return (from, to);
    }

    private (Control from, Control to) PopFromMenuStack()
    {
        Control from = _menuStack.Pop();
        Control to;
        if (_menuStack.Count > 0)
        {
            to = _menuStack.Peek();
            _menuControl.AddChild(to);
        }
        else
        {
            to = new Control();
        }

        return (from, to);
    }

    private (Control from, Control to) SwitchMenuInStackTo(PackedScene transitionTo)
    {
        Control from = _menuStack.Count > 0 ? _menuStack.Pop() : new Control();
        Control to = transitionTo.Instantiate<Control>();
        _menuStack.Push(to);
        _menuControl.AddChild(to);
        return (from, to);
    }

    private void CleanupMenuNodes(StackTransitionType transitionType, (Control From, Control To) menus)
    {
        if (transitionType is StackTransitionType.Pop or StackTransitionType.Switch || menus.From.IsInsideTree() == false)
        {
            menus.From.QueueFree();
        }
        else
        {
            _menuControl.RemoveChild(menus.From);
        }

        if (menus.To.IsInsideTree() == false)
        {
            menus.To.QueueFree();
        }
    }
}
