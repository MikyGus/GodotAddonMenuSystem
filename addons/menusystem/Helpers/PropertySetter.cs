using Godot;
using static Godot.Control;

namespace MenuControl.addons.MenuControl.Helpers;
public static class PropertySetter
{
    public static void SetFullScreen(Control control, MouseFilterEnum mouseFilterEnum)
    {
        control.AnchorLeft = 0;
        control.AnchorRight = 1;
        control.AnchorTop = 0;
        control.AnchorBottom = 1;
        control.MouseFilter = mouseFilterEnum;
    }
}
