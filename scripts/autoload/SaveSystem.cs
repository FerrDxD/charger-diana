using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;

public sealed record SaveData
{
    public int Version { get; init; } = 1;
    public string SavedAtUtc { get; init; } = DateTime.UtcNow.ToString("O");
    public int Chapter { get; init; } = 1;
    public string CheckpointId { get; init; } = "";
    public float BatteryPercent { get; init; } = 100f;
    public string[] Inventory { get; init; } = Array.Empty<string>();
    public string[] SolvedPuzzles { get; init; } = Array.Empty<string>();
    public string[] CollectedNotes { get; init; } = Array.Empty<string>();
    public string[] Flags { get; init; } = Array.Empty<string>();
    public int Deaths { get; init; } = 0;
    public double PlaySeconds { get; init; } = 0.0;
}

public partial class SaveSystem : Node
{
    private const string SavePath = "user://save.json";
    private const string SavePathTmp = "user://save.json.tmp";
    private const string SavePathBak = "user://save.json.bak";
    
    private const int CurrentSaveVersion = 1;

    public SaveData CurrentSave { get; private set; }

    [Signal]
    public delegate void SaveCompletedEventHandler();

    public override void _Ready()
    {
        // Subscribe to GameState or Checkpoint reached events if needed
        var eventBus = GetNodeOrNull<EventBus>("/root/EventBus");
        if (eventBus != null)
        {
            if (eventBus.HasUserSignal("CheckpointReached"))
                eventBus.Connect("CheckpointReached", new Callable(this, nameof(SaveAtCheckpoint)));
        }
    }

    public bool HasSave()
    {
        return FileAccess.FileExists(SavePath);
    }

    public void ClearSave()
    {
        if (HasSave())
        {
            DirAccess.RemoveAbsolute(SavePath);
            CurrentSave = null;
        }
    }

    public bool LoadGame()
    {
        if (!HasSave()) return false;

        try
        {
            using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
            if (file == null) return false;

            string json = file.GetAsText();
            var data = JsonSerializer.Deserialize<SaveData>(json);

            if (data == null || string.IsNullOrEmpty(data.CheckpointId))
            {
                GD.PrintErr("Save data is corrupted or invalid.");
                return false;
            }

            if (data.Version > CurrentSaveVersion)
            {
                GD.PrintErr("Save version is from a newer build.");
                return false;
            }

            // In the future, run migration if data.Version < CurrentSaveVersion
            // e.g., data = MigrateData(data);

            CurrentSave = data;
            
            // Push loaded data to managers
            var gameManager = GetNodeOrNull<GameManager>("/root/GameManager");
            if (gameManager != null)
            {
                gameManager.LoadInventory(data.Inventory);
                gameManager.CurrentBattery = Math.Max(data.BatteryPercent, 5f); // Prevent soft-lock (minimum 5%)
            }
            
            var puzzleManager = GetNodeOrNull<PuzzleManager>("/root/PuzzleManager");
            if (puzzleManager != null)
            {
                puzzleManager.LoadSolvedPuzzles(data.SolvedPuzzles);
            }
            
            return true;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to load save: {ex.Message}");
            return false;
        }
    }

    public void SaveAtCheckpoint(string checkpointId)
    {
        var gameManager = GetNodeOrNull<GameManager>("/root/GameManager");
        var puzzleManager = GetNodeOrNull<PuzzleManager>("/root/PuzzleManager");
        
        var newData = new SaveData
        {
            Version = CurrentSaveVersion,
            SavedAtUtc = DateTime.UtcNow.ToString("O"),
            Chapter = CurrentSave?.Chapter ?? 1,
            CheckpointId = checkpointId,
            BatteryPercent = gameManager?.CurrentBattery ?? 35f,
            Inventory = gameManager?.GetInventoryArray() ?? Array.Empty<string>(),
            SolvedPuzzles = puzzleManager?.GetSolvedPuzzles() ?? Array.Empty<string>(),
            CollectedNotes = CurrentSave?.CollectedNotes ?? Array.Empty<string>(),
            Flags = CurrentSave?.Flags ?? Array.Empty<string>(),
            Deaths = CurrentSave?.Deaths ?? 0,
            PlaySeconds = CurrentSave?.PlaySeconds ?? 0.0
        };

        WriteSave(newData);
    }

    private void WriteSave(SaveData data)
    {
        try
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

            // Atomic write
            using (var file = FileAccess.Open(SavePathTmp, FileAccess.ModeFlags.Write))
            {
                file.StoreString(json);
            }

            // Backup old save if exists
            if (FileAccess.FileExists(SavePath))
            {
                if (FileAccess.FileExists(SavePathBak))
                    DirAccess.RemoveAbsolute(SavePathBak);
                DirAccess.RenameAbsolute(SavePath, SavePathBak);
            }

            // Commit new save
            DirAccess.RenameAbsolute(SavePathTmp, SavePath);

            CurrentSave = data;
            EmitSignal(SignalName.SaveCompleted);
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to write save: {ex.Message}");
        }
    }
}
