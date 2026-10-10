using Godot;
using System;

public partial class EndingB : Control
{
    private SceneManager _sceneManager;
    private Control _dialogueBox;
    private Label _tamatLabel;

    public override async void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        _sceneManager = GetNode<SceneManager>("/root/SceneManager");

        _dialogueBox = GetNode<Control>("DialogueContainer/DialogueBox");
        _tamatLabel = GetNode<Label>("CenterContainer/TamatText");

        _tamatLabel.Visible = false;

        await ToSignal(GetTree().CreateTimer(4.0f), SceneTreeTimer.SignalName.Timeout);
        
        _dialogueBox.Visible = false;
        _tamatLabel.Visible = true;

        await ToSignal(GetTree().CreateTimer(4.0f), SceneTreeTimer.SignalName.Timeout);

        _sceneManager.GoTo(GameFlow.Credits);
    }
}
