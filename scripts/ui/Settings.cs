using Godot;
using System;
using System.Collections.Generic;

public partial class Settings : Control
{
    private VBoxContainer _settingsContainer;
    private List<Button> _settingRows = new List<Button>();
    private int _selectedIndex = 0;
    private SettingsManager _settingsManager;

    private class SettingItem
    {
        public string Title;
        public string Section;
        public string Key;
        public enum Type { Slider, Toggle, Choice, Action }
        public Type ItemType;
        public string[] Choices;
        public Action OnAction;
    }

    private List<SettingItem> _items = new List<SettingItem>();

    public override void _Ready()
    {
        _settingsManager = GetNodeOrNull<SettingsManager>("/root/SettingsManager");

        _items.Add(new SettingItem { Title = "VOLUME UTAMA", Section = "audio", Key = "volume_master", ItemType = SettingItem.Type.Slider });
        _items.Add(new SettingItem { Title = "VOLUME SUARA (SFX)", Section = "audio", Key = "volume_sfx", ItemType = SettingItem.Type.Slider });
        _items.Add(new SettingItem { Title = "VOLUME AMBIENCE", Section = "audio", Key = "volume_ambience", ItemType = SettingItem.Type.Slider });
        _items.Add(new SettingItem { Title = "VOICE ACTING (VO)", Section = "audio", Key = "volume_vo", ItemType = SettingItem.Type.Slider });
        
        _items.Add(new SettingItem { Title = "SUBTITLE INDONESIA", Section = "subtitle", Key = "subtitle_on", ItemType = SettingItem.Type.Toggle });
        _items.Add(new SettingItem { Title = "UKURAN SUBTITLE", Section = "subtitle", Key = "subtitle_size", ItemType = SettingItem.Type.Slider }); // We can treat 10-40 as slider
        
        _items.Add(new SettingItem { Title = "KECERAHAN (GAMMA)", Section = "display", Key = "brightness", ItemType = SettingItem.Type.Slider });
        _items.Add(new SettingItem { Title = "PRESET GRAFIS", Section = "graphics", Key = "graphics_preset", ItemType = SettingItem.Type.Choice, Choices = new[] { "RENDAH", "SEDANG", "TINGGI" } });
        _items.Add(new SettingItem { Title = "EFEK RETRO", Section = "effects", Key = "retro_effects", ItemType = SettingItem.Type.Toggle });
        _items.Add(new SettingItem { Title = "KURANGI KEDIP", Section = "effects", Key = "reduce_flicker", ItemType = SettingItem.Type.Toggle });
        
        _items.Add(new SettingItem { Title = "SENSITIVITAS MOUSE", Section = "controls", Key = "mouse_sensitivity", ItemType = SettingItem.Type.Slider });
        _items.Add(new SettingItem { Title = "KONFIGURASI KONTROL", ItemType = SettingItem.Type.Action, OnAction = () => { GD.Print("Konfigurasi Kontrol (C5)"); } });

        // Build UI
        _settingsContainer = GetNode<VBoxContainer>("MainContainer/SettingsPanel/Panel/MarginContainer/VBoxContainer/SettingsContainer");
        
        // Clear existing children
        foreach (Node child in _settingsContainer.GetChildren())
        {
            child.QueueFree();
        }

        var theme = GD.Load<Theme>("res://resources/ui_theme.tres");

        for (int i = 0; i < _items.Count; i++)
        {
            var btn = new Button();
            btn.Theme = theme;
            btn.AddThemeFontSizeOverride("font_size", 24);
            btn.Alignment = HorizontalAlignment.Left;
            btn.Flat = true; // Use flat style
            _settingsContainer.AddChild(btn);
            _settingRows.Add(btn);

            int captureIndex = i;
            btn.FocusEntered += () => OnRowFocused(captureIndex);
            btn.Pressed += () => OnRowPressed(captureIndex);
        }

        // Wait a frame for children to be ready
        CallDeferred(nameof(InitFocus));
    }

    private void InitFocus()
    {
        if (_settingRows.Count > 0)
        {
            _settingRows[0].GrabFocus();
        }
        UpdateVisuals();
    }

    private void OnRowFocused(int index)
    {
        _selectedIndex = index;
        UpdateVisuals();
    }

    private void OnRowPressed(int index)
    {
        AdjustValue(1);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            if (GetNodeOrNull("/root/SceneManager") != null)
                GetNode("/root/SceneManager").Call("GoTo", 2); // GameFlow.MainMenu
            else
                GetTree().ChangeSceneToFile("res://scenes/menu/MainMenu.tscn");
        }
        else if (@event.IsActionPressed("ui_up"))
        {
            _selectedIndex = (_selectedIndex - 1 + _settingRows.Count) % _settingRows.Count;
            _settingRows[_selectedIndex].GrabFocus();
        }
        else if (@event.IsActionPressed("ui_down"))
        {
            _selectedIndex = (_selectedIndex + 1) % _settingRows.Count;
            _settingRows[_selectedIndex].GrabFocus();
        }
        else if (@event.IsActionPressed("ui_left"))
        {
            AdjustValue(-1);
        }
        else if (@event.IsActionPressed("ui_right"))
        {
            AdjustValue(1);
        }
    }

    private void AdjustValue(int direction)
    {
        var item = _items[_selectedIndex];
        
        if (item.ItemType == SettingItem.Type.Action)
        {
            if (direction > 0) item.OnAction?.Invoke();
            return;
        }

        if (_settingsManager == null) return;
        Variant currentVal = _settingsManager.GetValue(item.Section, item.Key);

        if (item.ItemType == SettingItem.Type.Slider)
        {
            int val = currentVal.AsInt32();
            if (item.Key == "subtitle_size")
            {
                val = Mathf.Clamp(val + direction * 2, 16, 48);
            }
            else
            {
                val = Mathf.Clamp(val + direction * 10, 0, 100);
            }
            _settingsManager.SetValue(item.Section, item.Key, val);
        }
        else if (item.ItemType == SettingItem.Type.Toggle)
        {
            bool val = currentVal.AsBool();
            _settingsManager.SetValue(item.Section, item.Key, !val);
        }
        else if (item.ItemType == SettingItem.Type.Choice)
        {
            int val = currentVal.AsInt32();
            val = (val + direction + item.Choices.Length) % item.Choices.Length;
            _settingsManager.SetValue(item.Section, item.Key, val);
        }

        // AudioManager.Instance?.PlayUi("click");
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            var item = _items[i];
            bool isSelected = i == _selectedIndex;
            string prefix = isSelected ? "> " : "  ";

            if (isSelected)
            {
                _settingRows[i].AddThemeColorOverride("font_color", new Color("#B3261E"));
            }
            else
            {
                _settingRows[i].RemoveThemeColorOverride("font_color");
            }

            string valueText = "";
            Variant val = _settingsManager != null ? _settingsManager.GetValue(item.Section, item.Key) : default;

            if (item.ItemType == SettingItem.Type.Slider)
            {
                int intVal = val.AsInt32();
                int steps = item.Key == "subtitle_size" ? (intVal - 16) * 10 / 32 : intVal / 10;
                steps = Mathf.Clamp(steps, 0, 10);
                string filled = new string('■', steps);
                string empty = new string('□', 10 - steps);
                valueText = $"{filled}{empty} | {intVal}";
                if (item.Key != "subtitle_size") valueText += "%";
            }
            else if (item.ItemType == SettingItem.Type.Toggle)
            {
                bool bVal = val.AsBool();
                valueText = bVal ? "[ AKTIF ]  MATI" : "AKTIF  [ MATI ]";
            }
            else if (item.ItemType == SettingItem.Type.Choice)
            {
                valueText = $"< {item.Choices[val.AsInt32()]} >";
            }
            else if (item.ItemType == SettingItem.Type.Action)
            {
                valueText = "[ TEKAN ENTER UNTUK MELIHAT ]";
            }

            string paddedTitle = item.Title.PadRight(25, ' ');
            _settingRows[i].Text = $"{prefix}{paddedTitle} : {valueText}";
        }
    }
}
