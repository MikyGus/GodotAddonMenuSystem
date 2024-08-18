using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public abstract partial class TransitionOption : Control
{
    [Export]
    public WhenToDoAction WhenToPerformAction { get; protected set; } = WhenToDoAction.AfterAllTransition;

    public abstract void SetAction();
    public abstract void UnsetAction();
}