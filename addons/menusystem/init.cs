#if TOOLS
using Godot;
using MenySystem.addons.menusystem;

[Tool]
public partial class init : EditorPlugin
{
    private const string TransitionButton = "TransitionButton";
    private const string QuitGame = "QuitGameTransitionOption";
    private const string PauseGame = "PauseGameTransitionOption";
    private const string TranslucentScreen = "TranslucentScreenTransitionOption";
    private const string TestPrintOption = "TestPrintOption";
    private const string DisableMenu = "DisableMenuOption";
    private const string EventOption = "InvokeEventOption";

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


        Script quitOption = GD.Load<Script>($"{Constants.BASE_PATH}TransitionOptions/QuitGameTransitionOption.cs");
        Texture2D quitOptionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/power_30dp.svg");
        AddCustomType(QuitGame, Constants.NODE_CONTROL, quitOption, quitOptionIcon);

        Script pauseOption = GD.Load<Script>($"{Constants.BASE_PATH}TransitionOptions/PauseGameTransitionOption.cs");
        Texture2D pauseOptionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/hourglass_empty_30dp.svg");
        AddCustomType(PauseGame, Constants.NODE_CONTROL, pauseOption, pauseOptionIcon);

        Script translucentOption = GD.Load<Script>($"{Constants.BASE_PATH}TransitionOptions/TranslucentScreenTransitionOption.cs");
        Texture2D translucentOptionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/blur_on_30dp.svg");
        AddCustomType(TranslucentScreen, Constants.NODE_CONTROL, translucentOption, translucentOptionIcon);

        Script testOption = GD.Load<Script>($"{Constants.BASE_PATH}TransitionOptions/TestPrintTransitionOption.cs");
        Texture2D testOptionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/experiment_30dp.svg");
        AddCustomType(TestPrintOption, Constants.NODE_CONTROL, testOption, testOptionIcon);

        Script eventOption = GD.Load<Script>($"{Constants.BASE_PATH}TransitionOptions/InvokeEventTransitionOption.cs");
        Texture2D eventOptionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/cell_tower_30dp.svg");
        AddCustomType(EventOption, Constants.NODE_CONTROL, eventOption, eventOptionIcon);

        Script disableMenuOption = GD.Load<Script>($"{Constants.BASE_PATH}DisableOption/MenuDisableOption.cs");
        Texture2D disableMenuOptionIcon = GD.Load<Texture2D>($"{Constants.BASE_PATH}Art/blur_on_30dp_red.svg");
        AddCustomType(DisableMenu, Constants.NODE_CONTROL, disableMenuOption, disableMenuOptionIcon);

        AddAutoloadSingleton("MenuController", $"{Constants.BASE_PATH}MenuController.tscn");
    }

    public override void _ExitTree()
    {
        RemoveCustomType(TransitionButton);
        RemoveCustomType("InstantTransition");
        RemoveCustomType("MoveTransition");
        RemoveCustomType("FadeTransition");
        RemoveCustomType(QuitGame);
        RemoveCustomType(PauseGame);
        RemoveCustomType(TranslucentScreen);
        RemoveCustomType(TestPrintOption);
        RemoveCustomType(DisableMenu);
        RemoveCustomType(EventOption);
        RemoveAutoloadSingleton("MenuController");
    }
}
#endif
