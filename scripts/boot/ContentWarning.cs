using Godot;
using System;

public partial class ContentWarning : Control
{
    private CheckButton _toggle;
    private Button _continueButton;
    // We will need a reference to SettingRow to set its title properly.

    public override void _Ready()
    {
        // Try to get nodes
        var row = GetNode<Node>("CenterContainer/VBoxContainer/HBoxContainer/SettingRow");
        if (row.HasMethod("SetTitle"))
        {
            row.Call("SetTitle", "KURANGI KEDIP");
        }
        else
        {
            var title = row.GetNode<Label>("TitleLabel");
            if (title != null) title.Text = "KURANGI KEDIP";
        }

        _toggle = GetNode<CheckButton>("CenterContainer/VBoxContainer/HBoxContainer/SettingRow/Toggle");
        // We will move Toggle into ControlSlot for layout if it exists
        var slot = row.GetNodeOrNull<Control>("ControlSlot");
        if (slot != null && _toggle != null && _toggle.GetParent() == row)
        {
            row.RemoveChild(_toggle);
            slot.AddChild(_toggle);
            _toggle.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
        }

        if (_toggle != null)
        {
            _toggle.Toggled += OnToggle;
        }

        _continueButton = GetNode<Button>("CenterContainer/VBoxContainer/ContinueButton");
        if (_continueButton != null)
        {
            _continueButton.Pressed += OnContinuePressed;
            _continueButton.GrabFocus(); // Auto focus continue button
        }
    }

    private void OnToggle(bool buttonPressed)
    {
        _toggle.Text = buttonPressed ? "ON" : "OFF";
        // TODO: Save this in SettingsManager when C3 is done
        // For now we just emit a global flag or do nothing until SettingsManager is ready
    }

    private void OnContinuePressed()
    {
        // Transition to MainMenu. We should use SceneManager if available, otherwise fallback.
        // SceneManager is milestone C1 as well.
        if (GetNodeOrNull("/root/SceneManager") != null)
        {
            GetNode("/root/SceneManager").Call("GoTo", 2); // 2 is MainMenu based on enum GameFlow { Boot, ContentWarning, MainMenu, Prolog, Gameplay, EndingA, EndingB, EndingC, Credits }
        }
        else
        {
            // Fallback for testing C1 without SceneManager fully hooked up
            GetTree().ChangeSceneToFile("res://scenes/menu/MainMenu.tscn");
        }
    }
}
