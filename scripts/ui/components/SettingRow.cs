using Godot;

/// <summary>
/// Reusable setting row component following DESIGN.md.
/// Left: setting title (VT323, bone color).
/// Right: container for custom setting control (toggle, slider, dropdown).
/// </summary>
public partial class SettingRow : HBoxContainer
{
	[Export] public string SettingTitle { get; set; } = "PENGATURAN";

	private Label _titleLabel;
	private Control _controlSlot;

	public override void _Ready()
	{
		_titleLabel = GetNode<Label>("TitleLabel");
		_controlSlot = GetNode<Control>("ControlSlot");
		if (_titleLabel != null && !string.IsNullOrEmpty(SettingTitle))
		{
			_titleLabel.Text = SettingTitle;
		}
	}

	public void SetTitle(string title)
	{
		SettingTitle = title;
		if (_titleLabel != null)
		{
			_titleLabel.Text = title;
		}
	}

	public Control GetControlSlot() => _controlSlot;
}
