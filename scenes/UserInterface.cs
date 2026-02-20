using Godot;

namespace First3dGame.Scenes;

public partial class UserInterface : Control
{
    private Main _scene;
    private Label _scoreLabel;
    private ColorRect _retryButton;

    public override void _Ready()
    {
        _scene = GetParent<Main>();
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _retryButton = GetNode<ColorRect>("Retry");

        _retryButton.Hide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") && _retryButton.Visible)
        {
            _scene.RestartLevel();
        }
    }

    public void UpdateScore(int score)
    {
        _scoreLabel.Text = $"Score: {score}";
    }

    public void ShowRetry()
    {
        _retryButton.Show();
    }
}
