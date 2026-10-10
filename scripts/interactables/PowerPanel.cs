using Godot;
using System.Linq;

/// <summary>
/// Power panel with multiple switches for puzzle P2.
/// Player must toggle switches in correct sequence.
/// Wrong sequence triggers loud noise.
/// </summary>
public partial class PowerPanel : InteractableBase
{
	[Export] public StringName PuzzleId { get; set; }
	[Export] public PuzzleData PuzzleConfig { get; set; }
	[Export] public Switch[] Switches { get; set; }

	private int _sequenceIndex = 0;
	private PuzzleManager _puzzleManager;

	public override void _Ready()
	{
		base._Ready();
		PromptText = "Gunakan panel listrik";

		_puzzleManager = GetNode<PuzzleManager>("/root/PuzzleManager");

		if (Switches == null || Switches.Length == 0)
		{
			var list = new System.Collections.Generic.List<Switch>();
			foreach (var child in GetChildren())
			{
				if (child is Switch sw) list.Add(sw);
			}
			Switches = list.ToArray();
		}
	}

	/// <summary>Call this when a switch is toggled.</summary>
	public void OnSwitchToggled(Switch switchedSwitch)
	{
		if (PuzzleConfig == null || PuzzleConfig.Sequence == null || PuzzleConfig.Sequence.Length == 0)
			return;

		int expectedIndex = _sequenceIndex;
		int correctSwitchIndex = PuzzleConfig.Sequence[expectedIndex];

		if (switchedSwitch.SwitchIndex == correctSwitchIndex)
		{
			_sequenceIndex++;
			GD.Print($"Correct switch! Progress: {_sequenceIndex}/{PuzzleConfig.Sequence.Length}");

			if (_sequenceIndex >= PuzzleConfig.Sequence.Length)
			{
				_puzzleManager.SolvePuzzle(PuzzleId);
				var gameManager = GetNode<GameManager>("/root/GameManager");
				gameManager.IsPowerOn = true;
				var eventBus = GetNode<EventBus>("/root/EventBus");
				eventBus.EmitSignal(EventBus.SignalName.PowerStateChanged, true);
				GD.Print("Power panel solved! Electricity is now on.");
			}
		}
		else
		{
			// Wrong switch - loud noise and reset
			_sequenceIndex = 0;
			EmitNoise(1.0f); // Maximum noise
			GD.Print("Wrong switch! Sequence reset with loud noise.");

			// Reset all switches
			if (Switches != null)
			{
				foreach (var sw in Switches)
				{
					sw?.SetState(false);
				}
			}
		}
	}
}
