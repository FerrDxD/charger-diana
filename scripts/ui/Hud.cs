using Godot;

/// <summary>
/// HUD following DESIGN.md and PRD V2 specifications.
/// Uses reusable components: BatteryIndicator, StaminaBar, InteractPrompt, SubtitleBox.
/// Updates only via EventBus signals — no per-frame polling.
/// </summary>
public partial class Hud : Control
{
	private BatteryIndicator _batteryIndicator;
	private StaminaBar _staminaBar;
	private InteractPrompt _interactPrompt;
	private SubtitleBox _subtitleBox;
	private ColorRect _scentOverlay;
	private ColorRect _retroOverlay;
	private EventBus _eventBus;

	public override void _Ready()
	{
		_batteryIndicator = GetNode<BatteryIndicator>("BottomLeft/BatteryIndicator");
		_staminaBar = GetNode<StaminaBar>("BottomLeft/StaminaBar");
		_interactPrompt = GetNode<InteractPrompt>("PromptContainer/InteractPrompt");
		_subtitleBox = GetNodeOrNull<SubtitleBox>("SubtitleLayer/SubtitleBox") ?? GetNodeOrNull<SubtitleBox>("BottomCenter/SubtitleBox");
		_eventBus = GetNode<EventBus>("/root/EventBus");

		SetupVisualOverlays();

		_eventBus.BatteryChanged += OnBatteryChanged;
		_eventBus.StaminaChanged += OnStaminaChanged;
		_eventBus.InteractPromptChanged += OnPromptChanged;
		_eventBus.SubtitleRequested += OnSubtitleRequested;
		_eventBus.JasmineScentIntensityChanged += OnJasmineScentIntensityChanged;
		_eventBus.RetroEffectsToggled += OnRetroEffectsToggled;

		var gameManager = GetNodeOrNull<GameManager>("/root/GameManager");
		if (gameManager != null)
		{
			OnBatteryChanged(gameManager.CurrentBattery);
		}
	}

	private void SetupVisualOverlays()
	{
		// 1. Jasmine Scent Overlay (soft white/green hue vignetting)
		_scentOverlay = new ColorRect
		{
			Name = "ScentOverlay",
			MouseFilter = MouseFilterEnum.Ignore,
			Color = new Color(0.9f, 0.95f, 0.85f, 0f),
			LayoutMode = 1,
			AnchorsPreset = (int)LayoutPreset.FullRect
		};
		AddChild(_scentOverlay);
		MoveChild(_scentOverlay, 0);

		// 2. PS1 Dither & Scanline Retro Overlay
		_retroOverlay = new ColorRect
		{
			Name = "RetroOverlay",
			MouseFilter = MouseFilterEnum.Ignore,
			LayoutMode = 1,
			AnchorsPreset = (int)LayoutPreset.FullRect
		};

		var shader = GD.Load<Shader>("res://resources/shaders/ps1_post_process.gdshader");
		if (shader != null)
		{
			var mat = new ShaderMaterial { Shader = shader };
			_retroOverlay.Material = mat;
		}

		AddChild(_retroOverlay);
		MoveChild(_retroOverlay, 0);
	}

	private void OnBatteryChanged(float percent)
	{
		_batteryIndicator?.SetBattery(percent);
	}

	private void OnStaminaChanged(float percent)
	{
		_staminaBar?.SetStamina(percent);
	}

	private void OnPromptChanged(string text)
	{
		_interactPrompt?.SetPrompt(text);
	}

	private void OnSubtitleRequested(string speaker, string text, float duration)
	{
		_subtitleBox?.ShowSubtitle(speaker, text, duration);
	}

	private void OnJasmineScentIntensityChanged(float intensity)
	{
		if (_scentOverlay != null)
		{
			float alpha = Mathf.Clamp(intensity * 0.45f, 0f, 0.45f);
			_scentOverlay.Color = new Color(0.9f, 0.95f, 0.85f, alpha);
		}
	}

	private void OnRetroEffectsToggled(bool enabled)
	{
		if (_retroOverlay != null)
		{
			_retroOverlay.Visible = enabled;
		}
	}
}
