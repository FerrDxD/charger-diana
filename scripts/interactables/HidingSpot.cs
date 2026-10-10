using Godot;

/// <summary>
/// A spot where the player can hide (locker, under table).
/// Interact to enter/exit. While hidden, player is invisible to AI
/// unless enemy is within 1m.
/// </summary>
public partial class HidingSpot : InteractableBase
{
	[Export] public Marker3D HidePosition { get; set; }
	[Export] public Marker3D CameraPosition { get; set; }

	private bool _occupied;
	private Node3D _hiddenPlayer;
	private Vector3 _playerPrevPos;
	private Vector3 _playerPrevRotation;

	public override void _Ready()
	{
		PromptText = "Sembunyi";
	}

	public override bool CanInteract(Node3D player) => !_occupied || _hiddenPlayer == player;

	public override void Interact(Node3D player)
	{
		var eventBus = GetNode<EventBus>("/root/EventBus");

		if (!_occupied)
		{
			// Enter hiding
			_occupied = true;
			_hiddenPlayer = player;
			_playerPrevPos = player.GlobalPosition;
			_playerPrevRotation = player.Rotation;

			if (HidePosition != null)
				player.GlobalPosition = HidePosition.GlobalPosition;

			if (CameraPosition != null && player is Player p)
			{
				var head = p.GetNode<Node3D>("Head");
				head.GlobalPosition = CameraPosition.GlobalPosition;
				head.GlobalRotation = CameraPosition.GlobalRotation;
			}

			PromptText = "Keluar";
			EmitNoise(NoiseOnUse);
			eventBus.EmitSignal(EventBus.SignalName.PlayerHiddenChanged, true);
		}
		else
		{
			// Exit hiding
			_occupied = false;
			player.GlobalPosition = _playerPrevPos;
			player.Rotation = _playerPrevRotation;
			_hiddenPlayer = null;
			PromptText = "Sembunyi";
			EmitNoise(NoiseOnUse);
			eventBus.EmitSignal(EventBus.SignalName.PlayerHiddenChanged, false);
		}
	}
}
