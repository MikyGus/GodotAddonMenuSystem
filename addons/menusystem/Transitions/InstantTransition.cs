using Godot;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
[Tool]
public partial class InstantTransition : Transition
{
    protected override async Task PerformTransition(Control transitionFrom, Control transitionTo)
    {
        RaiseOnPreTransition(transitionFrom, transitionTo);

        transitionFrom.Visible = false;
        RaiseOnPostPageFromTransition(transitionFrom);

        transitionTo.GlobalPosition = Vector2.Zero;
        transitionTo.Visible = true;
        RaiseOnPostPageToTransition(transitionTo);

        RaiseOnPostTransition(transitionFrom, transitionTo);

        await Task.CompletedTask;
    }
}
