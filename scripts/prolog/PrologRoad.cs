using Godot;
using System;
using System.Threading.Tasks;

public partial class PrologRoad : Node3D
{
    private Area3D _startTrigger;
    private Area3D _lampTrigger;
    private PrologInteractable _gateInteract;
    private OmniLight3D _flickerLamp;
    
    private bool _hasStartedWalking = false;
    private bool _hasLampFlickered = false;

    public override void _Ready()
    {
        _startTrigger = GetNode<Area3D>("Triggers/StartTrigger");
        _lampTrigger = GetNode<Area3D>("Triggers/LampTrigger");
        _gateInteract = GetNode<PrologInteractable>("Interactables/SchoolGate");
        _flickerLamp = GetNode<OmniLight3D>("Environment/FlickerLamp");

        _startTrigger.BodyEntered += OnStartTriggerEntered;
        _lampTrigger.BodyEntered += OnLampTriggerEntered;
        _gateInteract.ObjectInteracted += OnGateInteracted;
    }

    private void OnStartTriggerEntered(Node3D body)
    {
        if (_hasStartedWalking || body.Name != "Player") return;
        
        _hasStartedWalking = true;
        ShowSubtitle("Diana: Dingin banget... Harusnya tadi pakai jaket yang lebih tebal.", 4.0f);
        
        // Show tutorial UI
        var eventBus = GetNodeOrNull<EventBus>("/root/EventBus");
        // eventBus?.EmitSignal(...) tutorial [SHIFT] Lari
        GD.Print("[TUTORIAL] Tahan SHIFT untuk lari.");
    }

    private async void OnLampTriggerEntered(Node3D body)
    {
        if (_hasLampFlickered || body.Name != "Player") return;
        
        _hasLampFlickered = true;
        
        // Flicker effect
        for (int i = 0; i < 5; i++)
        {
            _flickerLamp.Visible = !_flickerLamp.Visible;
            await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
        }
        
        // Turn off completely
        _flickerLamp.Visible = false;
        
        // Stop cricket sounds here (via AudioManager when ready)
        // audioMgr?.StopAmbience();
        
        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        ShowSubtitle("Diana: Eh... Untung aja bawa HP buat senter.", 4.0f);
    }

    private async void OnGateInteracted(string objectId)
    {
        if (objectId == "school_gate")
        {
            // SFX clank
            GD.Print("[SFX] Suara gembok besi besar ditarik");
            ShowSubtitle("Diana: Digembok... Berarti harus cari jalan masuk lain. Pos satpam dulu deh, cari kuncinya.", 5.0f);
            
            // Wait for VO to finish
            await ToSignal(GetTree().CreateTimer(5.5f), SceneTreeTimer.SignalName.Timeout);
            
            // Transition to Chapter 1
            var sceneMgr = GetNodeOrNull<SceneManager>("/root/SceneManager");
            if (sceneMgr != null)
            {
                // Go to next scene (Chapter 1)
                GD.Print("Transitioning to Chapter 1...");
                sceneMgr.GoTo(GameFlow.Gameplay); // Assuming Gameplay flow loads Chapter 1
            }
            else 
            {
                GD.PrintErr("SceneManager not found! Cannot transition to Chapter 1.");
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            // Simple skip for now
            SkipPrologue();
            GetViewport().SetInputAsHandled();
        }
    }

    private void SkipPrologue()
    {
        GD.Print("[SKIP] Prologue dilewati.");
        var sceneMgr = GetNodeOrNull<SceneManager>("/root/SceneManager");
        if (sceneMgr != null)
        {
            sceneMgr.GoTo(GameFlow.Gameplay);
        }
    }

    private void ShowSubtitle(string text, float duration)
    {
        GD.Print($"[SUBTITLE] {text}");
    }
}
