using Godot;
using MenuControl.addons.MenuControl.Helpers;
using MenySystem.addons.menusystem.Buttons;
using MenySystem.addons.menusystem.TransitionOptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
public abstract partial class Transition : Control
{
    public event Action<Control, Control> OnPreTransition;
    public event Action<Control, Control> OnPostTransition;
    public event Action<Control> OnPostPageFromTransition;
    public event Action<Control> OnPostPageToTransition;

    protected Vector2 ViewPortSize => GetViewportRect().Size;
    public TransitionButton TransitionBttnFrom { get; private set; }
    public TransitionButton TransitionBttnTo { get; private set; }

    public async Task PerformTransition(Control transitionFrom, Control transitionTo, TransitionButton transitionButtonFrom, TransitionButton transitionButtonTo)
    {
        ArgumentNullException.ThrowIfNull(transitionFrom, nameof(transitionFrom));
        ArgumentNullException.ThrowIfNull(transitionTo, nameof(transitionTo));

        TransitionBttnFrom = transitionButtonFrom;
        TransitionBttnTo = transitionButtonTo;
        await PerformTransition(transitionFrom, transitionTo);
    }

    protected abstract Task PerformTransition(Control transitionFrom, Control transitionTo);

    protected void RaiseOnPostPageFromTransition(Control menu)
    {
        TransitionBttnFrom?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenToPerformAction == WhenToDoAction.AfterPageFromTransition)
            .ForEach(x => x.AfterPageFromTransition_Set());
        TransitionBttnTo?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenToPerformAction == WhenToDoAction.AfterPageFromTransition)
            .ForEach(x => x.AfterPageFromTransition_UnSet());

        OnPostPageFromTransition?.Invoke(menu);
    }

    protected void RaiseOnPostPageToTransition(Control menu)
    {
        TransitionBttnFrom?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenToPerformAction == WhenToDoAction.AfterPageToTransition)
            .ForEach(x => x.AfterPageToTransition_Set());
        TransitionBttnTo?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenToPerformAction == WhenToDoAction.AfterPageToTransition)
            .ForEach(x => x.AfterPageToTransition_UnSet());

        OnPostPageToTransition?.Invoke(menu);
    }

    protected void RaiseOnPreTransition(Control menuFrom, Control menuTo)
    {
        TransitionBttnFrom?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenToPerformAction == WhenToDoAction.BeforeAllTransition)
            .ForEach(x => x.BeforeAllTransitions_Set());
        TransitionBttnTo?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenToPerformAction == WhenToDoAction.BeforeAllTransition)
            .ForEach(x => x.BeforeAllTransitions_UnSet());

        OnPreTransition?.Invoke(menuFrom, menuTo);
    }

    protected void RaiseOnPostTransition(Control menuFrom, Control menuTo)
    {
        TransitionBttnFrom?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenToPerformAction == WhenToDoAction.AfterAllTransition)
            .ForEach(x => x.AfterAllTransitions_Set());
        TransitionBttnTo?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenToPerformAction == WhenToDoAction.AfterAllTransition)
            .ForEach(x => x.AfterAllTransitions_UnSet());

        OnPostTransition?.Invoke(menuFrom, menuTo);
    }
}
