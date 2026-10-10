using Godot;

/// <summary>
/// Phone UI for flashlight toggle and notes gallery.
/// Diegetic HP frame with VT323 font, Special Elite for note previews.
/// </summary>
public partial class PhoneUi : Control
{
	private Button _flashlightTile;
	private Button _notesTile;
	private Label _batteryPercent;
	private ColorRect _batteryFill;
	private VBoxContainer _noteThumbnails;
	private int _selectedTile = 0;

	private bool _flashlightOn = true;
	private float _batteryLevel = 34.0f;

	public override void _Ready()
	{
		_flashlightTile = GetNode<Button>("PhoneFrame/MainArea/ActionTiles/FlashlightTile");
		_notesTile = GetNode<Button>("PhoneFrame/MainArea/ActionTiles/NotesTile");
		_batteryPercent = GetNode<Label>("PhoneFrame/StatusBar/BatteryContainer/BatteryPercent");
		_batteryFill = GetNode<ColorRect>("PhoneFrame/StatusBar/BatteryContainer/BatteryIcon/BatteryFill");
		_noteThumbnails = GetNode<VBoxContainer>("PhoneFrame/MainArea/NotesGrid");

		_flashlightTile.Pressed += OnFlashlightPressed;
		_notesTile.Pressed += OnNotesPressed;

		UpdateBatteryDisplay();
		UpdateTileSelection();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("ui_text_back"))
		{
			ClosePhone();
		}
		else if (@event.IsActionPressed("ui_left"))
		{
			_selectedTile = 0;
			UpdateTileSelection();
		}
		else if (@event.IsActionPressed("ui_right"))
		{
			_selectedTile = 1;
			UpdateTileSelection();
		}
		else if (@event.IsActionPressed("ui_accept"))
		{
			if (_selectedTile == 0)
				OnFlashlightPressed();
			else
				OnNotesPressed();
		}
	}

	private void OnFlashlightPressed()
	{
		_flashlightOn = !_flashlightOn;
		UpdateFlashlightTile();
		// TODO: Emit signal to GameManager to toggle flashlight
		GD.Print($"Flashlight toggled: {_flashlightOn}");
	}

	private void OnNotesPressed()
	{
		// TODO: Open notes gallery
		GD.Print("Notes gallery - not implemented yet");
	}

	private void UpdateFlashlightTile()
	{
		string status = _flashlightOn ? "AKTIF" : "MATI";
		string statusText = _flashlightOn ? "[STATUS: MENYALA]" : "[STATUS: MATI]";
		_flashlightTile.Text = $"[F] TOGGLE\n{status}\nSENTER\n{statusText}";
	}

	private void UpdateTileSelection()
	{
		// Update visual selection for tiles
		Color selectedColor = new Color(0.623529f, 0.721569f, 0.784314f, 1f);
		Color normalColor = new Color(0.243137f, 0.290196f, 0.227451f, 1f);

		_flashlightTile.Modulate = _selectedTile == 0 ? selectedColor : Colors.White;
		_notesTile.Modulate = _selectedTile == 1 ? selectedColor : Colors.White;
	}

	private void UpdateBatteryDisplay()
	{
		_batteryPercent.Text = $"{_batteryLevel:F0}%";
		float fillWidth = (_batteryLevel / 100.0f) * 20.0f;
		_batteryFill.CustomMinimumSize = new Vector2(fillWidth, 10.0f);

		// Change color to warn if below 10%
		if (_batteryLevel < 10.0f)
		{
			_batteryFill.Color = new Color(0.788235f, 0.635294f, 0.152941f, 1f);
		}
		else
		{
			_batteryFill.Color = new Color(0.623529f, 0.721569f, 0.784314f, 1f);
		}
	}

	private void ClosePhone()
	{
		// TODO: Emit signal to hide phone UI
		GD.Print("Phone closed");
		QueueFree();
	}
}
