using Godot;
using MenySystem.addons.menusystem;
using System.Threading.Tasks;

public partial class FadeScreen : ColorRect
{
    public async Task FadeToAlpha(float alpha, float fadeTime)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this, Constants.PROPERTY_MODULATE_A, alpha, fadeTime);
        await ToSignal(tween, Constants.EVENT_FINISHED);
    }
}
