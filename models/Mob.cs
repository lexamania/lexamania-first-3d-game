using Godot;

namespace First3dGame.Models;

public partial class Mob : CharacterBody3D
{
    [Export] public float MinSpeed = 10;
    [Export] public float MaxSpeed = 18;

    [Signal] public delegate void SquashedEventHandler();

    private Vector3 LocalVelocity;
    private AnimationPlayer _animationPlayer;

    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
        RotateY((float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4));

        var speed = (float)GD.RandRange(MinSpeed, MaxSpeed);

        LocalVelocity = Vector3.Forward * speed;
        LocalVelocity = LocalVelocity.Rotated(Vector3.Up, Rotation.Y);
        Velocity = LocalVelocity;

        var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.SpeedScale = speed / MinSpeed;
    }

    public void Squash()
    {
        EmitSignal(SignalName.Squashed);
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = LocalVelocity;
        MoveAndSlide();
    }

    private void OnScreenExited()
    {
        QueueFree();
    }
}
