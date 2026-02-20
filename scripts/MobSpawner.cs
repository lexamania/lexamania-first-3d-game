using Godot;

namespace First3dGame;

public partial class MobSpawner : Node3D
{
    [Export] public PackedScene MobScene { get; set; }

    private Player _player;
    private Timer _timer;
    private PathFollow3D _spawnLocation;
    private ScoreLabel _scoreLable;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = GetParent().GetNode<Player>("Player");
        _timer = GetNode<Timer>("Timer");
        _spawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
        _scoreLable = GetParent().GetNode<ScoreLabel>("UserInterface/ScoreLabel");
    } 

    private void OnTimerTimeout()
    {
        _spawnLocation.ProgressRatio = GD.Randf();

        var mob = MobScene.Instantiate<Mob>();
        mob.Initialize(_spawnLocation.Position, _player.Position);
        mob.Squashed += _scoreLable.OnMobSquashed;

        AddChild(mob);
    }

    private void OnPlayerHit()
    {
        _timer.Stop();
    }
}
