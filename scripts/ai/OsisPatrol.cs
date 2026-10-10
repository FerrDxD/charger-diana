using Godot;

/// <summary>
/// OSIS patrol guard. State machine: Patrol → Suspicious → Investigate → Chase → Search.
/// Uses NavigationAgent3D for pathfinding, EventBus.NoiseEmitted for hearing,
/// and raycasts for line-of-sight.
/// </summary>
public partial class OsisPatrol : CharacterBody3D
{
	public enum State { Patrol, Suspicious, Investigate, Chase, Search, Panic }

	// -- Profil Karakter --
	[Export] public OsisProfile Profile { get; set; }

	// Fallback speeds if Profile is null
	private float PatrolSpeed => Profile != null ? Profile.PatrolSpeed : 1.8f;
	private float InvestigateSpeed => Profile != null ? Profile.InvestigateSpeed : 2.4f;
	private float ChaseSpeed => Profile != null ? Profile.ChaseSpeed : 4.0f;
	private float PanicSpeed => Profile != null ? Profile.PanicSpeed : 4.2f;

	// Fallback perception
	private float SightRange => Profile != null ? Profile.SightRange : 9f;
	private float AlertRiseRate => Profile != null ? Profile.AlertRiseRate : 0.4f;
	private float SuspiciousThreshold => Profile != null ? Profile.SuspiciousThreshold : 0.3f;
	private float InvestigateThreshold => Profile != null ? Profile.InvestigateThreshold : 0.6f;

	[Export] public float SightConeAngle { get; set; } = 70f; // total degrees
	[Export] public float AlertDecayRate { get; set; } = 0.15f;
	[Export] public float CatchDistance { get; set; } = 1.2f;

	// -- Detection --
	[Export] public float HiddenDetectionRange { get; set; } = 1f;
	[Export] public float HiddenBreathDetectionRange { get; set; } = 0.5f;

	// -- Ghost Fear --
	[Export] public float GhostFearRange { get; set; } = 6f;
	[Export] public float PanicDuration { get; set; } = 5f;

	// -- Chase --
	[Export] public float ChaseLostTimeout { get; set; } = 4f;

	// -- Timers --
	[Export] public float SearchDuration { get; set; } = 8f;
	[Export] public float WaypointWaitMin { get; set; } = 2f;
	[Export] public float WaypointWaitMax { get; set; } = 4f;

	// Cached nodes
	private NavigationAgent3D _navAgent;
	private RayCast3D _sightRay;
	private Node3D[] _waypoints;
	private EventBus _eventBus;
	private Kuntilanak _ghostRef;

	// State
	public State CurrentState { get; private set; } = State.Patrol;
	private float _alert;
	private int _waypointIndex;
	private float _waitTimer;
	private float _searchTimer;
	private float _chaseLostTimer;
	private float _panicTimer;
	private Vector3 _lastKnownPos;
	private Node3D _playerRef;
	private float _gravity;

	public override void _Ready()
	{
		_navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		_sightRay = GetNode<RayCast3D>("SightRay");
		_eventBus = GetNode<EventBus>("/root/EventBus");
		_gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

		// Collect waypoints from child Marker3D nodes
		var list = new System.Collections.Generic.List<Node3D>();
		foreach (var child in GetChildren())
		{
			if (child is Marker3D marker && marker.IsInGroup("patrol_waypoint"))
				list.Add(marker);
		}
		// If no child markers, look for group-based waypoints in scene
		if (list.Count == 0)
		{
			foreach (var node in GetTree().GetNodesInGroup("patrol_waypoint"))
			{
				if (node is Node3D n) list.Add(n);
			}
		}
		_waypoints = list.ToArray();

		_eventBus.NoiseEmitted += OnNoiseEmitted;

		if (_waypoints.Length > 0)
			_navAgent.TargetPosition = _waypoints[0].GlobalPosition;
	}

	/// <summary>
	/// Dipanggil oleh ChapterController untuk memberikan rute spesifik babak ini.
	/// </summary>
	public void SetWaypoints(Node3D[] newWaypoints)
	{
		_waypoints = newWaypoints;
		_waypointIndex = 0;
		if (_waypoints != null && _waypoints.Length > 0)
		{
			_navAgent.TargetPosition = _waypoints[0].GlobalPosition;
			_waitTimer = (float)GD.RandRange(WaypointWaitMin, WaypointWaitMax);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;

		// Gravity
		if (!IsOnFloor())
			Velocity = new Vector3(Velocity.X, Velocity.Y - _gravity * dt, Velocity.Z);

		bool canSeePlayer = CheckSight();
		CheckGhostFear();

		switch (CurrentState)
		{
			case State.Patrol: ProcessPatrol(dt, canSeePlayer); break;
			case State.Suspicious: ProcessSuspicious(dt, canSeePlayer); break;
			case State.Investigate: ProcessInvestigate(dt, canSeePlayer); break;
			case State.Chase: ProcessChase(dt, canSeePlayer); break;
			case State.Search: ProcessSearch(dt, canSeePlayer); break;
			case State.Panic: ProcessPanic(dt); break;
		}

		// Alert decay when not seeing player
		if (!canSeePlayer && CurrentState != State.Panic)
			_alert = Mathf.Max(0f, _alert - AlertDecayRate * dt);

		MoveAndSlide();
	}

	private void CheckGhostFear()
	{
		if (CurrentState == State.Panic) return;

		if (_ghostRef == null)
		{
			_ghostRef = GetTree().GetFirstNodeInGroup("kuntilanak") as Kuntilanak;
		}

		if (_ghostRef != null && _ghostRef.CurrentState != Kuntilanak.State.Dormant)
		{
			if (GlobalPosition.DistanceTo(_ghostRef.GlobalPosition) < GhostFearRange)
			{
				ChangeState(State.Panic);
			}
		}
	}

	private void ProcessPanic(float dt)
	{
		_panicTimer -= dt;
		if (_panicTimer <= 0f)
		{
			ChangeState(State.Patrol);
			return;
		}

		if (_ghostRef != null)
		{
			Vector3 fleeDir = (GlobalPosition - _ghostRef.GlobalPosition).Normalized();
			fleeDir.Y = 0f;
			Velocity = new Vector3(fleeDir.X * PanicSpeed, Velocity.Y, fleeDir.Z * PanicSpeed);
			if (fleeDir.LengthSquared() > 0.01f && Mathf.Abs(fleeDir.Dot(Vector3.Up)) < 0.95f)
			{
				LookAt(GlobalPosition + fleeDir, Vector3.Up);
			}
		}
	}

	// ---- State processors ----

	private void ProcessPatrol(float dt, bool canSee)
	{
		if (_alert > SuspiciousThreshold)
		{
			ChangeState(State.Suspicious);
			return;
		}

		if (_waypoints.Length == 0) return;

		if (_navAgent.IsNavigationFinished())
		{
			_waitTimer -= dt;
			if (_waitTimer <= 0f)
			{
				_waypointIndex = (_waypointIndex + 1) % _waypoints.Length;
				_navAgent.TargetPosition = _waypoints[_waypointIndex].GlobalPosition;
				_waitTimer = (float)GD.RandRange(WaypointWaitMin, WaypointWaitMax);
			}
		}
		else
		{
			MoveToward(PatrolSpeed);
		}
	}

	private void ProcessSuspicious(float dt, bool canSee)
	{
		if (_alert >= InvestigateThreshold)
		{
			_navAgent.TargetPosition = _lastKnownPos;
			ChangeState(State.Investigate);
			return;
		}
		if (_alert < 0.1f)
		{
			ChangeState(State.Patrol);
			return;
		}
		// Stand still, look around (ponytail: no head turn anim yet)
	}

	private void ProcessInvestigate(float dt, bool canSee)
	{
		if (canSee)
		{
			ChangeState(State.Chase);
			return;
		}

		if (_navAgent.IsNavigationFinished())
		{
			// Arrived at last known, nobody here
			_searchTimer = SearchDuration;
			ChangeState(State.Search);
			return;
		}
		MoveToward(InvestigateSpeed);
	}

	private void ProcessChase(float dt, bool canSee)
	{
		if (canSee)
		{
			_chaseLostTimer = 0f;
			_lastKnownPos = _playerRef.GlobalPosition;
			_navAgent.TargetPosition = _lastKnownPos;
		}
		else
		{
			_chaseLostTimer += dt;
			if (_chaseLostTimer > ChaseLostTimeout)
			{
				_searchTimer = SearchDuration;
				ChangeState(State.Search);
				return;
			}
		}

		// Catch check
		if (_playerRef != null && GlobalPosition.DistanceTo(_playerRef.GlobalPosition) < CatchDistance)
		{
			_eventBus.EmitSignal(EventBus.SignalName.PlayerCaught, (StringName)"osis");
			ChangeState(State.Patrol); // reset after event fires
			return;
		}

		MoveToward(ChaseSpeed);
	}

	private void ProcessSearch(float dt, bool canSee)
	{
		if (canSee)
		{
			ChangeState(State.Chase);
			return;
		}

		_searchTimer -= dt;
		if (_searchTimer <= 0f)
		{
			_alert = 0f;
			ChangeState(State.Patrol);
		}
	}

	// ---- Perception ----

	private bool CheckSight()
	{
		// Find player node (cache on first find)
		if (_playerRef == null)
		{
			_playerRef = GetTree().GetFirstNodeInGroup("player") as Node3D;
			if (_playerRef == null) return false;
		}

		Vector3 toPlayer = _playerRef.GlobalPosition - GlobalPosition;
		float dist = toPlayer.Length();

		// Hidden player only detectable within 1m (per ARCHITECT.md)
		// Holding breath reduces detection chance even further
		if (_playerRef is Player player && player.IsHidden)
		{
			if (player.IsHoldingBreath && dist > HiddenBreathDetectionRange)
				return false;
			if (dist > HiddenDetectionRange)
				return false;
		}

		if (dist > SightRange) return false;

		// Cone check
		Vector3 forward = -GlobalTransform.Basis.Z;
		float dot = forward.Normalized().Dot(toPlayer.Normalized());
		float halfAngle = Mathf.DegToRad(SightConeAngle * 0.5f);
		if (dot < Mathf.Cos(halfAngle)) return false;

		// Raycast LOS
		_sightRay.TargetPosition = _sightRay.ToLocal(_playerRef.GlobalPosition);
		_sightRay.ForceRaycastUpdate();
		if (!_sightRay.IsColliding()) return false;

		var hit = _sightRay.GetCollider() as Node;
		if (hit == null) return false;

		// Walk up tree to check if we hit the player
		while (hit != null)
		{
			if (hit == _playerRef) break;
			hit = hit.GetParent();
		}
		if (hit == null) return false;

		// Seen! Raise alert
		_alert = Mathf.Min(1f, _alert + AlertRiseRate * (float)GetPhysicsProcessDeltaTime());
		_lastKnownPos = _playerRef.GlobalPosition;
		return true;
	}

	private void OnNoiseEmitted(Vector3 position, float loudness)
	{
		if (!NoiseSystem.CanHear(GlobalPosition, position, loudness)) return;

		// Noise raises alert proportional to loudness
		_alert = Mathf.Min(1f, _alert + loudness * 0.5f);
		_lastKnownPos = position;
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
			// Face movement direction
			if (Mathf.Abs(dir.Dot(Vector3.Up)) < 0.95f)
			{
				LookAt(GlobalPosition + dir, Vector3.Up);
			}
		}
	}

	private void ChangeState(State newState)
	{
		CurrentState = newState;

		string speakerName = Profile != null ? Profile.AgentName : "OSIS";
		string voInvPath = Profile != null ? Profile.VoInvestigatePath : "vo_osis_curious";
		string voInvText = Profile != null ? Profile.VoInvestigateText : "Siapa di situ? Ada yang belum pulang?";
		string voChasePath = Profile != null ? Profile.VoChasePath : "vo_osis_chase";
		string voChaseText = Profile != null ? Profile.VoChaseText : "Hei! Berhenti kamu!";
		string voPanicPath = Profile != null ? Profile.VoPanicPath : "vo_osis_panic";
		string voPanicText = Profile != null ? Profile.VoPanicText : "A-apaan itu?! LARI!!";
		float pitch = Profile != null ? Profile.VoicePitch : 1.0f;
		string busName = Profile != null ? Profile.AudioBusName : "VO";

		switch (newState)
		{
			case State.Patrol:
				if (_waypoints.Length > 0)
				{
					_navAgent.TargetPosition = _waypoints[_waypointIndex].GlobalPosition;
					_waitTimer = (float)GD.RandRange(WaypointWaitMin, WaypointWaitMax);
				}
				break;

			case State.Investigate:
				AudioManager.Instance?.PlayVo(voInvPath, speakerName, voInvText, 2.5f, pitch, busName);
				break;

			case State.Chase:
				AudioManager.Instance?.PlayVo(voChasePath, speakerName, voChaseText, 2.0f, pitch, busName);
				break;

			case State.Panic:
				_panicTimer = PanicDuration;
				AudioManager.Instance?.PlayVo(voPanicPath, speakerName, voPanicText, 2.5f, pitch, busName);
				break;
		}
	}
}
