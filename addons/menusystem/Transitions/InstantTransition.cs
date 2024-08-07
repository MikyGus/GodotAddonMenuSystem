using Godot;
using System;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
[Tool]
public partial class InstantTransition : Transition
{
    public override async Task PerformTransition(Control transitionFrom, Control transitionTo)
    {
        ArgumentNullException.ThrowIfNull(transitionFrom, nameof(transitionFrom));
        ArgumentNullException.ThrowIfNull(transitionTo, nameof(transitionTo));

        //transitionFrom.MenuController.PrePageTransition(transitionFrom);
        transitionFrom.Visible = false;
        //transitionFrom.MenuController.PostPageTransition(transitionFrom);

        //transitionTo.MenuController.PrePageTransition(transitionTo);
        transitionTo.GlobalPosition = Vector2.Zero;
        transitionTo.Visible = true;
        //transitionTo.MenuController.PostPageTransition(transitionTo);

        await Task.CompletedTask;
    }
}
