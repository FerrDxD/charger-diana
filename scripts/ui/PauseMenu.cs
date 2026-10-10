using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
    private Button _btnLanjut;
    private Button _btnPengaturan;
    private Button _btnMenu;
    private Control _overlay;

    private Node _settingsInstance;
    private SceneManager _sceneManager;
    private AudioBusLayout _defaultBusLayout;

    public override void _Ready()
    {
        _overlay = GetNode<Control>("Overlay");
        _overlay.Hide();

        _btnLanjut = GetNode<Button>("Overlay/SideColumn/Margin/VBox/BtnLanjut");
        _btnPengaturan = GetNode<Button>("Overlay/SideColumn/Margin/VBox/BtnPengaturan");
        _btnMenu = GetNode<Button>("Overlay/SideColumn/Margin/VBox/BtnMenu");

        _btnLanjut.Pressed += Resume;
        _btnPengaturan.Pressed += OpenSettings;
        _btnMenu.Pressed += GoToMenu;

        _sceneManager = GetNodeOrNull<SceneManager>("/root/SceneManager");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            // Only allow pausing if in Gameplay flow
            bool isGameplay = _sceneManager != null && _sceneManager.CurrentFlow == GameFlow.Gameplay;
            if (!isGameplay) return;

            if (_settingsInstance != null)
            {
                // Close settings if it's open
                CloseSettings();
                GetViewport().SetInputAsHandled();
                return;
            }

            if (GetTree().Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            GetViewport().SetInputAsHandled();
        }
    }

    private void Pause()
    {
        GetTree().Paused = true;
        _overlay.Show();
        _btnLanjut.GrabFocus();

        // Mute Ambience
        int ambienceBus = AudioServer.GetBusIndex("Ambience");
        if (ambienceBus >= 0)
        {
            AudioServer.SetBusMute(ambienceBus, true);
        }
    }

    private void Resume()
    {
        GetTree().Paused = false;
        _overlay.Hide();

        int ambienceBus = AudioServer.GetBusIndex("Ambience");
        if (ambienceBus >= 0)
        {
            AudioServer.SetBusMute(ambienceBus, false);
        }
    }

    private void OpenSettings()
    {
        if (_settingsInstance == null)
        {
            var settingsScene = GD.Load<PackedScene>("res://scenes/ui/Settings.tscn");
            if (settingsScene != null)
            {
                _settingsInstance = settingsScene.Instantiate();
                AddChild(_settingsInstance);
                
                // Hide background of settings so it looks like it's over the pause menu/game
                var bg = _settingsInstance.GetNodeOrNull<ColorRect>("Background");
                if (bg != null) bg.Hide();
            }
        }
    }

    private void CloseSettings()
    {
        if (_settingsInstance != null)
        {
            _settingsInstance.QueueFree();
            _settingsInstance = null;
            _btnPengaturan.GrabFocus();
        }
    }

    private void GoToMenu()
    {
        Resume();
        if (_sceneManager != null)
        {
            _sceneManager.GoTo(GameFlow.MainMenu);
        }
    }
}
