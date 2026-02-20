using Godot;

namespace First3dGame.Models;

public partial class Player : CharacterBody3D
{
    [Export] public float Speed { get; set; } = 14;
    [Export] public float FallAcceleration { get; set; } = 75;
    [Export] public float JumpImpulse { get; set; } = 20;
    [Export] public float BounceImpulse { get; set; } = 16;

    [Signal] public delegate void HitEventHandler();

    private Vector3 _targetVelocity = Vector3.Zero;
    private AnimationPlayer _animationPlayer;
    private Node3D _pivot;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _pivot = GetNode<Node3D>("Pivot");
    }

    public override void _PhysicsProcess(double delta)
    {
        CalculateMovement((float)delta);
        CalculateGravityAndJump((float)delta);
        CalculateCollisions((float)delta);

        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    private void CalculateMovement(float delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed(Actions.MoveForward))
            direction.Z -= 1;
        if (Input.IsActionPressed(Actions.MoveBack))
            direction.Z += 1;
        if (Input.IsActionPressed(Actions.MoveLeft))
            direction.X -= 1;
        if (Input.IsActionPressed(Actions.MoveRight))
            direction.X += 1;

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            Basis = Basis.LookingAt(direction, Vector3.Up);
        }

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        _animationPlayer.SpeedScale = direction != Vector3.Zero ? 2 : 1;
    }

    private void CalculateGravityAndJump(float delta)
    {
        var onFloor = IsOnFloor();

        if (!onFloor)
            _targetVelocity.Y -= FallAcceleration * (float)delta;

        if (onFloor && Input.IsActionPressed(Actions.Jump))
            _targetVelocity.Y = JumpImpulse;

        var angle = Mathf.Pi / 6.0f;
        var progress = Velocity.Y / JumpImpulse;
        _pivot.Rotation = new Vector3(
            angle * progress,
            _pivot.Rotation.Y,
            _pivot.Rotation.Z
        );
    }

    private void CalculateCollisions(float delta)
    {
        for (int i = 0; i < GetSlideCollisionCount(); ++i)
        {
            var collision = GetSlideCollision(i);
            if (collision.GetCollider() is not Mob mob)
                continue;

            if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
            {
                mob.Squash();
                _targetVelocity.Y = BounceImpulse;
                break;
            }
        }
    }

    private void Die()
    {
        EmitSignal(SignalName.Hit);
        QueueFree();
    }

    private void OnMobDetected(Node3D mob)
    {
        Die();
    }
}
