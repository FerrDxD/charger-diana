using Godot;

/// <summary>
/// Readable note that stores text in HP gallery when read.
/// Emits NoteCollected signal on first read.
/// </summary>
public partial class NoteReadable : InteractableBase
{
	[Export] public StringName NoteId { get; set; }
	[Export] public string NoteTitle { get; set; } = "Catatan";
	[Export] public string NoteContent { get; set; } = "";

	private bool _hasRead;

	public override void _Ready()
	{
		base._Ready();
		PromptText = "Baca catatan";
	}

	public override void Interact(Node3D player)
	{
		if (_hasRead) return;

		_hasRead = true;
		EmitNoise(0.05f);
		var eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.EmitSignal(EventBus.SignalName.NoteCollected, NoteId);

		GD.Print($"Note read: {NoteTitle} - {NoteContent}");
	}
}
