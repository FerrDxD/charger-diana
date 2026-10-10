using Godot;

/// <summary>
/// Intro screen with location title and battery status.
/// Minimalist layout, VT323 font, no panels.
/// </summary>
public partial class Intro : Control
{
	private Label _subliminalQuote;
	private bool _triggered = false;

	public override void _Ready()
	{
		_subliminalQuote = GetNode<Label>("CenterContainer/VBoxContainer/SubliminalQuote");

		// Show subliminal quote after 3.5 seconds
		GetTree().CreateTimer(3.5).Timeout += () =>
		{
			_subliminalQuote.Modulate = new Color(1, 1, 1, 0.4f);
		};
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept") || @event.IsActionPressed("ui_select"))
		{
			Proceed();
		}
	}

	private void Proceed()
	{
		if (_triggered) return;
		_triggered = true;

		// TODO: Load main game scene
		GD.Print("Proceeding to game...");
		GetTree().ChangeSceneToFile("res://scenes/level/TestLevel.tscn");
	}
}
