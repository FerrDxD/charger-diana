using Godot;

/// <summary>
/// Item that can be picked up. Emits ItemCollected signal and removes itself.
/// </summary>
public partial class Pickup : InteractableBase
{
	[Export] public StringName ItemId { get; set; }

	public override void _Ready()
	{
		PromptText = "Ambil";
	}

	public override void Interact(Node3D player)
	{
		base.Interact(player);
		AudioManager.Instance?.PlaySfx("item_pickup");
		var eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.EmitSignal(EventBus.SignalName.ItemCollected, ItemId);
		QueueFree();
	}
}
