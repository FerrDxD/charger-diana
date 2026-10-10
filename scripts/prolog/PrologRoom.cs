using Godot;
using System;
using System.Threading.Tasks;

public partial class PrologRoom : Node3D
{
    private PrologInteractable _bag;
    private PrologInteractable _drawer;
    private PrologInteractable _jacket;
    private PrologChatUi _chatUi;
    
    public override void _Ready()
    {
        _bag = GetNode<PrologInteractable>("Interactables/Bag");
        _drawer = GetNode<PrologInteractable>("Interactables/Drawer");
        _jacket = GetNode<PrologInteractable>("Interactables/Jacket");
        _chatUi = GetNode<PrologChatUi>("PrologChatUi");

        _bag.ObjectInteracted += OnObjectInteracted;
        _drawer.ObjectInteracted += OnObjectInteracted;
        _jacket.ObjectInteracted += OnObjectInteracted;
        _chatUi.ChatClosed += OnChatClosed;

        // Jacket is initially non-interactable
        _jacket.SetProcess(false);
        _jacket.Visible = false; // or disable collision

        _chatUi.Hide();

        // Start P-1
        CallDeferred(nameof(StartSequence));
    }

    private async void StartSequence()
    {
        // Simulate VO P1_1
        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        ShowSubtitle("Diana: Tigapuluh lima persen... Besok pagi pasti mati total ini.", 4.0f);
    }

    private async void OnObjectInteracted(string objectId)
    {
        if (objectId == "bag")
        {
            ShowSubtitle("Diana: Kosong. Gak ada di sini.", 3.0f);
        }
        else if (objectId == "drawer")
        {
            ShowSubtitle("Diana: Cuma ada kabel bekas... ujungnya beda. Kenapa sih aku nggak pernah kepikiran beli adaptor cadangan?", 5.0f);
            
            // Trigger P-2 Chat
            await ToSignal(GetTree().CreateTimer(5.5f), SceneTreeTimer.SignalName.Timeout);
            _ = _chatUi.PlayChatSequence();
        }
        else if (objectId == "jacket")
        {
            ShowSubtitle("Diana: Sekolah malam-malam... Gak apa-apa, cepet kok. Masuk, ambil, pulang.", 4.0f);
            
            // Fade out and move to P-4 (PrologRoad)
            await ToSignal(GetTree().CreateTimer(4.5f), SceneTreeTimer.SignalName.Timeout);
            var sceneMgr = GetNodeOrNull<SceneManager>("/root/SceneManager");
            if (sceneMgr != null)
            {
                // Go to next scene (PrologRoad)
                GD.Print("Transitioning to PrologRoad...");
                sceneMgr.GoToScene("res://scenes/prolog/PrologRoad.tscn");
            }
        }
    }

    private void OnChatClosed()
    {
        // P-3
        ShowSubtitle("Diana: Gak bisa. Aku gak bisa hidup tanpa HP besok. Seharian bengong di sekolah? Bisa gila.", 5.0f);
        
        // Enable jacket
        _jacket.SetProcess(true);
        _jacket.Visible = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && !_chatUi.Visible)
        {
            // Simple skip
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
            // Go to Gameplay (Chapter 1)
            sceneMgr.GoTo(GameFlow.Gameplay);
        }
    }

    private void ShowSubtitle(string text, float duration)
    {
        // Mocking subtitle system since AudioManager/SubtitleBox might not be fully wired here.
        GD.Print($"[SUBTITLE] {text}");
        
        // Try finding a subtitle box in HUD if player has one, or eventbus
        var eventBus = GetNodeOrNull<EventBus>("/root/EventBus");
        // eventBus?.EmitSignal(...)
    }
}
