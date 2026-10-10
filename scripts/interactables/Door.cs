using Godot;

/// <summary>
/// Door that rotates open/closed on interact. Emits noise.
/// </summary>
public partial class Door : InteractableBase
{
	[Export] public float OpenAngle { get; set; } = 90f;
	[Export] public float OpenDuration { get; set; } = 0.6f;

	private bool _isOpen;
	private Tween _tween;

	public override void Interact(Node3D player)
	{
		if (_tween?.IsRunning() == true) return;

		_isOpen = !_isOpen;
		EmitNoise(NoiseOnUse);
		PromptText = _isOpen ? "Tutup" : "Buka";
		AudioManager.Instance?.PlaySfx("door_creak", GlobalPosition);

		_tween = CreateTween();
		_tween.TweenProperty(this, "rotation:y",
			_isOpen ? Mathf.DegToRad(OpenAngle) : 0f, OpenDuration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
	}
}
