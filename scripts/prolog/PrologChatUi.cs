using Godot;
using System;
using System.Threading.Tasks;

public partial class PrologChatUi : CanvasLayer
{
    private Control _messageContainer;
    private Label _hintLabel;
    private bool _sequenceCompleted = false;

    [Signal]
    public delegate void ChatClosedEventHandler();

    public override void _Ready()
    {
        _messageContainer = GetNode<Control>("Background/PhoneFrame/ChatArea/Scroll/VBox");
        _hintLabel = GetNode<Label>("Background/Hint");

        foreach (Control child in _messageContainer.GetChildren())
        {
            child.Hide();
        }
        _hintLabel.Hide();
        Hide();
    }

    public async Task PlayChatSequence()
    {
        Show();
        _sequenceCompleted = false;
        _hintLabel.Hide();

        var msgs = _messageContainer.GetChildren();
        for (int i = 0; i < msgs.Count; i++)
        {
            var msg = msgs[i] as Control;
            if (msg == null) continue;

            await ToSignal(GetTree().CreateTimer(1.2f), SceneTreeTimer.SignalName.Timeout);
            msg.Show();

            if (i % 2 == 0)
            {
                // Salsa message (incoming)
                AudioManager.Instance?.PlayUi("chat_receive");
                if (i == 0)
                {
                    // VO_P2_1: Diana murmurs "Ketinggalan di kelas..."
                    await ToSignal(GetTree().CreateTimer(0.8f), SceneTreeTimer.SignalName.Timeout);
                    AudioManager.Instance?.PlayVo("VO_P2_1", "Diana", "Ketinggalan di kelas...", 3.0f);
                }
            }
            else
            {
                // Diana message (outgoing)
                AudioManager.Instance?.PlayUi("chat_send");
            }
        }

        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        _sequenceCompleted = true;
        _hintLabel.Show();
    }

    public override void _Input(InputEvent @event)
    {
        if (!Visible) return;

        if (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("interact") || @event.IsActionPressed("toggle_phone"))
        {
            CloseChat();
            GetViewport().SetInputAsHandled();
        }
    }

    public void CloseChat()
    {
        Hide();
        EmitSignal(SignalName.ChatClosed);
    }
}
