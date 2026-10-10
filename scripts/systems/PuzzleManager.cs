using Godot;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages puzzle state across the game.
/// Tracks which puzzles are solved and their current progress.
/// </summary>
public partial class PuzzleManager : Node
{
	private readonly Dictionary<StringName, bool> _solvedPuzzles = new();

	public bool IsPuzzleSolved(StringName puzzleId)
	{
		return _solvedPuzzles.ContainsKey(puzzleId) && _solvedPuzzles[puzzleId];
	}

	public void SolvePuzzle(StringName puzzleId)
	{
		if (IsPuzzleSolved(puzzleId)) return;

		_solvedPuzzles[puzzleId] = true;
		var eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.EmitSignal(EventBus.SignalName.PuzzleSolved, puzzleId);

		GD.Print($"Puzzle solved: {puzzleId}");
	}

	public void ResetPuzzle(StringName puzzleId)
	{
		_solvedPuzzles[puzzleId] = false;
	}

	public void ResetAll()
	{
		_solvedPuzzles.Clear();
	}
    
    public string[] GetSolvedPuzzles()
    {
        return _solvedPuzzles.Where(kv => kv.Value).Select(kv => kv.Key.ToString()).ToArray();
    }
    
    public void LoadSolvedPuzzles(string[] puzzles)
    {
        _solvedPuzzles.Clear();
        if (puzzles != null)
        {
            foreach (var p in puzzles)
            {
                _solvedPuzzles[(StringName)p] = true;
            }
        }
    }
}
