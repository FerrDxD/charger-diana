using Godot;

/// <summary>
/// Global signal hub. No logic — only signal definitions.
/// All cross-system communication goes through here.
/// </summary>
public partial class EventBus : Node
{
	[Signal] public delegate void BatteryChangedEventHandler(float percent);
	[Signal] public delegate void StaminaChangedEventHandler(float percent);
	[Signal] public delegate void NoiseEmittedEventHandler(Vector3 position, float loudness);
	[Signal] public delegate void PowerStateChangedEventHandler(bool isOn);
	[Signal] public delegate void PuzzleSolvedEventHandler(StringName puzzleId);
	[Signal] public delegate void ItemCollectedEventHandler(StringName itemId);
	[Signal] public delegate void NoteCollectedEventHandler(StringName noteId);
	[Signal] public delegate void PlayerHiddenChangedEventHandler(bool isHidden);
	[Signal] public delegate void PlayerCaughtEventHandler(StringName catcher);
	[Signal] public delegate void SubtitleRequestedEventHandler(string speaker, string text, float duration);
	[Signal] public delegate void EndingTriggeredEventHandler(StringName endingId);
	[Signal] public delegate void CheckpointReachedEventHandler(StringName checkpointId);
	[Signal] public delegate void InteractPromptChangedEventHandler(string text);
	[Signal] public delegate void GamePausedChangedEventHandler(bool isPaused);
	[Signal] public delegate void JasmineScentIntensityChangedEventHandler(float intensity);
	[Signal] public delegate void RetroEffectsToggledEventHandler(bool enabled);
	[Signal] public delegate void KuntilanakTelegraphFlickerEventHandler(float intensity);
}
