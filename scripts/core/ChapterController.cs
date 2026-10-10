using Godot;
using System;

public partial class ChapterController : Node3D
{
    [Export] public int ChapterNumber { get; set; } = 1;

    public override void _Ready()
    {
        // Setup level based on GameState
        // e.g. if we are loading from a save, place player at checkpoint
        var saveSystem = GetNodeOrNull<SaveSystem>("/root/SaveSystem");
        if (saveSystem != null && saveSystem.HasSave())
        {
            var data = saveSystem.CurrentSave;
            if (data != null && !string.IsNullOrEmpty(data.CheckpointId))
            {
                PlacePlayerAtCheckpoint(data.CheckpointId);
            }
        }
        
        GD.Print($"[ChapterController] Chapter {ChapterNumber} started.");
    }

    private void PlacePlayerAtCheckpoint(string id)
    {
        // Simple search for CheckpointMarker or a Node3D with the name
        var marker = FindChild(id, true, false) as Node3D;
        if (marker != null)
        {
            var player = GetNodeOrNull<CharacterBody3D>("Player");
            if (player != null)
            {
                player.GlobalPosition = marker.GlobalPosition;
                // Optional: set rotation
                GD.Print($"[ChapterController] Placed player at checkpoint: {id}");
            }
        }
    }
}
