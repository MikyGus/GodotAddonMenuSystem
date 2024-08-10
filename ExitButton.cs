using Godot;
using MenySystem.addons.menusystem.Transitions;

public partial class ExitButton : Node
{
    private Transition _transition;
    public override void _Ready()
    {
        _transition = GetNode<FadeTransition>("../FadeTransition");
        _transition.OnPostPageToTransition += OnPostPage;
    }

    public override void _ExitTree()
    {
        _transition.OnPostPageToTransition -= OnPostPage;
    }

    private void OnPostPage(Control control)
    {
        GetTree().Quit();
    }
}
