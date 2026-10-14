using Godot;
using System;

public partial class Credits : Control
{
    private ScrollContainer _scrollContainer;
    private VBoxContainer _creditsContent;
    private Button _btnKembali;
    private SceneManager _sceneManager;

    [Export] public float ScrollSpeed { get; set; } = 48f;
    [Export] public float FastMultiplier { get; set; } = 3.0f;

    private float _currentScroll = 0f;
    private float _maxScroll = 0f;
    private bool _isFinished = false;
    private float _endTimer = 0f;

    public override void _Ready()
    {
        _scrollContainer = GetNode<ScrollContainer>("CenterContainer/ScrollContainer");
        _creditsContent = GetNode<VBoxContainer>("CenterContainer/ScrollContainer/CreditsContent");
        _btnKembali = GetNode<Button>("BottomBar/Margin/HBox/BtnKembali");
        _sceneManager = GetNodeOrNull<SceneManager>("/root/SceneManager");

        _btnKembali.Pressed += OnKembaliPressed;

        Input.MouseMode = Input.MouseModeEnum.Visible;
        _btnKembali.GrabFocus();

        CallDeferred(nameof(InitScrollBounds));
    }

    private void InitScrollBounds()
    {
        _scrollContainer.ScrollVertical = 0;
        _currentScroll = 0f;

        // Give Godot a frame to calculate full content height
        GetTree().ProcessFrame += () =>
        {
            if (IsInstanceValid(_creditsContent) && IsInstanceValid(_scrollContainer))
            {
                _maxScroll = Mathf.Max(0f, _creditsContent.Size.Y - _scrollContainer.Size.Y + 120f);
            }
        };
    }

    public override void _Process(double delta)
    {
        if (_isFinished)
        {
            _endTimer += (float)delta;
            if (_endTimer > 3.0f)
            {
                ReturnToMenu();
            }
            return;
        }

        // Keep internal scroll tracker synced if player used mouse wheel or scrollbar
        if (Mathf.Abs(_scrollContainer.ScrollVertical - (int)_currentScroll) > 16)
        {
            _currentScroll = _scrollContainer.ScrollVertical;
        }

        float speed = ScrollSpeed;
        if (Input.IsActionPressed("ui_select") || Input.IsKeyPressed(Key.Space) || Input.IsKeyPressed(Key.Down))
        {
            speed *= FastMultiplier;
        }

        _currentScroll += speed * (float)delta;
        _scrollContainer.ScrollVertical = (int)_currentScroll;

        if (_currentScroll >= _maxScroll && _maxScroll > 0)
        {
            _isFinished = true;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("pause"))
        {
            ReturnToMenu();
            GetViewport().SetInputAsHandled();
        }
    }

    private void OnKembaliPressed()
    {
        ReturnToMenu();
    }

    private void ReturnToMenu()
    {
        if (_sceneManager != null)
        {
            _sceneManager.GoTo(GameFlow.MainMenu);
        }
        else
        {
            GetTree().ChangeSceneToFile("res://scenes/menu/MainMenu.tscn");
        }
    }
}
