using Godot;
using System.Threading.Tasks;

namespace MenySystem.addons.menusystem.Transitions;
public abstract partial class Transition : Control
{
    protected Vector2 ViewPortSize => GetViewportRect().Size;
    public abstract Task PerformTransition(Control transitionFrom, Control transitionTo);
}
