using Godot;
using System.Collections.Generic;

/// <summary>
/// Holds global game state and registers input actions at startup.
/// </summary>
public partial class GameManager : Node
{
	public enum GameStateEnum { Menu, Playing, Paused, Ending }

	public const int InventoryMaxSlots = 4;

	public GameStateEnum CurrentState { get; private set; } = GameStateEnum.Playing;
	public bool IsPowerOn { get; set; }
	public float CurrentBattery { get; set; } = 35f; // Added for save state

	private readonly List<StringName> _inventory = new(InventoryMaxSlots);

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		SetupInputActions();

		var eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.ItemCollected += OnItemCollected;
		eventBus.PlayerCaught += OnPlayerCaught;
		eventBus.BatteryChanged += (percent) => CurrentBattery = percent;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			TogglePause();
		}
	}

	// ---- Inventory ----

	public bool AddItem(StringName itemId)
	{
		if (_inventory.Count >= InventoryMaxSlots) return false;
		_inventory.Add(itemId);
		return true;
	}

	public bool HasItem(StringName itemId) => _inventory.Contains(itemId);
	public bool RemoveItem(StringName itemId) => _inventory.Remove(itemId);
	public IReadOnlyList<StringName> GetInventory() => _inventory;

	public string[] GetInventoryArray()
	{
		var arr = new string[_inventory.Count];
		for (int i = 0; i < _inventory.Count; i++) arr[i] = _inventory[i].ToString();
		return arr;
	}

	public void LoadInventory(string[] inventory)
	{
		_inventory.Clear();
		if (inventory != null)
		{
			foreach (var item in inventory)
			{
				_inventory.Add((StringName)item);
			}
		}
	}

	private void OnItemCollected(StringName itemId)
	{
		if (!AddItem(itemId))
			GD.PushWarning($"Inventory full, cannot add {itemId}");
	}

	// ---- Player caught ----

	private void OnPlayerCaught(StringName catcher)
	{
		CurrentState = GameStateEnum.Ending;
		var eventBus = GetNode<EventBus>("/root/EventBus");

		if ((string)catcher == "osis")
			eventBus.EmitSignal(EventBus.SignalName.EndingTriggered, (StringName)"B");
		else
			eventBus.EmitSignal(EventBus.SignalName.EndingTriggered, (StringName)"C");
	}

	// ---- Pause ----

	public void TogglePause()
	{
		var eventBus = GetNode<EventBus>("/root/EventBus");

		if (CurrentState == GameStateEnum.Playing)
		{
			CurrentState = GameStateEnum.Paused;
			GetTree().Paused = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			eventBus.EmitSignal(EventBus.SignalName.GamePausedChanged, true);
		}
		else if (CurrentState == GameStateEnum.Paused)
		{
			CurrentState = GameStateEnum.Playing;
			GetTree().Paused = false;
			Input.MouseMode = Input.MouseModeEnum.Captured;
			eventBus.EmitSignal(EventBus.SignalName.GamePausedChanged, false);
		}
	}

	// Input actions defined here so they work even without project.godot entries.
	// Move to Project Settings via editor when convenient.
	private void SetupInputActions()
	{
		AddAction("move_forward", Key.W);
		AddAction("move_backward", Key.S);
		AddAction("move_left", Key.A);
		AddAction("move_right", Key.D);
		AddAction("sprint", Key.Shift);
		AddAction("crouch", Key.C);
		AddAction("toggle_flashlight", Key.F);
		AddAction("interact", Key.E);
		AddAction("pause", Key.Escape);
		AddAction("hold_breath", Key.Space);
	}

	private static void AddAction(string name, Key key)
	{
		if (InputMap.HasAction(name)) return;
		InputMap.AddAction(name);
		var ev = new InputEventKey { PhysicalKeycode = key };
		InputMap.ActionAddEvent(name, ev);
	}

	private static void AddActionAlt(string name, Key key1, Key key2)
	{
		if (InputMap.HasAction(name)) return;
		InputMap.AddAction(name);
		var ev1 = new InputEventKey { PhysicalKeycode = key1 };
		var ev2 = new InputEventKey { PhysicalKeycode = key2 };
		InputMap.ActionAddEvent(name, ev1);
		InputMap.ActionAddEvent(name, ev2);
	}
}

