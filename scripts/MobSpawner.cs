using Godot;

namespace First3dGame;

public partial class MobSpawner : Node3D
{
    [Export] public PackedScene MobScene { get; set; }

    private Player _player;
    private Timer _timer;
    private PathFollow3D _spawnLocation;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = GetParent().GetNode<Player>("Player");
        _timer = GetNode<Timer>("Timer");
        _spawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
    }

    private void OnTimerTimeout()
    {
        var mob = MobScene.Instantiate<Mob>();
        _spawnLocation.ProgressRatio = GD.Randf();
        mob.Initialize(_spawnLocation.Position, _player.Position);

        AddChild(mob);
    }

    private void OnPlayerHit()
    {
        _timer.Stop();
    }
}
