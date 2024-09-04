using Godot;

public partial class LoadingTexture : TextureRect
{
    private const double _rotationSpeed = 100;

    public override void _PhysicsProcess(double delta)
    {
        RotationDegrees += (float)(_rotationSpeed * delta);
    }
}
