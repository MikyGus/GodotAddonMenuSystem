using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class MenuDisableOption : Control
{
    public Control DisableMenu(Control menu)
    {
        PackedScene packed = GD.Load<PackedScene>("res://addons/menusystem/DisableOption/MenuDisabler.tscn");
        MenuDisabler menuDisabler = packed.Instantiate<MenuDisabler>();
        menuDisabler.AddMenu(menu);
        return menuDisabler;
    }
}