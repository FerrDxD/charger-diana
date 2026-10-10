using Godot;
using System;
using System.Threading.Tasks;

public partial class PrologChatUi : CanvasLayer
{
    private Label _chatLabel;
    private ColorRect _bg;
    private VBoxContainer _messageContainer;

    [Signal]
    public delegate void ChatClosedEventHandler();

    public override void _Ready()
    {
        _bg = GetNode<ColorRect>("Background");
        _messageContainer = GetNode<VBoxContainer>("Background/Margin/Scroll/VBox");
        
        // Hide all initial messages
        foreach (Control child in _messageContainer.GetChildren())
        {
            child.Hide();
        }
    }

    public async Task PlayChatSequence()
    {
        Show();
        // Reveal messages one by one
        var msgs = _messageContainer.GetChildren();
        foreach (Control msg in msgs)
        {
            await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
            msg.Show();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Visible && (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("interact") || @event.IsActionPressed("toggle_phone")))
        {
            Hide();
            EmitSignal(SignalName.ChatClosed);
            GetViewport().SetInputAsHandled();
        }
    }
}
