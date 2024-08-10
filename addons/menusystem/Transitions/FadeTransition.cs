using Godot;
using MenySystem.addons.menusystem.PropertyTypes;
using System;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
[Tool]
public partial class FadeTransition : Transition
{
    [Export]
    public FadeType FadeType { get; set; } = FadeType.In | FadeType.Out;
    [Export(PropertyHint.Range, "0,10,0.1")]
    public float TransitionTime { get; private set; } = 1f;

    public override async Task PerformTransition(Control transitionFrom, Control transitionTo)
    {
        ArgumentNullException.ThrowIfNull(transitionFrom, nameof(transitionFrom));
        ArgumentNullException.ThrowIfNull(transitionTo, nameof(transitionTo));

        transitionTo.Visible = false;
        transitionTo.GlobalPosition = Vector2.Zero;

        if (FadeType.HasFlag(FadeType.Out))
        {
            await MenuController.Instance.CoverScreen.FadeToAlpha(1, TransitionTime);
            transitionFrom.Visible = false;
        }
        if (FadeType.HasFlag(FadeType.In))
        {
            transitionTo.Visible = true;
            await MenuController.Instance.CoverScreen.FadeToAlpha(0, TransitionTime);
        }
    }


}
