using Godot;
using System;

[GlobalClass]
public partial class CheckpointMarker : Area3D
{
    [Export] public string CheckpointId { get; set; } = "";
    
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body.Name == "Player")
        {
            if (string.IsNullOrEmpty(CheckpointId)) return;
            
            // Trigger save
            var eventBus = GetNodeOrNull<Node>("/root/EventBus");
            if (eventBus != null && eventBus.HasUserSignal("CheckpointReached"))
            {
                eventBus.EmitSignal("CheckpointReached", CheckpointId);
            }
            else
            {
                // Fallback direct call if EventBus isn't set up yet
                var saveSystem = GetNodeOrNull<SaveSystem>("/root/SaveSystem");
                saveSystem?.SaveAtCheckpoint(CheckpointId);
            }

            // A checkpoint usually triggers only once
            QueueFree();
        }
    }
}
