using Godot;
using MenuControl.addons.MenuControl.Helpers;
using MenySystem.addons.menusystem.Buttons;
using MenySystem.addons.menusystem.PropertyTypes;
using MenySystem.addons.menusystem.TransitionOptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem;
public partial class MenuController : CanvasLayer
{
    public static MenuController Instance { get; private set; }
    public FadeScreen CoverScreen { get; private set; }
    public FadeScreen TranslucentScreen { get; private set; }
    public Spinner LoadingSpinner { get; private set; }

    private Stack<StackMenu> _menuStack = new();
    private bool _isPerformingTransition = false;
    private Control _menuControl;
    private TransitionButton _latestTransitionButton;

    private record struct StackMenu
    {
        public Control Menu { get; set; }
        public TransitionButton Button { get; set; }
    }

    private static StackMenu DefaultStackMenu = new() { Menu = new Control() };

    public override void _Ready()
    {
        Instance = this;

        ProcessMode = ProcessModeEnum.Always;

        _menuControl = GetNode<Control>("Menus");
        TranslucentScreen = _menuControl.GetNode<FadeScreen>("TranslucentScreen");

        CoverScreen = GetNode<FadeScreen>("CoverScreen");
        CoverScreen.MouseFilter = Control.MouseFilterEnum.Ignore;

        LoadingSpinner = GetNode<Spinner>("Spinner");
    }

    //public async Task TransitionToMenu(Transition transitionNode, StackTransitionType transitionType, string transitionToPath)
    //{
    //    if (transitionNode is null)
    //    {
    //        GD.PushError($"{nameof(transitionNode)} is NULL.");
    //        return;
    //    }
    //    if (_isPerformingTransition)
    //    {
    //        GD.PushWarning("Transition active! May not start a new at the moment");
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(transitionToPath))
    //    {
    //        transitionToPath = Constants.MENU_EMPTY_PATH;
    //    }
    public async Task TransitionToMenu(TransitionButton transitionButton)
    {
        if (transitionButton is null)
        {
            GD.PushError($"{nameof(transitionButton)} is NULL.");
            return;
        }
        if (transitionButton.IsValid() == false)
        {
            GD.PushError($"{nameof(transitionButton)} is not valid.");
            return;
        }

        _latestTransitionButton = transitionButton;
        _isPerformingTransition = true;
        CoverScreen.MouseFilter = Control.MouseFilterEnum.Stop;

        string transitionToPath = string.IsNullOrEmpty(transitionButton.TransitionToPath) ? Constants.MENU_EMPTY_PATH : transitionButton.TransitionToPath;
        PackedScene transitionToScene = GD.Load<PackedScene>(transitionToPath);

        SetTransitionButtonToCurrentMenu(transitionButton);

        (StackMenu From, StackMenu To) menus = transitionButton.TransitionType switch
        {
            StackTransitionType.Push => PushToMenuStack(transitionToScene),
            StackTransitionType.Pop when _menuStack.Count >= 1 => PopFromMenuStack(),
            StackTransitionType.Switch => SwitchMenuInStackTo(transitionToScene),
            StackTransitionType.ClearAllAndPush => ClearAllAndPushToStack(transitionToScene),
            _ => (DefaultStackMenu, DefaultStackMenu)
        };

        RearrangeOrderOfMenus(menus);

        menus.From.Menu = PerformDisableMenuOption(menus.From);
        menus.To.Menu = PerformDisableMenuOption(menus.To);


        await transitionButton.TransitionNode.PerformTransition(menus.From.Menu, menus.To.Menu, menus.From.Button, menus.To.Button);

        CleanupMenuNodes(transitionButton.TransitionType, menus);

#if DEBUG
        // Debug
        //StackDebug();
        // Debug
#endif

        CoverScreen.MouseFilter = Control.MouseFilterEnum.Ignore;
        _isPerformingTransition = false;
    }

    /// <summary>
    /// Activate the ability to disable the menu with the 'MenuDisableOption' node
    /// </summary>
    /// <param name="menuStack"></param>
    /// <returns>The Menu-Control or the Mock-Control from MenuDisabler</returns>
    private Control PerformDisableMenuOption(StackMenu menuStack)
    {
        MenuDisableOption menuDisable = menuStack.Button?.GetAllChildren<MenuDisableOption>().FirstOrDefault();
        if (menuDisable is null)
        {
            return menuStack.Menu;
        }
        Control newMenu = menuDisable.DisableMenu(menuStack.Menu);
        return newMenu;
    }

    /// <summary>
    /// Making sure MenuFrom and MenuTo have the TranslucentScreen between them
    /// </summary>
    /// <param name="menus"></param>
    private void RearrangeOrderOfMenus((StackMenu From, StackMenu To) menus)
    {
        if (menus.From.Menu.IsInsideTree())
        {
            _menuControl.MoveChild(menus.From.Menu, 0);
        }
        if (menus.To.Menu.IsInsideTree())
        {
            _menuControl.MoveChild(menus.To.Menu, -1);
        }
    }

    /// <summary>
    /// Make sure the transitionButton, the one attached to the button the user clicked, is linked to the current menu.
    /// </summary>
    /// <param name="transitionButton"></param>
    private void SetTransitionButtonToCurrentMenu(TransitionButton transitionButton)
    {
        if (_menuStack.Count > 0)
        {
            StackMenu currentStackMenu = _menuStack.Pop();
            currentStackMenu.Button = transitionButton;
            _menuStack.Push(currentStackMenu);
        }
    }

#if DEBUG
    /// <summary>
    /// In debug we print the current stack
    /// </summary>
    private void StackDebug()
    {
        GD.Print("-------------STACK-START--------");
        foreach (StackMenu control in _menuStack)
        {
            GD.Print(control.Menu.Name);
        }
        //PrintOrphanNodes();
        GD.Print("-------------STACK-END--------");
    }
#endif

    public void SetInitialMenu(PackedScene initialMenu)
    {
        if (initialMenu is null || initialMenu.Instantiate() is not Control)
        {
            GD.PushError($"{nameof(MenuController)}-{nameof(initialMenu)}: Is not valid");
            return;
        }
        Control menu = initialMenu.Instantiate<Control>();
        StackMenu stackMenu = new StackMenu() { Menu = menu };
        _menuControl.AddChild(menu);
        _menuStack.Push(stackMenu);
    }

    /// <summary>
    /// Add the menu (Control) to the MenuControl-node. But only if it not already is attached to the tree.
    /// </summary>
    /// <param name="menu"></param>
    private void MenuControlAddChild(Control menu)
    {
        if (menu.IsInsideTree() == false)
        {
            _menuControl.AddChild(menu);
        }
    }

    private (StackMenu from, StackMenu to) PushToMenuStack(PackedScene transitionTo)
    {
        StackMenu from = _menuStack.Count > 0 ? _menuStack.Peek() : DefaultStackMenu;
        Control toMenu = transitionTo.Instantiate<Control>();
        StackMenu to = new StackMenu() { Menu = toMenu };
        _menuStack.Push(to);
        MenuControlAddChild(to.Menu);
        return (from, to);
    }

    private (StackMenu from, StackMenu to) PopFromMenuStack()
    {
        StackMenu from = _menuStack.Pop();
        StackMenu to;
        if (_menuStack.Count > 0)
        {
            to = _menuStack.Peek();
            MenuControlAddChild(to.Menu);
        }
        else
        {
            to = DefaultStackMenu;
        }

        return (from, to);
    }

    private (StackMenu from, StackMenu to) SwitchMenuInStackTo(PackedScene transitionTo)
    {
        StackMenu from = _menuStack.Count > 0 ? _menuStack.Pop() : DefaultStackMenu;
        Control toMenu = transitionTo.Instantiate<Control>();
        StackMenu to = new() { Menu = toMenu };
        _menuStack.Push(to);
        MenuControlAddChild(to.Menu);
        return (from, to);
    }

    private (StackMenu from, StackMenu to) ClearAllAndPushToStack(PackedScene transitionTo)
    {
        StackMenu from = _menuStack.Count > 0 ? _menuStack.Pop() : DefaultStackMenu;
        Control toMenu = transitionTo.Instantiate<Control>();
        StackMenu to = new StackMenu() { Menu = toMenu };

        _latestTransitionButton.TransitionNode.OnPostPageFromTransition += ClearMenuStack;

        _menuStack.Push(to);
        MenuControlAddChild(to.Menu);
        return (from, to);
    }

    private void ClearMenuStack(Control _)
    {
        StackMenu currentMenu = _menuStack.Pop();
        foreach (StackMenu item in _menuStack)
        {
            item.Menu.QueueFree();
        }
        _menuStack.Clear();
        _menuStack.Push(currentMenu);

        _latestTransitionButton.TransitionNode.OnPostPageFromTransition -= ClearMenuStack;
    }

    private void CleanupMenuNodes(StackTransitionType transitionType, (StackMenu From, StackMenu To) menus)
    {
        if (transitionType is StackTransitionType.Pop or StackTransitionType.Switch || menus.From.Menu.IsInsideTree() == false)
        {
            menus.From.Menu.QueueFree();
        }
        else
        {
            _menuControl.RemoveChild(menus.From.Menu);
        }

        if (menus.To.Menu.IsInsideTree() == false)
        {
            menus.To.Menu.QueueFree();
        }
    }
}
