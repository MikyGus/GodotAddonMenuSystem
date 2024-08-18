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
    public FadeScreen TranslucentScreen { get; private set; }

    private Stack<StackMenu> _menuStack = new();
    private bool _isPerformingTransition = false;
    private Control _menuControl;

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
            _ => (DefaultStackMenu, DefaultStackMenu)
        };

        SetZIndexOfMenus(menus);

        await transitionButton.TransitionNode.PerformTransition(menus.From.Menu, menus.To.Menu, menus.From.Button, menus.To.Button);

        CleanupMenuNodes(transitionButton.TransitionType, menus);

        // Debug
        //StackDebug();
        // Debug

        CoverScreen.MouseFilter = Control.MouseFilterEnum.Ignore;
        _isPerformingTransition = false;
    }

    /// <summary>
    /// Making sure MenuFrom and MenuTo have the TranslucentScreen (ZIndex=2) between them
    /// </summary>
    /// <param name="menus"></param>
    private static void SetZIndexOfMenus((StackMenu From, StackMenu To) menus)
    {
        menus.From.Menu.ZIndex = 1;
        menus.To.Menu.ZIndex = 3;
    }

    private void SetTransitionButtonToCurrentMenu(TransitionButton transitionButton)
    {
        StackMenu currentStackMenu = _menuStack.Pop();
        currentStackMenu.Button = transitionButton;
        _menuStack.Push(currentStackMenu);
    }

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

    private (StackMenu from, StackMenu to) PushToMenuStack(PackedScene transitionTo)
    {
        StackMenu from = _menuStack.Count > 0 ? _menuStack.Peek() : DefaultStackMenu;
        Control toMenu = transitionTo.Instantiate<Control>();
        StackMenu to = new StackMenu() { Menu = toMenu };
        _menuStack.Push(to);
        _menuControl.AddChild(to.Menu);
        return (from, to);
    }

    private (StackMenu from, StackMenu to) PopFromMenuStack()
    {
        StackMenu from = _menuStack.Pop();
        StackMenu to;
        if (_menuStack.Count > 0)
        {
            to = _menuStack.Peek();
            _menuControl.AddChild(to.Menu);
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
        _menuControl.AddChild(to.Menu);
        return (from, to);
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
