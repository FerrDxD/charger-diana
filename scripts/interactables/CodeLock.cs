using Godot;

/// <summary>
/// Numeric combination lock for puzzle P3 (classroom key).
/// Player enters code; if correct, door opens and key is accessible.
/// </summary>
public partial class CodeLock : InteractableBase
{
	[Export] public StringName PuzzleId { get; set; }
	[Export] public PuzzleData PuzzleConfig { get; set; }
	[Export] public string CorrectCode { get; set; } = "1234";

	private string _currentInput = "";
	private PuzzleManager _puzzleManager;

	public override void _Ready()
	{
		base._Ready();
		PromptText = "Masukkan kode";

		_puzzleManager = GetNode<PuzzleManager>("/root/PuzzleManager");

		if (PuzzleConfig != null && !string.IsNullOrEmpty(PuzzleConfig.Code))
		{
			CorrectCode = PuzzleConfig.Code;
		}
	}

	public override void Interact(Node3D player)
	{
		base.Interact(player);

		// For simplicity, just check if player has the required items
		// Ponytail: implement proper code input UI in polish milestone
		// For now, auto-solve if player has required items
		var gameManager = GetNode<GameManager>("/root/GameManager");

		if (PuzzleConfig != null && PuzzleConfig.RequiredItems.Length > 0)
		{
			bool hasAll = true;
			foreach (var item in PuzzleConfig.RequiredItems)
			{
				if (!gameManager.HasItem(item))
				{
					hasAll = false;
					break;
				}
			}

			if (hasAll)
			{
				_puzzleManager.SolvePuzzle(PuzzleId);
				GD.Print($"Code lock {PuzzleId} unlocked!");
			}
			else
			{
				GD.Print("Missing required items for code lock");
			}
		}
		else
		{
			// No items required, just check code
			if (_currentInput == CorrectCode)
			{
				_puzzleManager.SolvePuzzle(PuzzleId);
				GD.Print($"Code lock {PuzzleId} unlocked with code {CorrectCode}!");
			}
		}
	}

	/// <summary>Call this when player enters a digit (via UI).</summary>
	public void AddDigit(char digit)
	{
		if (_currentInput.Length < 4)
		{
			_currentInput += digit;
		}
	}

	/// <summary>Clear current input.</summary>
	public void ClearInput()
	{
		_currentInput = "";
	}
}
