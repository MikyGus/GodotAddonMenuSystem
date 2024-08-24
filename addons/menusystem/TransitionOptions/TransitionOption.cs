using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public abstract partial class TransitionOption : Control
{
    [Export]
    public WhenToDoAction WhenLeavingMenu { get; protected set; } = WhenToDoAction.AfterAllTransition;
    [Export]
    public WhenToDoAction WhenReturningToMenu { get; protected set; } = WhenToDoAction.AfterAllTransition;

    public abstract void ActionOnFromMenu();
    public abstract void ActionOnReturnToMenu();
}