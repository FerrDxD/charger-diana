using Godot;

/// <summary>
/// Exit gate for puzzle P5.
/// Requires key to unlock, then triggers Ending A when passed through.
/// </summary>
public partial class ExitGate : InteractableBase
{
	[Export] public StringName RequiredKey { get; set; }
	[Export] public bool IsOpen { get; set; }

	private MeshInstance3D _mesh;
	private CollisionShape3D _collision;
	private GameManager _gameManager;
	private PuzzleManager _puzzleManager;

	public override void _Ready()
	{
		base._Ready();
		PromptText = "Buka gerbang";

		_mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		_collision = GetNode<CollisionShape3D>("CollisionShape3D");
		_gameManager = GetNode<GameManager>("/root/GameManager");
		_puzzleManager = GetNode<PuzzleManager>("/root/PuzzleManager");

		UpdateGateState();
	}

	public override bool CanInteract(Node3D player)
	{
		if (IsOpen) return true;

		if (!string.IsNullOrEmpty(RequiredKey))
		{
			return _gameManager.HasItem(RequiredKey);
		}
		return true;
	}

	public override void Interact(Node3D player)
	{
		base.Interact(player);

		if (IsOpen)
		{
			// Player is exiting - trigger Ending A
			_puzzleManager.SolvePuzzle("p5_exit");
			var eventBus = GetNode<EventBus>("/root/EventBus");
			eventBus.EmitSignal(EventBus.SignalName.EndingTriggered, (StringName)"A");
			GD.Print("Player escaped! Ending A triggered.");
		}
		else
		{
			if (!string.IsNullOrEmpty(RequiredKey) && !_gameManager.HasItem(RequiredKey))
			{
				GD.Print($"Gate locked, need key: {RequiredKey}");
				return;
			}
			IsOpen = true;
			UpdateGateState();
		}
	}

	private void UpdateGateState()
	{
		if (_collision != null)
		{
			_collision.Disabled = IsOpen;
		}

		if (_mesh != null)
		{
			// Simple animation: move up when open
			_mesh.Position = new Vector3(0, IsOpen ? 3f : 0f, 0);
		}

		PromptText = IsOpen ? "Keluar" : "Buka gerbang";
	}
}
