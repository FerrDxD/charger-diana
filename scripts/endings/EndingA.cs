using Godot;
using System;

public partial class EndingA : Control
{
    private SceneManager _sceneManager;

    public override async void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        _sceneManager = GetNode<SceneManager>("/root/SceneManager");

        await ToSignal(GetTree().CreateTimer(5.0f), SceneTreeTimer.SignalName.Timeout);
        _sceneManager.GoTo(GameFlow.Credits);
    }
}
