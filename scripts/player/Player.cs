using Godot;

/// <summary>
/// First-person player controller: movement, stamina, flashlight/battery, interact ray.
/// </summary>
public partial class Player : CharacterBody3D
{
	public bool IsHidden { get; set; }
	public bool IsHoldingBreath => _isHoldingBreath && IsHidden;
	public bool IsFlashlightOn => _batteryModel?.IsFlashlightOn ?? false;
	public Vector3 FlashlightDirection => _head != null ? -_head.GlobalTransform.Basis.Z : -GlobalTransform.Basis.Z;
	// -- Movement --
	[Export] public float WalkSpeed { get; set; } = 2.5f;
	[Export] public float SprintSpeed { get; set; } = 4.5f;
	[Export] public float CrouchSpeed { get; set; } = 1.2f;
	[Export] public float MouseSensitivity { get; set; } = 0.002f;

	// -- Stamina --
	[Export] public float StaminaMax { get; set; } = 100f;
	[Export] public float StaminaDrain { get; set; } = 25f;
	[Export] public float StaminaRegen { get; set; } = 15f;
	[Export] public float StaminaCooldown { get; set; } = 1.5f;

	// -- Hold Breath --
	[Export] public float BreathMax { get; set; } = 6f;
	[Export] public float BreathRegen { get; set; } = 3f;

	// -- Battery --
	[Export] public float BatteryStart { get; set; } = 35f;
	[Export] public float FlashlightDrain { get; set; } = 0.06f;
	[Export] public float IdleDrain { get; set; } = 0.01f;
	[Export] public float LowBatteryThreshold { get; set; } = 10f;

	// -- Noise --
	[Export] public float SprintLoudness { get; set; } = 0.8f;
	[Export] public float WalkLoudness { get; set; } = 0.25f;
	[Export] public float CrouchLoudness { get; set; } = 0.08f;
	[Export] public float SprintStepInterval { get; set; } = 0.35f;
	[Export] public float WalkStepInterval { get; set; } = 0.5f;
	[Export] public float CrouchStepInterval { get; set; } = 0.7f;
	[Export] public float FootstepPitchMin { get; set; } = 0.9f;
	[Export] public float FootstepPitchMax { get; set; } = 1.1f;

	// -- Flicker --
	[Export] public float FlickerChance { get; set; } = 0.35f;
	[Export] public float FlickerIntervalMin { get; set; } = 0.2f;
	[Export] public float FlickerIntervalMax { get; set; } = 1.2f;

	// Cached node refs
	private Node3D _head;
	private SpotLight3D _flashlight;
	private RayCast3D _interactRay;

	// State
	private Core.StaminaModel _staminaModel;
	private Core.BatteryModel _batteryModel;
	private bool _isSprinting;
	private bool _isCrouching;
	private float _gravity;
	private InteractableBase _currentInteractable;

	// Hold breath state
	private float _breath;
	private float _breathRegenTimer;
	private bool _isHoldingBreath;

	private EventBus _eventBus;

	public override void _Ready()
	{
		_head = GetNode<Node3D>("Head");
		_flashlight = GetNode<SpotLight3D>("Head/Flashlight");
		_interactRay = GetNode<RayCast3D>("Head/InteractRay");
		_eventBus = GetNode<EventBus>("/root/EventBus");

		_staminaModel = new Core.StaminaModel(StaminaMax)
		{
			MaxStamina = StaminaMax,
			DrainRate = StaminaDrain,
			RegenRate = StaminaRegen,
			CooldownTime = StaminaCooldown
		};

		var gameManager = GetNodeOrNull<GameManager>("/root/GameManager");
		float initialBattery = gameManager != null ? gameManager.CurrentBattery : BatteryStart;

		_batteryModel = new Core.BatteryModel(initialBattery)
		{
			FlashlightDrainRate = FlashlightDrain,
			IdleDrainRate = IdleDrain,
			LowThreshold = LowBatteryThreshold
		};

		_breath = BreathMax;
		_gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

		Input.MouseMode = Input.MouseModeEnum.Captured;
		_flashlight.Visible = _batteryModel.IsFlashlightOn;
		AddToGroup("player");

		_eventBus.PlayerHiddenChanged += (hidden) => IsHidden = hidden;
		_eventBus.EmitSignal(EventBus.SignalName.BatteryChanged, _batteryModel.CurrentBattery);
		_eventBus.EmitSignal(EventBus.SignalName.StaminaChanged, _staminaModel.CurrentStamina);

		var settings = GetNodeOrNull<SettingsManager>("/root/SettingsManager");
		if (settings != null)
		{
			settings.SettingChanged += OnSettingChanged;
			ApplyGraphicsPreset(settings);
		}
	}

	private void OnSettingChanged(string key)
	{
		if (key == "graphics_preset")
		{
			var settings = GetNodeOrNull<SettingsManager>("/root/SettingsManager");
			if (settings != null) ApplyGraphicsPreset(settings);
		}
	}

	private void ApplyGraphicsPreset(SettingsManager settings)
	{
		int preset = (int)settings.GetValue("graphics", "graphics_preset", 1);
		var p = preset switch
		{
			0 => GraphicsPresets.Low,
			1 => GraphicsPresets.Medium,
			_ => GraphicsPresets.High
		};
		if (_flashlight != null)
		{
			_flashlight.ShadowEnabled = p.FlashlightShadows;
		}
	}

	public override void _Input(InputEvent @event)
	{
		// Mouse look (disabled while hidden)
		if (!IsHidden && @event is InputEventMouseMotion motion)
		{
			RotateY(-motion.Relative.X * MouseSensitivity);
			_head.RotateX(-motion.Relative.Y * MouseSensitivity);
			var rot = _head.Rotation;
			rot.X = Mathf.Clamp(rot.X, Mathf.DegToRad(-89f), Mathf.DegToRad(89f));
			_head.Rotation = rot;
		}

		if (@event.IsActionPressed("toggle_flashlight"))
			ToggleFlashlight();

		if (@event.IsActionPressed("interact"))
			TryInteract();

		if (@event.IsActionPressed("hold_breath") && IsHidden)
			_isHoldingBreath = true;

		if (@event.IsActionReleased("hold_breath"))
			_isHoldingBreath = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;

		ApplyGravity(dt);

		if (!IsHidden)
		{
			ReadMovementInput();
			ApplyMovement();
		}
		else
		{
			Velocity = new Vector3(0, Velocity.Y, 0);
		}

		MoveAndSlide();
		UpdateStamina(dt);
		UpdateBattery(dt);
		UpdateBreath(dt);
		UpdateInteractRay();

		if (!IsHidden)
			EmitMovementNoise();
	}

	// ---- Movement ----

	private void ApplyGravity(float dt)
	{
		if (!IsOnFloor())
			Velocity = new Vector3(Velocity.X, Velocity.Y - _gravity * dt, Velocity.Z);
	}

	private void ReadMovementInput()
	{
		_isSprinting = Input.IsActionPressed("sprint") && !_staminaModel.IsExhausted;
		_isCrouching = Input.IsActionPressed("crouch");
		if (_isCrouching) _isSprinting = false;
	}

	private void ApplyMovement()
	{
		float speed = _isSprinting ? SprintSpeed : (_isCrouching ? CrouchSpeed : WalkSpeed);
		Vector2 input = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 dir = (Transform.Basis * new Vector3(input.X, 0f, input.Y)).Normalized();

		if (dir != Vector3.Zero)
			Velocity = new Vector3(dir.X * speed, Velocity.Y, dir.Z * speed);
		else
			Velocity = new Vector3(
				Mathf.MoveToward(Velocity.X, 0f, WalkSpeed),
				Velocity.Y,
				Mathf.MoveToward(Velocity.Z, 0f, WalkSpeed));
	}

	// ---- Stamina ----

	private void UpdateStamina(float dt)
	{
		_staminaModel.Update(dt, _isSprinting);
		if (_staminaModel.IsExhausted) _isSprinting = false;

		_eventBus.EmitSignal(EventBus.SignalName.StaminaChanged, _staminaModel.CurrentStamina);
	}

	private float _flickerTimer;
	private float _footstepTimer;

	// ---- Battery / Flashlight ----

	private void UpdateBattery(float dt)
	{
		_batteryModel.Update(dt);

		if (_batteryModel.IsDepleted && _flashlight.Visible)
		{
			_flashlight.Visible = false;
			_eventBus.EmitSignal(EventBus.SignalName.PowerStateChanged, false);
		}
		else if (_batteryModel.IsLow && _batteryModel.IsFlashlightOn)
		{
			// Low battery flicker
			_flickerTimer -= dt;
			if (_flickerTimer <= 0f)
			{
				_flickerTimer = (float)GD.RandRange(FlickerIntervalMin, FlickerIntervalMax);
				float intensity = GD.Randf() > FlickerChance ? 1f : 0f;
				_flashlight.Visible = FlickerPolicy.Scale(intensity) > 0.5f;
			}
		}
		else if (_batteryModel.IsFlashlightOn && !_flashlight.Visible && !_batteryModel.IsLow)
		{
			_flashlight.Visible = true;
		}

		_eventBus.EmitSignal(EventBus.SignalName.BatteryChanged, _batteryModel.CurrentBattery);
	}

	private void ToggleFlashlight()
	{
		if (_batteryModel.IsDepleted) return;
		_batteryModel.SetFlashlight(!_batteryModel.IsFlashlightOn);
		_flashlight.Visible = _batteryModel.IsFlashlightOn;
		_eventBus.EmitSignal(EventBus.SignalName.PowerStateChanged, _batteryModel.IsFlashlightOn);
		AudioManager.Instance?.PlaySfx("flashlight_click");
	}

	// ---- Hold Breath ----

	private void UpdateBreath(float dt)
	{
		if (!IsHidden)
		{
			_breath = Mathf.Min(BreathMax, _breath + BreathRegen * dt);
			return;
		}

		if (_isHoldingBreath)
		{
			_breath = Mathf.Max(0f, _breath - dt);
			if (_breath <= 0f)
				_isHoldingBreath = false;
		}
		else
		{
			_breathRegenTimer = Mathf.Max(0f, _breathRegenTimer - dt);
			if (_breathRegenTimer <= 0f)
				_breath = Mathf.Min(BreathMax, _breath + BreathRegen * dt);
		}
	}

	// ---- Interaction ----

	private void TryInteract()
	{
		if (_currentInteractable != null && _currentInteractable.CanInteract(this))
			_currentInteractable.Interact(this);
	}

	private void UpdateInteractRay()
	{
		InteractableBase found = null;

		if (_interactRay.IsColliding())
		{
			var hit = _interactRay.GetCollider() as Node;
			found = FindInteractable(hit);
		}

		if (found == _currentInteractable) return;

		_currentInteractable = found;
		_eventBus.EmitSignal(
			EventBus.SignalName.InteractPromptChanged,
			found?.PromptText ?? "");
	}

	/// <summary>Walk up the node tree to find an InteractableBase ancestor.</summary>
	private static InteractableBase FindInteractable(Node node)
	{
		int depth = 0;
		while (node != null && depth < 5)
		{
			if (node is InteractableBase ib) return ib;
			node = node.GetParent();
			depth++;
		}
		return null;
	}

	// ---- Noise ----

	private void EmitMovementNoise()
	{
		if (Velocity.LengthSquared() < 0.01f) return;

		float loudness;
		float stepInterval;
		if (_isSprinting)
		{
			loudness = SprintLoudness;
			stepInterval = SprintStepInterval;
		}
		else if (_isCrouching)
		{
			loudness = CrouchLoudness;
			stepInterval = CrouchStepInterval;
		}
		else
		{
			loudness = WalkLoudness;
			stepInterval = WalkStepInterval;
		}

		_footstepTimer -= (float)GetPhysicsProcessDeltaTime();
		if (_footstepTimer <= 0f)
		{
			_footstepTimer = stepInterval;
			AudioManager.Instance?.PlaySfx("footstep", GlobalPosition, pitchScale: (float)GD.RandRange(FootstepPitchMin, FootstepPitchMax));
		}

		_eventBus.EmitSignal(EventBus.SignalName.NoiseEmitted, GlobalPosition, loudness);
	}
}
