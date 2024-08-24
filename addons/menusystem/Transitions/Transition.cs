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

    private void RaiseOptionSetUnset(WhenToDoAction whenToDoAction)
    {
        TransitionBttnFrom?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenLeavingMenu == whenToDoAction)
            .ForEach(x => x.ActionOnFromMenu());
        TransitionBttnTo?.GetAllChildren<TransitionOption>()
            .Where(x => x.WhenReturningToMenu == whenToDoAction)
            .ForEach(x => x.ActionOnReturnToMenu());
    }

    protected void RaiseOnPostPageFromTransition(Control menu)
    {
        RaiseOptionSetUnset(WhenToDoAction.AfterPageFromTransition);
        OnPostPageFromTransition?.Invoke(menu);
    }

    protected void RaiseOnPostPageToTransition(Control menu)
    {
        RaiseOptionSetUnset(WhenToDoAction.AfterPageToTransition);
        OnPostPageToTransition?.Invoke(menu);
    }

    protected void RaiseOnPreTransition(Control menuFrom, Control menuTo)
    {
        RaiseOptionSetUnset(WhenToDoAction.BeforeAllTransition);
        OnPreTransition?.Invoke(menuFrom, menuTo);
    }

    protected void RaiseOnPostTransition(Control menuFrom, Control menuTo)
    {
        RaiseOptionSetUnset(WhenToDoAction.AfterAllTransition);
        OnPostTransition?.Invoke(menuFrom, menuTo);
    }
}
