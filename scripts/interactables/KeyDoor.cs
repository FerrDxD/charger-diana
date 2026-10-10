using Godot;

/// <summary>
/// Door that requires a specific key item to open.
/// Used for P1 (main door entry) and other locked doors.
/// </summary>
public partial class KeyDoor : InteractableBase
{
	[Export] public StringName RequiredKey { get; set; }
	[Export] public bool IsOpen { get; set; }

	private MeshInstance3D _mesh;
	private CollisionShape3D _collision;
	private GameManager _gameManager;

	public override void _Ready()
	{
		base._Ready();
		PromptText = "Buka pintu";

		_mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		_collision = GetNode<CollisionShape3D>("CollisionShape3D");
		_gameManager = GetNode<GameManager>("/root/GameManager");

		UpdateDoorState();
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
			IsOpen = false;
		}
		else
		{
			if (!string.IsNullOrEmpty(RequiredKey) && !_gameManager.HasItem(RequiredKey))
			{
				GD.Print($"Door locked, need key: {RequiredKey}");
				return;
			}
			IsOpen = true;
		}

		UpdateDoorState();
	}

	private void UpdateDoorState()
	{
		if (_collision != null)
		{
			_collision.Disabled = IsOpen;
		}

		if (_mesh != null)
		{
			// Simple rotation to simulate opening
			_mesh.Rotation = new Vector3(0, IsOpen ? Mathf.DegToRad(90f) : 0f, 0f);
		}

		PromptText = IsOpen ? "Tutup pintu" : "Buka pintu";
	}
}
