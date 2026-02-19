using Godot;

namespace First3dGame;

public partial class Mob : CharacterBody3D
{
    [Export] public float MinSpeed = 10;
    [Export] public float MaxSpeed = 18;

    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
        RotateY((float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4));

        var speed = (float)GD.RandRange(MinSpeed, MaxSpeed);
        Velocity = Vector3.Forward * speed;
        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    private void OnScreenExited()
    {
        QueueFree();
    }
}
