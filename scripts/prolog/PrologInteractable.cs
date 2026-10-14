using Godot;
using System;

[GlobalClass]
public partial class PrologInteractable : InteractableBase
{
    [Export] public string ObjectId { get; set; } = "";
    [Export] public bool IsInteractable { get; set; } = true;
    
    [Signal]
    public delegate void ObjectInteractedEventHandler(string objectId);

    public override bool CanInteract(Node3D player) => IsInteractable;

    public override void Interact(Node3D player)
    {
        if (!IsInteractable) return;
        base.Interact(player);
        EmitSignal(SignalName.ObjectInteracted, ObjectId);
    }
}
