using Godot;
using System.Collections.Generic;

/// <summary>
/// Kuntilanak ghost enemy. State machine: Dormant -> Roaming -> Telegraph -> Appear -> Hunting -> Retreat.
/// Mechanics: inverted laugh (loud=far, quiet=near), jasmine scent visual effect,
/// flashlight vulnerability, reacts to noise and light.
/// </summary>
public partial class Kuntilanak : CharacterBody3D
{
	public enum State { Dormant, Roaming, Telegraph, Appear, Hunting, Retreat }

	// -- Movement speeds --
	[Export] public float RoamSpeed { get; set; } = 1.2f;
	[Export] public float HuntSpeed { get; set; } = 3.6f;

	// -- Perception --
	[Export] public float NoiseThreshold { get; set; } = 0.6f;
	[Export] public float SightRange { get; set; } = 15f;
	[Export] public float FlashlightStunDuration { get; set; } = 2.5f;

	// -- Laugh (inverted) --
	[Export] public float LaughMaxDistance { get; set; } = 20f;
	[Export] public float LaughIntervalMin { get; set; } = 8f;
	[Export] public float LaughIntervalMax { get; set; } = 15f;

	// -- Scent & Telegraph --
	[Export] public float ScentRadius { get; set; } = 8f;
	[Export] public float TelegraphDuration { get; set; } = 5f;
	[Export] public float AppearDuration { get; set; } = 1.5f;

	// -- Timers --
	[Export] public float HuntLostTimeout { get; set; } = 10f;
	[Export] public float RetreatTimeout { get; set; } = 6f;

	// -- Catch --
	[Export] public float CatchDistance { get; set; } = 1.0f;

	// -- Roaming --
	[Export] public float RoamRange { get; set; } = 10f;

	// -- Flashlight --
	[Export] public float FlashlightStunRange { get; set; } = 7f;
	[Export] public float FlashlightConeDot { get; set; } = 0.7f;

	// -- Hunt Speed Boost --
	[Export] public float HuntSpeedBoostPowerOff { get; set; } = 1.15f;

	// Cached nodes
	private NavigationAgent3D _navAgent;
	private EventBus _eventBus;
	private GameManager _gameManager;
	private Node3D _playerRef;
	private float _gravity;

	// State
	public State CurrentState { get; private set; } = State.Dormant;
	private float _laughTimer;
	private float _huntLostTimer;
	private float _retreatTimer;
	private float _telegraphTimer;
	private float _appearTimer;
	private Vector3 _lastKnownPos;
	private float _stunTimer;
	
	// Encounter logic
	private int _encounterIntensity = 0; // 1=Telegraph only, 2=Telegraph->Appear->Retreat, 3=Telegraph->Appear->Hunt

	// Variasi suara tawa (5-8 klip sesuai PRD)
	private string[] _laughSounds = { "ghost_laugh_1", "ghost_laugh_2", "ghost_laugh_3", "ghost_laugh_4", "ghost_laugh_5" };
	private int _lastLaughIdx = -1;

	public override void _Ready()
	{
		_navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		_eventBus = GetNode<EventBus>("/root/EventBus");
		_gameManager = GetNodeOrNull<GameManager>("/root/GameManager"); // Make optional to prevent crash if not found
		_gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

		_eventBus.NoiseEmitted += OnNoiseEmitted;
		_eventBus.PowerStateChanged += OnPowerStateChanged;

		_laughTimer = (float)GD.RandRange(LaughIntervalMin, LaughIntervalMax);
	}

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;

		if (!IsOnFloor())
			Velocity = new Vector3(Velocity.X, Velocity.Y - _gravity * dt, Velocity.Z);

		if (_stunTimer > 0f)
		{
			_stunTimer -= dt;
			MoveAndSlide();
			return;
		}

		switch (CurrentState)
		{
			case State.Dormant: ProcessDormant(dt); break;
			case State.Roaming: ProcessRoaming(dt); break;
			case State.Telegraph: ProcessTelegraph(dt); break;
			case State.Appear: ProcessAppear(dt); break;
			case State.Hunting: ProcessHunting(dt); break;
			case State.Retreat: ProcessRetreat(dt); break;
		}

		UpdateLaugh(dt);
		UpdateScentEffect();
		MoveAndSlide();
	}

	// ---- Encounter System ----

	/// <summary>
	/// Dipanggil oleh script Chapter untuk mengatur intensity per babak.
	/// intensity 1 = Babak 1 (Telegraph terdengar)
	/// intensity 2 = Babak 2 (Penampakan)
	/// intensity 3 = Babak 3 (Hunting)
	/// </summary>
	public void TriggerEncounter(int intensity, Vector3 spawnPosition)
	{
		GlobalPosition = spawnPosition;
		_encounterIntensity = intensity;
		ChangeState(State.Telegraph);
	}

	// ---- State processors ----

	private void ProcessDormant(float dt)
	{
		// Noise/Power triggers are handled in OnNoiseEmitted/OnPowerStateChanged
	}

	private void ProcessRoaming(float dt)
	{
		// If player is heard/seen, transition to Telegraph before hunting
		if (_lastKnownPos != Vector3.Zero)
		{
			_encounterIntensity = 3; // Natural hunt
			ChangeState(State.Telegraph);
			return;
		}

		// Roam randomly or towards player slowly
		if (_navAgent.IsNavigationFinished())
		{
			var randomOffset = new Vector3(
				(float)GD.RandRange(-RoamRange, RoamRange),
				0,
				(float)GD.RandRange(-RoamRange, RoamRange)
			);
			_navAgent.TargetPosition = GlobalPosition + randomOffset;
		}
		MoveToward(RoamSpeed);
	}

	private void ProcessTelegraph(float dt)
	{
		_telegraphTimer -= dt;
		
		// Update flicker based on how close to appearing
		float flickerIntensity = 1f - Mathf.Clamp(_telegraphTimer / TelegraphDuration, 0f, 1f);
		flickerIntensity = FlickerPolicy.Scale(flickerIntensity);
		_eventBus.EmitSignal(EventBus.SignalName.KuntilanakTelegraphFlicker, flickerIntensity);

		if (_telegraphTimer <= 0f)
		{
			_eventBus.EmitSignal(EventBus.SignalName.KuntilanakTelegraphFlicker, 0f); // Reset flicker
			
			if (_encounterIntensity == 1)
			{
				ChangeState(State.Dormant);
			}
			else
			{
				ChangeState(State.Appear);
			}
		}
	}

	private void ProcessAppear(float dt)
	{
		// Stand still, become visible (handled by visual script or animation)
		_appearTimer -= dt;
		if (_appearTimer <= 0f)
		{
			if (_encounterIntensity == 2)
			{
				ChangeState(State.Retreat);
			}
			else
			{
				ChangeState(State.Hunting);
			}
		}
	}

	private void ProcessHunting(float dt)
	{
		if (_playerRef == null)
		{
			_playerRef = GetTree().GetFirstNodeInGroup("player") as Node3D;
			if (_playerRef == null) return;
		}

		Vector3 toPlayer = _playerRef.GlobalPosition - GlobalPosition;
		float dist = toPlayer.Length();

		// Flashlight stun check
		if (CheckFlashlightExposure())
		{
			_stunTimer = FlashlightStunDuration;
			ChangeState(State.Retreat);
			return;
		}

		// Catch check
		if (dist < CatchDistance)
		{
			_eventBus.EmitSignal(EventBus.SignalName.PlayerCaught, (StringName)"kuntilanak");
			ChangeState(State.Dormant);
			return;
		}

		// Update target position
		_lastKnownPos = _playerRef.GlobalPosition;
		_navAgent.TargetPosition = _lastKnownPos;

		// Lost player check
		_huntLostTimer += dt;
		if (_huntLostTimer > HuntLostTimeout)
		{
			ChangeState(State.Roaming);
			return;
		}

		float speed = HuntSpeed;
		if (_gameManager != null && !_gameManager.IsPowerOn)
		{
			speed *= HuntSpeedBoostPowerOff;
		}
		MoveToward(speed);
	}

	private void ProcessRetreat(float dt)
	{
		_retreatTimer += dt;
		if (_retreatTimer >= RetreatTimeout)
		{
			ChangeState(State.Roaming);
			return;
		}

		if (_lastKnownPos != Vector3.Zero)
		{
			Vector3 awayDir = (GlobalPosition - _lastKnownPos).Normalized();
			awayDir.Y = 0;
			Velocity = new Vector3(awayDir.X * RoamSpeed, Velocity.Y, awayDir.Z * RoamSpeed);
		}
	}

	// ---- Perception ----

	private void OnNoiseEmitted(Vector3 position, float loudness)
	{
		if (loudness < NoiseThreshold) return;

		if (CurrentState == State.Dormant)
		{
			_lastKnownPos = position;
			ChangeState(State.Roaming);
		}
		else if (CurrentState == State.Roaming)
		{
			_lastKnownPos = position;
		}
		else if (CurrentState == State.Hunting)
		{
			_lastKnownPos = position;
			_huntLostTimer = 0f;
		}
	}

	private void OnPowerStateChanged(bool isOn)
	{
		if (isOn && CurrentState == State.Dormant)
		{
			ChangeState(State.Roaming);
		}
	}

	private bool CheckFlashlightExposure()
	{
		if (_playerRef == null) return false;

		// We assume player script has an IsFlashlightOn property
		var isFlashlightOnProp = _playerRef.Get("IsFlashlightOn");
		if (isFlashlightOnProp.VariantType == Variant.Type.Nil || !(bool)isFlashlightOnProp) return false;

		Vector3 toGhost = GlobalPosition - _playerRef.GlobalPosition;
		float dist = toGhost.Length();
		if (dist > FlashlightStunRange) return false;

		// Assuming player script has FlashlightDirection
		var flashDirProp = _playerRef.Get("FlashlightDirection");
		if (flashDirProp.VariantType == Variant.Type.Nil) return false;
		Vector3 flashDir = (Vector3)flashDirProp;

		float dot = flashDir.Normalized().Dot(toGhost.Normalized());
		return dot > FlashlightConeDot;
	}

	private float _lastScentIntensity = -1f;

	// ---- Effects ----

	private void UpdateLaugh(float dt)
	{
		if (CurrentState == State.Dormant) return;

		_laughTimer -= dt;
		if (_laughTimer <= 0f)
		{
			_laughTimer = (float)GD.RandRange(LaughIntervalMin, LaughIntervalMax);
			PlayLaugh();
		}
	}

	private void PlayLaugh()
	{
		if (_playerRef == null)
		{
			_playerRef = GetTree().GetFirstNodeInGroup("player") as Node3D;
			if (_playerRef == null) return;
		}

		float dist = GlobalPosition.DistanceTo(_playerRef.GlobalPosition);
		float t = Mathf.Clamp(dist / LaughMaxDistance, 0f, 1f);

		// Inverted volume: quiet near, loud far (according to myth)
		float volumeLinear = Mathf.Clamp(t, 0.1f, 1.0f);
		float volumeDb = Mathf.LinearToDb(volumeLinear);

		// Random selection 5-8 clips without consecutive repetition
		int idx = GD.RandRange(0, _laughSounds.Length - 1);
		if (idx == _lastLaughIdx && _laughSounds.Length > 1)
		{
			idx = (idx + 1) % _laughSounds.Length;
		}
		_lastLaughIdx = idx;
		string clipName = _laughSounds[idx];

		// Play via AudioManager if it exists
		var audioMgr = GetNodeOrNull("/root/AudioManager");
		if (audioMgr != null)
		{
			audioMgr.Call("PlayGhostSound", clipName, GlobalPosition, volumeDb);
		}
	}

	private void UpdateScentEffect()
	{
		if (_playerRef == null) return;

		float dist = GlobalPosition.DistanceTo(_playerRef.GlobalPosition);
		float intensity = 0f;
		
		// Always strong scent during telegraph if player is close enough
		if (CurrentState == State.Telegraph)
		{
			intensity = 1.0f;
		}
		else if (dist < ScentRadius)
		{
			intensity = 1f - (dist / ScentRadius);
		}

		if (Mathf.Abs(intensity - _lastScentIntensity) > 0.03f)
		{
			_lastScentIntensity = intensity;
			_eventBus.EmitSignal(EventBus.SignalName.JasmineScentIntensityChanged, intensity);
		}
	}

	// ---- Helpers ----

	private void MoveToward(float speed)
	{
		Vector3 next = _navAgent.GetNextPathPosition();
		Vector3 dir = (next - GlobalPosition).Normalized();
		dir.Y = 0f;

		if (dir.LengthSquared() > 0.001f)
		{
			Velocity = new Vector3(dir.X * speed, Velocity.Y, dir.Z * speed);
			if (Mathf.Abs(dir.Dot(Vector3.Up)) < 0.95f)
			{
				LookAt(GlobalPosition + dir, Vector3.Up);
			}
		}
	}

	private void ChangeState(State newState)
	{
		CurrentState = newState;
		_telegraphTimer = TelegraphDuration;
		_appearTimer = AppearDuration;
		_huntLostTimer = 0f;
		_retreatTimer = 0f;
		_lastKnownPos = Vector3.Zero;

		GD.Print($"Kuntilanak state: {newState}");
	}
}
