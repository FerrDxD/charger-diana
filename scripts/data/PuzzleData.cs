using Godot;
using System;

/// <summary>
/// Data resource for puzzle configuration.
/// Used for sequence puzzles (MCB switches) and code locks.
/// </summary>
[GlobalClass]
public partial class PuzzleData : Resource
{
	[Export] public StringName Id { get; set; }
	[Export] public string Description { get; set; }
	[Export] public int[] Sequence { get; set; } = Array.Empty<int>();
	[Export] public string Code { get; set; } = "";
	[Export] public StringName[] RequiredItems { get; set; } = Array.Empty<StringName>();
}
