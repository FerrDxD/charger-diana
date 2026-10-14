using Godot;
using System;
using System.Threading.Tasks;

public partial class PrologRoad : Node3D
{
    private Area3D _startTrigger;
    private Area3D _lampTrigger;
    private PrologInteractable _gateInteract;
    private OmniLight3D _flickerLamp;
    private CharacterBody3D _player;
    private Pickup _gateKey;

    private bool _hasStartedWalking = false;
    private bool _hasLampFlickered = false;
    private bool _hasGateKey = false;
    private bool _keySpawned = false;

    private static readonly Vector3[] KeySpawnLocations = new Vector3[]
    {
        new Vector3(-2.8f, 0.2f, -8.0f),
        new Vector3(2.8f, 0.2f, -14.0f),
        new Vector3(-1.2f, 0.15f, -20.0f),
        new Vector3(3.0f, 0.2f, -28.0f),
        new Vector3(-2.9f, 0.2f, -34.0f),
        new Vector3(1.5f, 0.15f, -38.0f)
    };

    public override void _Ready()
    {
        _startTrigger = GetNode<Area3D>("Triggers/StartTrigger");
        _lampTrigger = GetNode<Area3D>("Triggers/LampTrigger");
        _gateInteract = GetNode<PrologInteractable>("Interactables/SchoolGate");
        _flickerLamp = GetNode<OmniLight3D>("Environment/FlickerLamp");
        _player = GetNodeOrNull<CharacterBody3D>("Player");
        _gateKey = GetNodeOrNull<Pickup>("Interactables/GateKey");

        if (_gateKey != null)
        {
            _gateKey.Visible = false;
            _gateKey.ProcessMode = ProcessModeEnum.Disabled;
        }

        _startTrigger.BodyEntered += OnStartTriggerEntered;
        _lampTrigger.BodyEntered += OnLampTriggerEntered;
        _gateInteract.ObjectInteracted += OnGateInteracted;

        // Preserve Diana's battery state from room (never reset or increase)
        var gameMgr = GetNodeOrNull<GameManager>("/root/GameManager");
        float currentBat = gameMgr?.CurrentBattery ?? 35f;
        if (currentBat > 35f) currentBat = 35f;
        if (gameMgr != null) gameMgr.CurrentBattery = currentBat;
        var eventBus = GetNodeOrNull<EventBus>("/root/EventBus");
        eventBus?.EmitSignal(EventBus.SignalName.BatteryChanged, currentBat);

        if (eventBus != null)
        {
            eventBus.ItemCollected += OnItemCollected;
        }

        // Play outdoor night ambience
        AudioManager.Instance?.PlayAmbience("ambience_outdoor_night");

        // Diana monologue when in the lorong (starts automatically upon arriving)
        GetTree().CreateTimer(0.8f).Timeout += () =>
        {
            if (!_hasStartedWalking)
            {
                _hasStartedWalking = true;
                AudioManager.Instance?.PlayVo("VO_P4_1", "Diana", "Dingin banget... Harusnya tadi pakai jaket yang lebih tebal.", 4.0f);
            }
        };
    }

    private void OnItemCollected(StringName itemId)
    {
        if (itemId == "kunci_gerbang_prolog")
        {
            _hasGateKey = true;
            if (_gateInteract != null)
            {
                _gateInteract.PromptText = "Buka Gerbang [Kunci]";
            }
            AudioManager.Instance?.PlayVo("VO_P5_KEY", "Diana", "Nah, ini kuncinya! Sekarang bisa buka gerbangnya.", 3.5f);
        }
    }

    private void OnStartTriggerEntered(Node3D body)
    {
        if (_hasStartedWalking || (!body.IsInGroup("player") && body.Name != "Player")) return;

        _hasStartedWalking = true;
        AudioManager.Instance?.PlayVo("VO_P4_1", "Diana", "Dingin banget... Harusnya tadi pakai jaket yang lebih tebal.", 4.0f);
    }

    private async void OnLampTriggerEntered(Node3D body)
    {
        if (_hasLampFlickered || (!body.IsInGroup("player") && body.Name != "Player")) return;

        _hasLampFlickered = true;

        // Flicker effect: lamp stutters before dying
        for (int i = 0; i < 5; i++)
        {
            _flickerLamp.Visible = !_flickerLamp.Visible;
            await ToSignal(GetTree().CreateTimer(0.12f), SceneTreeTimer.SignalName.Timeout);
        }

        // Lamp goes completely out
        _flickerLamp.Visible = false;

        await ToSignal(GetTree().CreateTimer(0.8f), SceneTreeTimer.SignalName.Timeout);
        AudioManager.Instance?.PlayVo("VO_P4_2", "Diana", "Eh... Untung aja bawa HP buat senter.", 4.0f);
    }

    private async void OnGateInteracted(string objectId)
    {
        if (objectId != "school_gate") return;

        if (!_hasGateKey)
        {
            // Gate is locked! Heavy padlock and iron chain clatter SFX
            AudioManager.Instance?.PlaySfx("gate_locked");

            if (!_keySpawned)
            {
                _keySpawned = true;
                SpawnKeyRandomly();
                AudioManager.Instance?.PlayVo(
                    "VO_P5_1",
                    "Diana",
                    "Digembok... Berarti harus cari kuncinya dulu. Coba cari di sekitar lorong ini.",
                    5.0f
                );
            }
            else
            {
                AudioManager.Instance?.PlayVo(
                    "VO_P5_REMIND",
                    "Diana",
                    "Masih terkunci. Kuncinya pasti jatuh di sekitar lorong.",
                    3.5f
                );
            }
            return;
        }

        // Gate unlocked!
        _gateInteract.IsInteractable = false;
        _gateInteract.PromptText = "";

        // Unlocking SFX and Diana reaction
        AudioManager.Instance?.PlaySfx("gate_unlock");
        AudioManager.Instance?.PlayVo("VO_P5_OPEN", "Diana", "Kebuka! Masuk sekarang...", 3.0f);

        // Remove key from inventory
        var gameMgr = GetNodeOrNull<GameManager>("/root/GameManager");
        gameMgr?.RemoveItem("kunci_gerbang_prolog");

        // Freeze player during transition
        if (_player != null)
        {
            _player.SetPhysicsProcess(false);
            _player.SetProcessInput(false);
        }

        // Wait for VO line to finish
        await ToSignal(GetTree().CreateTimer(3.2f), SceneTreeTimer.SignalName.Timeout);

        // Transition to Chapter 1 (Gameplay)
        var sceneMgr = GetNodeOrNull<SceneManager>("/root/SceneManager");
        if (sceneMgr != null)
        {
            GD.Print("[PrologRoad] Transitioning to Chapter 1...");
            sceneMgr.GoTo(GameFlow.Gameplay);
        }
        else
        {
            GD.PrintErr("SceneManager not found! Transitioning via fallback...");
            GetTree().ChangeSceneToFile("res://scenes/levels/Chapter1.tscn");
        }
    }

    private void SpawnKeyRandomly()
    {
        int idx = GD.RandRange(0, KeySpawnLocations.Length - 1);
        Vector3 spawnPos = KeySpawnLocations[idx];

        if (_gateKey != null)
        {
            _gateKey.GlobalPosition = spawnPos;
            _gateKey.Visible = true;
            _gateKey.ProcessMode = ProcessModeEnum.Inherit;
        }
        else
        {
            var pickupScene = GD.Load<PackedScene>("res://scenes/interactables/Pickup.tscn");
            if (pickupScene != null)
            {
                var pickup = pickupScene.Instantiate<Pickup>();
                pickup.Name = "GateKey";
                pickup.ItemId = "kunci_gerbang_prolog";
                pickup.PromptText = "Ambil Kunci Gerbang";
                pickup.GlobalPosition = spawnPos;
                GetNode("Interactables").AddChild(pickup);
                _gateKey = pickup;
            }
        }
        GD.Print($"[PrologRoad] Kunci gerbang spawned di {spawnPos}");
    }
}
