using Godot;

namespace First3dGame;

public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 14;
    [Export] public float FallAcceleration = 75;
    [Export] public float JumpPower = 20;
    [Export] public required Node3D Pivot;

    private Vector3 _targetVelocity = Vector3.Zero;

    public override void _Ready()
    {
        Pivot = GetNode<Node3D>("Pivot");
    }

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed(ActionsEnum.MoveForward))
            direction.Z -= 1;
        if (Input.IsActionPressed(ActionsEnum.MoveBack))
            direction.Z += 1;
        if (Input.IsActionPressed(ActionsEnum.MoveLeft))
            direction.X -= 1;
        if (Input.IsActionPressed(ActionsEnum.MoveRight))
            direction.X += 1;

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            Pivot.Basis = Basis.LookingAt(direction);
        }

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        if (!IsOnFloor())
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        else if (Input.IsActionPressed(ActionsEnum.Jump))
            _targetVelocity.Y = JumpPower;

        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
