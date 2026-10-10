using Godot;
using System;

public partial class MainMenu : Control
{
    private Button _btnMulai;
    private Button _btnLanjutkan;
    private Button _btnPengaturan;
    private Button _btnKredit;
    private Button _btnKeluar;

    private ColorRect _confirmDialog;
    private Button _btnYa;
    private Button _btnBatal;

    private SaveSystem _saveSystem;
    private SceneManager _sceneManager;

    public override void _Ready()
    {
        _saveSystem = GetNodeOrNull<SaveSystem>("/root/SaveSystem");
        _sceneManager = GetNodeOrNull<SceneManager>("/root/SceneManager");

        _btnMulai = GetNode<Button>("Margin/MenuContainer/BtnMulai");
        _btnLanjutkan = GetNode<Button>("Margin/MenuContainer/BtnLanjutkan");
        _btnPengaturan = GetNode<Button>("Margin/MenuContainer/BtnPengaturan");
        _btnKredit = GetNode<Button>("Margin/MenuContainer/BtnKredit");
        _btnKeluar = GetNode<Button>("Margin/MenuContainer/BtnKeluar");

        _confirmDialog = GetNode<ColorRect>("ConfirmDialog");
        _btnYa = GetNode<Button>("ConfirmDialog/VBox/HBox/BtnYa");
        _btnBatal = GetNode<Button>("ConfirmDialog/VBox/HBox/BtnBatal");

        _btnMulai.Pressed += OnMulaiPressed;
        _btnLanjutkan.Pressed += OnLanjutkanPressed;
        _btnPengaturan.Pressed += OnPengaturanPressed;
        _btnKredit.Pressed += OnKreditPressed;
        _btnKeluar.Pressed += OnKeluarPressed;

        _btnYa.Pressed += OnConfirmYa;
        _btnBatal.Pressed += OnConfirmBatal;

        SetupButtons();
    }

    private void SetupButtons()
    {
        bool hasSave = _saveSystem != null && _saveSystem.HasSave();
        _btnLanjutkan.Disabled = !hasSave;
        
        if (hasSave)
        {
            _btnLanjutkan.GrabFocus();
        }
        else
        {
            _btnMulai.GrabFocus();
        }
    }

    private void OnMulaiPressed()
    {
        if (_saveSystem != null && _saveSystem.HasSave())
        {
            _confirmDialog.Visible = true;
            _btnBatal.GrabFocus(); // Safe default
        }
        else
        {
            StartNewGame();
        }
    }

    private void OnConfirmYa()
    {
        _saveSystem?.ClearSave();
        StartNewGame();
    }

    private void OnConfirmBatal()
    {
        _confirmDialog.Visible = false;
        _btnMulai.GrabFocus();
    }

    private void OnLanjutkanPressed()
    {
        if (_sceneManager != null)
        {
            _sceneManager.ContinueGame();
        }
    }

    private void OnPengaturanPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/ui/Settings.tscn");
    }

    private void OnKreditPressed()
    {
        if (_sceneManager != null)
        {
            _sceneManager.GoTo(GameFlow.Credits);
        }
    }

    private void OnKeluarPressed()
    {
        GetTree().Quit();
    }

    private void StartNewGame()
    {
        if (_sceneManager != null)
        {
            _sceneManager.StartNewGame();
        }
    }
}
