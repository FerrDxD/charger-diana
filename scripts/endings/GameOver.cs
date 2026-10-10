using Godot;
using System;

public partial class GameOver : Control
{
    private SceneManager _sceneManager;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        _sceneManager = GetNode<SceneManager>("/root/SceneManager");

        var retryBtn = GetNode<Button>("VBoxContainer/RetryButton");
        var menuBtn = GetNode<Button>("VBoxContainer/MenuButton");

        retryBtn.Pressed += () => _sceneManager.GoTo(GameFlow.Gameplay);
        menuBtn.Pressed += () => _sceneManager.GoTo(GameFlow.MainMenu);
    }
}
