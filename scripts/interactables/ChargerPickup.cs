using Godot;

/// <summary>
/// Special pickup for the charger (P4).
/// Triggers kuntilanak event when collected.
/// </summary>
public partial class ChargerPickup : Pickup
{
	public override void Interact(Node3D player)
	{
		base.Interact(player);

		// Trigger kuntilanak event
		var eventBus = GetNode<EventBus>("/root/EventBus");
		var puzzleManager = GetNode<PuzzleManager>("/root/PuzzleManager");

		puzzleManager.SolvePuzzle("p4_charger");

		// Find kuntilanak and trigger event
		var kuntilanak = GetTree().GetFirstNodeInGroup("kuntilanak");
		if (kuntilanak != null)
		{
			GD.Print("Charger collected! Kuntilanak event triggered.");
			// Ponytail: implement proper event triggering in polish milestone
		}
	}
}
