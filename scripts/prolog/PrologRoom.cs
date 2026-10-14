using Godot;
using System;
using System.Threading.Tasks;

public partial class PrologRoom : Node3D
{
    private PrologInteractable _bag;
    private OmniLight3D _bagGlow;
    private PrologInteractable _drawer;
    private OmniLight3D _drawerGlow;
    private PrologInteractable _jacket;
    private OmniLight3D _jacketGlow;
    private CollisionShape3D _jacketCollision;
    private Control _chatNotification;
    private PrologChatUi _chatUi;
    private CharacterBody3D _player;
    private Camera3D _camera;

    private bool _phoneReady = false;
    private bool _isChatOpen = false;
    private bool _isChatCompleted = false;

    public override void _Ready()
    {
        _bag = GetNode<PrologInteractable>("Interactables/Bag");
        _bagGlow = GetNode<OmniLight3D>("Interactables/Bag/Glow");
        _drawer = GetNode<PrologInteractable>("Interactables/Drawer");
        _drawerGlow = GetNode<OmniLight3D>("Interactables/Drawer/Glow");
        _jacket = GetNode<PrologInteractable>("Interactables/Jacket");
        _jacketGlow = GetNode<OmniLight3D>("Interactables/Jacket/Glow");
        _jacketCollision = GetNode<CollisionShape3D>("Interactables/Jacket/Collision");

        _chatNotification = GetNode<Control>("UI/ChatNotification");
        _chatUi = GetNode<PrologChatUi>("PrologChatUi");
        _player = GetNode<CharacterBody3D>("Player");
        _camera = _player?.GetNodeOrNull<Camera3D>("Head/Camera3D");

        _bag.ObjectInteracted += OnObjectInteracted;
        _drawer.ObjectInteracted += OnObjectInteracted;
        _jacket.ObjectInteracted += OnObjectInteracted;
        _chatUi.ChatClosed += OnChatClosed;

        // Jacket is initially disabled until decision phase (P-3)
        _jacket.IsInteractable = false;
        _jacketCollision.Disabled = true;
        _jacket.Visible = false;
        _jacketGlow.Visible = false;

        _chatNotification.Visible = false;
        _chatUi.Hide();

        // Initialize battery to 35% as per NASKAH_PROLOG.md
        var gameMgr = GetNodeOrNull<GameManager>("/root/GameManager");
        if (gameMgr != null)
        {
            gameMgr.CurrentBattery = 35f;
        }
        var eventBus = GetNodeOrNull<EventBus>("/root/EventBus");
        eventBus?.EmitSignal(EventBus.SignalName.BatteryChanged, 35f);

        // Start P-1 sequence
        CallDeferred(nameof(StartSequence));
    }

    private async void StartSequence()
    {
        // Wait 1.0s before Diana's first line VO_P1_1
        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        AudioManager.Instance?.PlayVo("VO_P1_1", "Diana", "Tigapuluh lima persen... Besok pagi pasti mati total ini.", 4.0f);
    }

    private async void OnObjectInteracted(string objectId)
    {
        if (objectId == "bag")
        {
            AudioManager.Instance?.PlaySfx("zipper");
            AudioManager.Instance?.PlayVo("VO_P1_2", "Diana", "Kosong. Gak ada di sini.", 3.0f);
            if (_bagGlow != null) _bagGlow.Visible = false;
        }
        else if (objectId == "drawer")
        {
            AudioManager.Instance?.PlaySfx("drawer_open");
            AudioManager.Instance?.PlayVo("VO_P1_3", "Diana", "Cuma ada kabel bekas... ujungnya beda. Kenapa sih aku nggak pernah kepikiran beli adaptor cadangan?", 5.5f);
            if (_drawerGlow != null) _drawerGlow.Visible = false;

            // Trigger P-2 Chat notification after monologue
            await ToSignal(GetTree().CreateTimer(5.5f), SceneTreeTimer.SignalName.Timeout);
            TriggerChatNotification();
        }
        else if (objectId == "jacket")
        {
            _jacket.IsInteractable = false;
            AudioManager.Instance?.PlaySfx("cloth_rustle");
            AudioManager.Instance?.PlayVo("VO_P3_2", "Diana", "Sekolah malam-malam... Gak apa-apa, cepet kok. Masuk, ambil, pulang.", 4.0f);

            // Freeze player during fadeout
            if (_player != null)
            {
                _player.SetPhysicsProcess(false);
                _player.SetProcessInput(false);
            }

            // Transition to P-4 (PrologRoad)
            await ToSignal(GetTree().CreateTimer(3.8f), SceneTreeTimer.SignalName.Timeout);
            var sceneMgr = GetNodeOrNull<SceneManager>("/root/SceneManager");
            if (sceneMgr != null)
            {
                GD.Print("[PrologRoom] Transitioning to PrologRoad...");
                sceneMgr.GoToScene("res://scenes/prolog/PrologRoad.tscn");
            }
        }
    }

    private void TriggerChatNotification()
    {
        if (_isChatCompleted) return;

        _phoneReady = true;
        _chatNotification.Visible = true;
        AudioManager.Instance?.PlayUi("phone_notification");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (_phoneReady && !_isChatOpen && !_isChatCompleted)
        {
            if (@event.IsActionPressed("toggle_phone") || @event.IsActionPressed("interact"))
            {
                OpenPhone();
                GetViewport().SetInputAsHandled();
            }
        }
    }

    private void OpenPhone()
    {
        _phoneReady = false;
        _isChatOpen = true;
        _chatNotification.Visible = false;

        if (_player != null)
        {
            _player.SetPhysicsProcess(false);
            _player.SetProcessInput(false);
        }
        Input.MouseMode = Input.MouseModeEnum.Visible;

        _ = _chatUi.PlayChatSequence();
    }

    private async void OnChatClosed()
    {
        _isChatOpen = false;
        _isChatCompleted = true;

        if (_player != null)
        {
            _player.SetPhysicsProcess(true);
            _player.SetProcessInput(true);
        }
        Input.MouseMode = Input.MouseModeEnum.Captured;

        // Visual / Camera nod downwards and up
        if (_camera != null)
        {
            Vector3 initRot = _camera.Rotation;
            Tween tween = CreateTween();
            tween.TweenProperty(_camera, "rotation:x", initRot.X - Mathf.DegToRad(12f), 0.35f);
            tween.TweenProperty(_camera, "rotation:x", initRot.X, 0.35f);
        }

        // P-3 Monologue
        await ToSignal(GetTree().CreateTimer(0.4f), SceneTreeTimer.SignalName.Timeout);
        AudioManager.Instance?.PlayVo("VO_P3_1", "Diana", "Gak bisa. Aku gak bisa hidup tanpa HP besok. Seharian bengong di sekolah? Bisa gila.", 5.0f);

        // Enable Jacket interaction & lighting near door
        _jacket.Visible = true;
        _jacket.IsInteractable = true;
        _jacketCollision.Disabled = false;
        if (_jacketGlow != null) _jacketGlow.Visible = true;
    }
}
