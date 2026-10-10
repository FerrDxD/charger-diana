using Godot;
using System;
using System.IO;

public partial class GameLog : Node
{
    private string _logPath;
    private Godot.FileAccess _logFile;

    public override void _Ready()
    {
        string logsDir = ProjectSettings.GlobalizePath("user://logs/");
        if (!Directory.Exists(logsDir))
        {
            Directory.CreateDirectory(logsDir);
        }

        CleanOldLogs(logsDir);

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        _logPath = Path.Combine(logsDir, $"charger_diana_{timestamp}.log");

        _logFile = Godot.FileAccess.Open(_logPath, Godot.FileAccess.ModeFlags.Write);
        
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

        Info($"[GameLog] Session started at {DateTime.Now}");
        Info($"[GameLog] OS: {OS.GetName()} - {OS.GetVersion()}");
        Info($"[GameLog] Screen: {DisplayServer.WindowGetSize().X}x{DisplayServer.WindowGetSize().Y}");

        var eventBus = GetNodeOrNull<EventBus>("/root/EventBus");
        if (eventBus != null)
        {
            eventBus.CheckpointReached += (id) => Info($"[Checkpoint] Reached {id}");
        }

        var sceneManager = GetNodeOrNull<SceneManager>("/root/SceneManager");
        if (sceneManager != null)
        {
            sceneManager.FlowChanged += (flow) => Info($"[Flow] Changed to (GameFlow){flow}");
        }
    }

    private void CleanOldLogs(string dirPath)
    {
        var files = Directory.GetFiles(dirPath, "charger_diana_*.log");
        Array.Sort(files);
        if (files.Length > 5)
        {
            for (int i = 0; i < files.Length - 5; i++)
            {
                File.Delete(files[i]);
            }
        }
    }

    public void Info(string message)
    {
        GD.Print(message);
        if (_logFile != null && _logFile.IsOpen())
        {
            _logFile.StoreLine($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
            _logFile.Flush();
        }
    }

    public void Warn(string message)
    {
        GD.PushWarning(message);
        if (_logFile != null && _logFile.IsOpen())
        {
            _logFile.StoreLine($"[WARN] {DateTime.Now:HH:mm:ss} - {message}");
            _logFile.Flush();
        }
    }

    public void Error(string message)
    {
        GD.PushError(message);
        if (_logFile != null && _logFile.IsOpen())
        {
            _logFile.StoreLine($"[ERROR] {DateTime.Now:HH:mm:ss} - {message}");
            _logFile.Flush();
        }
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            Error($"FATAL CRASH: {ex.Message}\n{ex.StackTrace}");
        }
        else
        {
            Error($"FATAL CRASH: {e.ExceptionObject}");
        }
        
        if (_logFile != null && _logFile.IsOpen())
        {
            _logFile.Close();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (_logFile != null && _logFile.IsOpen())
        {
            _logFile.Close();
        }
        base.Dispose(disposing);
    }
}
