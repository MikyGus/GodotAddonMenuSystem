using Godot;

namespace MenySystem.addons.menusystem.TransitionOptions;
public abstract partial class TransitionOption : Control
{
    [Export]
    public WhenToDoAction WhenToPerformAction { get; protected set; } = WhenToDoAction.AfterAllTransition;

    public virtual void BeforeAllTransitions_Set() { }
    public virtual void BeforeAllTransitions_UnSet() { }
    public virtual void AfterAllTransitions_Set() { }
    public virtual void AfterAllTransitions_UnSet() { }
    public virtual void AfterPageFromTransition_Set() { }
    public virtual void AfterPageFromTransition_UnSet() { }
    public virtual void AfterPageToTransition_Set() { }
    public virtual void AfterPageToTransition_UnSet() { }
}