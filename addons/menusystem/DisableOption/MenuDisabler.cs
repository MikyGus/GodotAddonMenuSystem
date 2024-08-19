using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;

public partial class MenuDisabler : Control
{
    Control _menu;

    public void AddMenu(Control menu) => _menu = menu;
}