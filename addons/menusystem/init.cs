#if TOOLS
using Godot;
using MenySystem.addons.menusystem;

[Tool]
public partial class init : EditorPlugin
{
    private const string TransitionButton = "TransitionButton";

    public override void _EnterTree()
    {
        Script buttonScript = GD.Load<Script>($"{Constants.BASE_PATH}Buttons/{TransitionButton}.cs");
        Texture2D buttonScriptIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/left_click.svg");
        AddCustomType(TransitionButton, Constants.NODE_CONTROL, buttonScript, buttonScriptIcon);

        Script instantTransition = GD.Load<Script>($"{Constants.BASE_PATH}Transitions/InstantTransition.cs");
        Texture2D instantTransitionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/fastForward.png");
        AddCustomType("InstantTransition", Constants.NODE_CONTROL, instantTransition, instantTransitionIcon);

        Script moveTransition = GD.Load<Script>($"{Constants.BASE_PATH}Transitions/MoveTransition.cs");
        Texture2D moveTransitionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/arrowRight.png");
        AddCustomType("MoveTransition", Constants.NODE_CONTROL, moveTransition, moveTransitionIcon);

        Script fadeTransition = GD.Load<Script>($"{Constants.BASE_PATH}Transitions/FadeTransition.cs");
        Texture2D fadeTransitionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/contrast.png");
        AddCustomType("FadeTransition", Constants.NODE_CONTROL, fadeTransition, fadeTransitionIcon);

        AddAutoloadSingleton("MenuController", $"{Constants.BASE_PATH}MenuController.cs");
    }

    public override void _ExitTree()
    {
        RemoveCustomType(TransitionButton);
        RemoveCustomType("InstantTransition");
        RemoveCustomType("MoveTransition");
        RemoveCustomType("FadeTransition");
        RemoveAutoloadSingleton("MenuController");
    }
}
#endif
