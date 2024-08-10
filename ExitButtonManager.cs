using Godot;
using MenySystem.addons.menusystem.Transitions;

public partial class ExitButtonManager : Node
{
    private Transition _transition;
    public override void _Ready()
    {
        _transition = GetNode<FadeTransition>("../FadeTransition");
        _transition.OnPostPageFromTransition += OnPostPage;
    }

    public override void _ExitTree()
    {
        _transition.OnPostPageFromTransition -= OnPostPage;
    }

    private void OnPostPage(Control control)
    {
        GetTree().Quit();
    }
}
