using First3dGame.Models;
using Godot;

namespace First3dGame.Scenes;

public partial class MobSpawner : Node3D
{
    [Export] public PackedScene MobScene { get; set; }

    private Main _scene;
    private Timer _timer;
    private PathFollow3D _spawnLocation;

    public void StopSpawn()
    {
        _timer.Stop();
    }

    public override void _Ready()
    {
        _scene = GetParent<Main>();
        _timer = GetNode<Timer>("Timer");
        _spawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
    }

    private void OnTimerTimeout()
    {
        _spawnLocation.ProgressRatio = GD.Randf();

        var mob = MobScene.Instantiate<Mob>();
        mob.Initialize(_spawnLocation.Position, _scene.Player.Position);
        mob.Squashed += _scene.OnMobSquashed;

        AddChild(mob);
    }
}
