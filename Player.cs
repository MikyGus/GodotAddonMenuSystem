using Godot;

public partial class Player : Sprite2D
{
    private const float _speed = 300;
    private int goToRight = 1;

    public override void _PhysicsProcess(double delta)
    {
        Position = Position with { X = Position.X + (_speed * goToRight * (float)delta) };

        if (Position.X < 100)
        {
            goToRight = 1;
        }
        else if (Position.X > 800)
        {
            goToRight = -1;
        }
    }
}
