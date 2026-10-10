using Godot;

/// <summary>
/// Reusable menu button component according to DESIGN.md.
/// Normal: bone color text, no prefix.
/// Selected (hover/focus): blood color (#B3261E) with "> " prefix.
/// Flat style without background box.
/// </summary>
public partial class MenuButton : Button
{
	[Export] public string RawText { get; set; } = "";

	private readonly Color _colorBone = new Color(0.847059f, 0.827451f, 0.768627f, 1f); // bone (#D8D3C4)
	private readonly Color _colorBlood = new Color(0.701961f, 0.149020f, 0.117647f, 1f); // blood (#B3261E)
	private bool _isSelected = false;

	public override void _Ready()
	{
		Flat = true;
		if (string.IsNullOrEmpty(RawText))
		{
			RawText = Text;
		}

		FocusEntered += OnSelected;
		FocusExited += OnDeselected;
		MouseEntered += OnSelected;
		MouseExited += OnMouseExited;

		UpdateVisuals();
	}

	public void SetButtonText(string text)
	{
		RawText = text;
		UpdateVisuals();
	}

	private void OnSelected()
	{
		_isSelected = true;
		UpdateVisuals();
	}

	private void OnDeselected()
	{
		_isSelected = false;
		UpdateVisuals();
	}

	private void OnMouseExited()
	{
		if (!HasFocus())
		{
			_isSelected = false;
			UpdateVisuals();
		}
	}

	private void UpdateVisuals()
	{
		if (_isSelected)
		{
			Text = $"> {RawText}";
			AddThemeColorOverride("font_color", _colorBlood);
			AddThemeColorOverride("font_focus_color", _colorBlood);
			AddThemeColorOverride("font_hover_color", _colorBlood);
			AddThemeColorOverride("font_pressed_color", _colorBlood);
		}
		else
		{
			Text = RawText;
			AddThemeColorOverride("font_color", _colorBone);
			AddThemeColorOverride("font_focus_color", _colorBlood);
			AddThemeColorOverride("font_hover_color", _colorBlood);
			AddThemeColorOverride("font_pressed_color", _colorBlood);
		}
	}
}
