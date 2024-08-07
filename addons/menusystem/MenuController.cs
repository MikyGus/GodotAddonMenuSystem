using Godot;
using MenySystem.addons.menusystem.Buttons;

namespace MenySystem.addons.menusystem;
public partial class MenuController : Node
{
    public static MenuController Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    internal void TransitionToMenu(TransitionButton transitionButton)
    {
        if (transitionButton is null)
        {
            GD.PushError($"{nameof(transitionButton)} is NULL.");
        }
        else if (transitionButton.IsValid() == false)
        {
            GD.PushError($"{nameof(TransitionButton)}: is not valid!");
        }


    }
}
