using Godot;

/// <summary>
/// Thin stamina bar component following DESIGN.md.
/// 4px height, bone color fill, ash background.
/// Signal-driven via SetStamina — no per-frame polling in _Process.
/// </summary>
public partial class StaminaBar : ProgressBar
{
	[Export] public float LowStaminaThreshold { get; set; } = 20f;

	private readonly Color _colorNormal = new Color(0.847059f, 0.827451f, 0.768627f, 1f); // bone (#D8D3C4)
	private readonly Color _colorLow = new Color(0.850980f, 0.647059f, 0.125490f, 1f); // battery_low (#D9A520)

	public override void _Ready()
	{
		UpdateColor();
	}

	/// <summary>Update stamina value (0-100) event-driven.</summary>
	public void SetStamina(float percent)
	{
		Value = Mathf.Clamp(percent, 0f, 100f);
		UpdateColor();
	}

	private void UpdateColor()
	{
		Color targetColor = Value < LowStaminaThreshold ? _colorLow : _colorNormal;
		Modulate = targetColor;
	}
}
