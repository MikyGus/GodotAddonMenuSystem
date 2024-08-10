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
        AddCustomType("TransitionButton", Constants.NODE_BUTTON, buttonScript, buttonScriptIcon);

        Script instantTransition = GD.Load<Script>($"{Constants.BASE_PATH}Transitions/InstantTransition.cs");
        Texture2D instantTransitionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/fastForward.png");
        AddCustomType("InstantTransition", Constants.NODE_CONTROL, instantTransition, instantTransitionIcon);

        Script moveTransition = GD.Load<Script>($"{Constants.BASE_PATH}Transitions/MoveTransition.cs");
        Texture2D moveTransitionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/arrowRight.png");
        AddCustomType("MoveTransition", Constants.NODE_CONTROL, moveTransition, moveTransitionIcon);

        AddAutoloadSingleton("MenuController", $"{Constants.BASE_PATH}MenuController.cs");
    }

    public override void _ExitTree()
    {
        RemoveCustomType("TransitionButton");
        RemoveCustomType("InstantTransition");
        RemoveAutoloadSingleton("MenuController");
    }
}
#endif
