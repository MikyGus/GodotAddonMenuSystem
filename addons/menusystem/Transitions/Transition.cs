using Godot;
using System;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
public abstract partial class Transition : Control
{
    public event Action<Control, Control> OnPreTransition;
    public event Action<Control, Control> OnPostTransition;
    public event Action<Control> OnPostPageFromTransition;
    public event Action<Control> OnPostPageToTransition;

    protected Vector2 ViewPortSize => GetViewportRect().Size;
    public abstract Task PerformTransition(Control transitionFrom, Control transitionTo);

    protected void RaiseOnPostPageFromTransition(Control menu) => OnPostPageFromTransition?.Invoke(menu);
    protected void RaiseOnPostPageToTransition(Control menu) => OnPostPageToTransition?.Invoke(menu);
    protected void RaiseOnPreTransition(Control menuFrom, Control menuTo) => OnPreTransition?.Invoke(menuFrom, menuTo);
    protected void RaiseOnPostTransition(Control menuFrom, Control menuTo) => OnPreTransition?.Invoke(menuFrom, menuTo);
}
