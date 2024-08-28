using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public partial class ClearTransitionOptions : TransitionOption
{
    [Export]
    private bool _pausedGame;
    [Export]
    private bool _translucentScreen;

    public override void ActionOnFromMenu()
    {
        if (_pausedGame)
        {
            var pgo = new PauseGameTransitionOption();
            AddChild(pgo);
            pgo.ActionOnReturnToMenu();
            pgo.QueueFree();
        }
        if (_translucentScreen)
        {
            var tso = new TranslucentScreenTransitionOption()
            {
                TranslucentLevel = 0,
                FadeTime = 0,
            };
            AddChild(tso);
            tso.ActionOnReturnToMenu();
            tso.QueueFree();
        }
    }

    public override void ActionOnReturnToMenu()
    {
    }
}
