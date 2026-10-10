using Godot;
using System.Threading.Tasks;

/// <summary>
/// Full-screen fade overlay for scene transitions following DESIGN.md.
/// CanvasLayer z-index 100, ink color (#0B0B0D).
/// </summary>
public partial class ScreenFader : ColorRect
{
	[Export] public float DefaultFadeDuration { get; set; } = 0.5f;

	private Tween _tween;

	public override void _Ready()
	{
		Color = new Color(0.0431373f, 0.0431373f, 0.0509804f, 1f); // ink (#0B0B0D)
		Modulate = new Color(1f, 1f, 1f, 0f); // Default transparent
		MouseFilter = MouseFilterEnum.Ignore;
	}

	/// <summary>Fade to black (modulate alpha -> 1).</summary>
	public async Task FadeIn(float duration = -1f)
	{
		if (duration < 0f) duration = DefaultFadeDuration;

		_tween?.Kill();
		_tween = CreateTween();
		_tween.TweenProperty(this, "modulate:a", 1f, duration);
		await ToSignal(_tween, Tween.SignalName.Finished);
	}

	/// <summary>Fade from black to transparent (modulate alpha -> 0).</summary>
	public async Task FadeOut(float duration = -1f)
	{
		if (duration < 0f) duration = DefaultFadeDuration;

		_tween?.Kill();
		_tween = CreateTween();
		_tween.TweenProperty(this, "modulate:a", 0f, duration);
		await ToSignal(_tween, Tween.SignalName.Finished);
	}

	/// <summary>Fade to specific alpha value.</summary>
	public async Task FadeTo(float alpha, float duration = -1f)
	{
		if (duration < 0f) duration = DefaultFadeDuration;

		_tween?.Kill();
		_tween = CreateTween();
		_tween.TweenProperty(this, "modulate:a", Mathf.Clamp(alpha, 0f, 1f), duration);
		await ToSignal(_tween, Tween.SignalName.Finished);
	}

	/// <summary>Immediately set alpha without animation.</summary>
	public void SetAlpha(float alpha)
	{
		_tween?.Kill();
		Modulate = new Color(1f, 1f, 1f, Mathf.Clamp(alpha, 0f, 1f));
	}
}
