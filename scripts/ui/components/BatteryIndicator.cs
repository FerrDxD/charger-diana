using Godot;

/// <summary>
/// Battery indicator component following DESIGN.md.
/// Shows pixel battery icon with fill and percent label.
/// Normal color: battery_ok (#3E7CB1), Low color: battery_low (#D9A520) when below threshold (< 10%).
/// </summary>
public partial class BatteryIndicator : Control
{
	[Export] public float LowBatteryThreshold { get; set; } = 10f;
	[Export] public string Prefix { get; set; } = "";

	private ColorRect _fill;
	private Label _percentLabel;
	private readonly Color _colorOk = new Color(0.243137f, 0.486275f, 0.694118f, 1f); // battery_ok (#3E7CB1)
	private readonly Color _colorLow = new Color(0.850980f, 0.647059f, 0.125490f, 1f); // battery_low (#D9A520)

	public override void _Ready()
	{
		_fill = GetNode<ColorRect>("HBoxContainer/IconContainer/InnerBg/Fill");
		_percentLabel = GetNode<Label>("HBoxContainer/PercentLabel");
	}

	/// <summary>Update battery percentage (0-100).</summary>
	public void SetBattery(float percent)
	{
		percent = Mathf.Clamp(percent, 0f, 100f);
		if (_percentLabel != null)
		{
			string text = string.IsNullOrEmpty(Prefix) ? $"{Mathf.Floor(percent)}%" : $"{Prefix} {Mathf.Floor(percent)}%";
			_percentLabel.Text = text;
		}

		Color targetColor = percent < LowBatteryThreshold ? _colorLow : _colorOk;

		if (_fill != null)
		{
			// Inner width is 20px
			float fillWidth = Mathf.Lerp(0f, 20f, percent / 100f);
			_fill.Size = new Vector2(fillWidth, 10f);
			_fill.Color = targetColor;
		}

		if (_percentLabel != null)
		{
			_percentLabel.Modulate = targetColor;
		}
	}
}
