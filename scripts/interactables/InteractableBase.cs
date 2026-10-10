using Godot;

/// <summary>
/// Base class for all interactable objects.
/// Place as parent of a StaticBody3D so the player's interact ray
/// can detect it by walking up the node tree.
/// </summary>
public partial class InteractableBase : Node3D
{
	[Export] public string PromptText { get; set; } = "Buka";
	[Export] public float HoldTime { get; set; } = 0.0f;
	[Export] public float NoiseOnUse { get; set; } = 0.15f;

	public virtual bool CanInteract(Node3D player) => true;

	public virtual void Interact(Node3D player)
	{
		EmitNoise(NoiseOnUse);
	}

	/// <summary>Emit noise at this object's position via EventBus.</summary>
	protected void EmitNoise(float loudness)
	{
		var eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.EmitSignal(EventBus.SignalName.NoiseEmitted, GlobalPosition, loudness);
	}
}
