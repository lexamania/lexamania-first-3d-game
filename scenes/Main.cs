using First3dGame.Models;
using Godot;

namespace First3dGame.Scenes;

public partial class Main : Node3D
{
    private int _score = 0;

    public UserInterface UI { get; private set; }
    public Player Player { get; private set; }
    public MobSpawner MobSpawner { get; private set; }

    public override void _Ready()
    {
        UI = GetNode<UserInterface>("UserInterface");
        Player = GetNode<Player>("Player");
        MobSpawner = GetNode<MobSpawner>("MobSpawner");
    }

    public void RestartLevel()
    {
        GetTree().ReloadCurrentScene();
    }

    public void OnMobSquashed()
    {
        _score += 1;
        UI.UpdateScore(_score);
    }

    private void OnPlayerHit()
    {
        MobSpawner.StopSpawn();
        UI.ShowRetry();
    }
}