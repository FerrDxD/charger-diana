using Godot;
using System;

[GlobalClass]
public partial class PrologInteractable : InteractableBase
{
    [Export] public string ObjectId { get; set; } = "";
    
    [Signal]
    public delegate void ObjectInteractedEventHandler(string objectId);

    public override void Interact(Node3D player)
    {
        base.Interact(player);
        EmitSignal(SignalName.ObjectInteracted, ObjectId);
    }
}
