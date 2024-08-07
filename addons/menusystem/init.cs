#if TOOLS
using Godot;
using MenySystem.addons.menusystem;

[Tool]
public partial class init : EditorPlugin
{
    public override void _EnterTree()
    {
        Script buttonScript = GD.Load<Script>($"{Constants.BASE_PATH}Buttons/TransitionButton.cs");
        Texture2D buttonScriptIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/left_click.svg");
        AddCustomType("TransitionButton", "Button", buttonScript, buttonScriptIcon);

        Script instantTransition = GD.Load<Script>($"{Constants.BASE_PATH}Transitions/InstantTransition.cs");
        Texture2D instantTransitionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/fastForward.png");
        AddCustomType("InstantTransition", "Control", instantTransition, instantTransitionIcon);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("TransitionButton");
        RemoveCustomType("InstantTransition");
    }
}
#endif
