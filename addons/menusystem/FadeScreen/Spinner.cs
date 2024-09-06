using Godot;
using MenySystem.addons.menusystem;

public partial class Spinner : Control
{
    private TextureRect _loadingTexture;
    private float _scaleTransitionTime = 0.3f;
    private float _selfModulateTransitionTime = 1.0f;

    public override void _Ready()
    {
        _loadingTexture = GetNode<TextureRect>("LoadingTexture");
        _loadingTexture.Scale = Vector2.Zero;
        _loadingTexture.SelfModulate = _loadingTexture.SelfModulate with { A = 0 };
    }

    public void StartLoading()
    {
        Tween tween = CreateTween();
        tween.Parallel().TweenProperty(_loadingTexture, Constants.PROPERTY_SCALE, new Vector2(1, 1), _scaleTransitionTime);
        tween.Parallel().TweenProperty(_loadingTexture, Constants.PROPERTY_SELF_MODULATE_A, 1, _selfModulateTransitionTime);
    }

    public void EndLoading()
    {
        Tween tween = CreateTween();
        tween.Parallel().TweenProperty(_loadingTexture, Constants.PROPERTY_SCALE, new Vector2(0, 0), _scaleTransitionTime);
        tween.Parallel().TweenProperty(_loadingTexture, Constants.PROPERTY_SELF_MODULATE_A, 0, _selfModulateTransitionTime);
    }
}
