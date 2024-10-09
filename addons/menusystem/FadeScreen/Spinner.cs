using Godot;
using MenySystem.addons.menusystem;

public partial class Spinner : Control
{
    private TextureRect _loadingTexture;
    private float _scaleTransitionTime = .3f;
    private float _selfModulateTransitionTime = 1.0f;
    private Tween _tween;

    public override void _Ready()
    {
        _loadingTexture = GetNode<TextureRect>("LoadingTexture");
        _loadingTexture.Scale = Vector2.Zero;
        _loadingTexture.SelfModulate = _loadingTexture.SelfModulate with { A = 0 };
    }

    public void StartLoading()
    {
        _tween = CreateTween();
        _ = _tween.Parallel().TweenProperty(_loadingTexture, Constants.PROPERTY_SCALE, new Vector2(1, 1), _scaleTransitionTime + 2);
        _ = _tween.Parallel().TweenProperty(_loadingTexture, Constants.PROPERTY_SELF_MODULATE_A, 1, _selfModulateTransitionTime);
    }

    public void EndLoading()
    {
        if (_tween.IsRunning())
        {
            _tween.Stop();
        }
        _tween = CreateTween();
        _ = _tween.Parallel().TweenProperty(_loadingTexture, Constants.PROPERTY_SCALE, new Vector2(0, 0), _scaleTransitionTime);
        _ = _tween.Parallel().TweenProperty(_loadingTexture, Constants.PROPERTY_SELF_MODULATE_A, 0, _selfModulateTransitionTime - 0.7f);
    }
}