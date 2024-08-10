using Godot;
using MenySystem.addons.menusystem.PropertyTypes;
using System;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
[Tool]
public partial class MoveTransition : Transition
{
    [Export]
    public MoveDirection MoveDirection { get; private set; }
    [Export(PropertyHint.Range, "0,10,0.1")]
    public float TransitionTime { get; private set; } = 0.4f;
    [Export]
    public Tween.TransitionType TransitionType { get; private set; }
    [Export]
    public Tween.EaseType EaseType { get; private set; } = Tween.EaseType.Out;

    public override async Task PerformTransition(Control transitionFrom, Control transitionTo)
    {
        ArgumentNullException.ThrowIfNull(transitionFrom, nameof(transitionFrom));
        ArgumentNullException.ThrowIfNull(transitionTo, nameof(transitionTo));

        (Vector2 start, Vector2 final) transitionOutPositions = GetPositions(MoveDirection, false);
        (Vector2 start, Vector2 final) transitionInPositions = GetPositions(MoveDirection, true);

        transitionTo.GlobalPosition = transitionInPositions.start;
        transitionTo.Visible = true;

        Tween tween = CreateTween();
        tween.Parallel().TweenProperty(transitionTo, Constants.PROPERTY_POSITION, transitionInPositions.final, TransitionTime)
            .SetTrans(TransitionType)
            .SetEase(EaseType);
        tween.Parallel().TweenProperty(transitionFrom, Constants.PROPERTY_POSITION, transitionOutPositions.final, TransitionTime)
            .SetTrans(TransitionType)
            .SetEase(EaseType);
        await ToSignal(tween, Constants.EVENT_FINISHED);
    }

    public (Vector2 start, Vector2 final) GetPositions(MoveDirection direction, bool IsTransitioningTo)
    => (direction, IsTransitioningTo) switch
    {
        (MoveDirection.Up, true) => (new Vector2(0, ViewPortSize.Y), Vector2.Zero),
        (MoveDirection.Up, false) => (Vector2.Zero, new Vector2(0, -ViewPortSize.Y)),
        (MoveDirection.Right, true) => (new Vector2(-ViewPortSize.X, 0), Vector2.Zero),
        (MoveDirection.Right, false) => (Vector2.Zero, new Vector2(ViewPortSize.X, 0)),
        (MoveDirection.Down, true) => (new Vector2(0, -ViewPortSize.Y), Vector2.Zero),
        (MoveDirection.Down, false) => (Vector2.Zero, new Vector2(0, ViewPortSize.Y)),
        (MoveDirection.Left, true) => (new Vector2(ViewPortSize.X, 0), Vector2.Zero),
        (MoveDirection.Left, false) => (Vector2.Zero, new Vector2(-ViewPortSize.X, 0)),
        _ => throw new ArgumentOutOfRangeException(nameof(direction)),
    };
}
