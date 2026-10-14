using Godot;
using System;
using System.Collections.Generic;

public partial class Settings : Control
{
    private Label _clockLabel;
    private Button[] _categoryTabs = new Button[3];
    private VBoxContainer _rowsContainer;
    private Label _hintLabel;
    private Button _btnBack;

    private ColorRect _controlModal;
    private Button _btnCloseModal;

    private SettingsManager _settingsManager;

    private class SettingItem
    {
        public string Title;
        public string Section;
        public string Key;
        public enum Type { Slider, Toggle, Choice, Action }
        public Type ItemType;
        public string[] Choices;
        public string Hint;
        public Action OnAction;
        public int MinVal = 0;
        public int MaxVal = 100;
        public int Step = 10;
    }

    private class Category
    {
        public string Name;
        public List<SettingItem> Items = new();
    }

    private List<Category> _categories = new();
    private int _currentCategoryIndex = 0;
    private int _currentRowIndex = 0;
    private List<Button> _rowButtons = new();
    private List<Label> _markerLabels = new();
    private List<Label> _titleLabels = new();
    private List<RichTextLabel> _valueRtls = new();

    private readonly string[] _baseTabNames = new[]
    {
        "[ 1. SUARA / AUDIO ]",
        "[ 2. GRAFIS & EFEK ]",
        "[ 3. KONTROL & TEKS ]"
    };

    private double _clockTimer = 0.0;

    public override void _Ready()
    {
        _settingsManager = GetNodeOrNull<SettingsManager>("/root/SettingsManager");

        // Bind Nodes
        _clockLabel = GetNode<Label>("Margin/MainVBox/Header/ClockLabel");
        _categoryTabs[0] = GetNode<Button>("Margin/MainVBox/CenterWrap/SettingsCard/CardMargin/CardVBox/CategoryBar/Tab0");
        _categoryTabs[1] = GetNode<Button>("Margin/MainVBox/CenterWrap/SettingsCard/CardMargin/CardVBox/CategoryBar/Tab1");
        _categoryTabs[2] = GetNode<Button>("Margin/MainVBox/CenterWrap/SettingsCard/CardMargin/CardVBox/CategoryBar/Tab2");
        _rowsContainer = GetNode<VBoxContainer>("Margin/MainVBox/CenterWrap/SettingsCard/CardMargin/CardVBox/RowsContainer");
        _hintLabel = GetNode<Label>("Margin/MainVBox/CenterWrap/SettingsCard/CardMargin/CardVBox/HintContainer/HintLabel");
        _btnBack = GetNode<Button>("Margin/MainVBox/Footer/BtnBack");

        _controlModal = GetNode<ColorRect>("ControlModal");
        _btnCloseModal = GetNode<Button>("ControlModal/ModalCenter/ModalPanel/ModalMargin/ModalVBox/BtnCloseModal");

        // Setup Categories
        BuildCategories();

        // Wire Tab buttons
        for (int i = 0; i < 3; i++)
        {
            int idx = i;
            _categoryTabs[i].Pressed += () => SwitchCategory(idx);
        }

        // Wire Footer & Modal
        _btnBack.Pressed += GoBack;
        _btnCloseModal.Pressed += () => ShowModal(false);

        // Build active category rows
        PopulateCategoryRows();

        // Update real time clock
        UpdateClock();
    }

    private void BuildCategories()
    {
        // --- 1. Audio ---
        var audioCat = new Category { Name = "AUDIO" };
        audioCat.Items.Add(new SettingItem
        {
            Title = "VOLUME UTAMA",
            Section = "audio",
            Key = "volume_master",
            ItemType = SettingItem.Type.Slider,
            MinVal = 0, MaxVal = 100, Step = 10,
            Hint = "Atur volume suara keseluruhan game."
        });
        audioCat.Items.Add(new SettingItem
        {
            Title = "VOLUME SUARA (SFX)",
            Section = "audio",
            Key = "volume_sfx",
            ItemType = SettingItem.Type.Slider,
            MinVal = 0, MaxVal = 100, Step = 10,
            Hint = "Atur kekerasan efek suara lingkungan, langkah kaki, dan interaksi."
        });
        audioCat.Items.Add(new SettingItem
        {
            Title = "VOLUME AMBIENCE",
            Section = "audio",
            Key = "volume_ambience",
            ItemType = SettingItem.Type.Slider,
            MinVal = 0, MaxVal = 100, Step = 10,
            Hint = "Atur intensitas dengung atmosfer dan kesunyian mencekam sekolah."
        });
        audioCat.Items.Add(new SettingItem
        {
            Title = "VOICE ACTING (VO)",
            Section = "audio",
            Key = "volume_vo",
            ItemType = SettingItem.Type.Slider,
            MinVal = 0, MaxVal = 100, Step = 10,
            Hint = "Atur kejernihan suara karakter Diana dan radio HT OSIS."
        });
        _categories.Add(audioCat);

        // --- 2. Graphics & Effects ---
        var gfxCat = new Category { Name = "GRAFIS" };
        gfxCat.Items.Add(new SettingItem
        {
            Title = "KECERAHAN (GAMMA)",
            Section = "display",
            Key = "brightness",
            ItemType = SettingItem.Type.Slider,
            MinVal = 0, MaxVal = 100, Step = 10,
            Hint = "*Disarankan redup agar kuntilanak tidak mudah terlihat."
        });
        gfxCat.Items.Add(new SettingItem
        {
            Title = "PRESET GRAFIS",
            Section = "graphics",
            Key = "graphics_preset",
            ItemType = SettingItem.Type.Choice,
            Choices = new[] { "RENDAH", "SEDANG", "TINGGI" },
            Hint = "Rendah: bayangan mati | Sedang: bayangan senter | Tinggi: kualitas maksimal."
        });
        gfxCat.Items.Add(new SettingItem
        {
            Title = "EFEK RETRO (CRT & DITHER)",
            Section = "effects",
            Key = "retro_effects",
            ItemType = SettingItem.Type.Toggle,
            Hint = "Aktifkan efek CRT scanlines, grain, dan dither PS1."
        });
        gfxCat.Items.Add(new SettingItem
        {
            Title = "KURANGI KEDIP (AKSESIBILITAS)",
            Section = "effects",
            Key = "reduce_flicker",
            ItemType = SettingItem.Type.Toggle,
            Hint = "Nonaktifkan lampu berkedip dan efek flicker untuk kenyamanan visual."
        });
        _categories.Add(gfxCat);

        // --- 3. Controls & Subtitles ---
        var ctrlCat = new Category { Name = "KONTROL" };
        ctrlCat.Items.Add(new SettingItem
        {
            Title = "SENSITIVITAS MOUSE",
            Section = "controls",
            Key = "mouse_sensitivity",
            ItemType = SettingItem.Type.Slider,
            MinVal = 10, MaxVal = 100, Step = 5,
            Hint = "Kecepatan rotasi kamera saat melihat sekeliling."
        });
        ctrlCat.Items.Add(new SettingItem
        {
            Title = "SUBTITLE INDONESIA",
            Section = "subtitle",
            Key = "subtitle_on",
            ItemType = SettingItem.Type.Toggle,
            Hint = "Tampilkan teks subtitle bahasa Indonesia untuk dialog."
        });
        ctrlCat.Items.Add(new SettingItem
        {
            Title = "UKURAN SUBTITLE",
            Section = "subtitle",
            Key = "subtitle_size",
            ItemType = SettingItem.Type.Slider,
            MinVal = 16, MaxVal = 48, Step = 2,
            Hint = "Besar font subtitle: 16px (kecil) hingga 48px (sangat besar)."
        });
        ctrlCat.Items.Add(new SettingItem
        {
            Title = "KONFIGURASI KONTROL",
            ItemType = SettingItem.Type.Action,
            OnAction = () => ShowModal(true),
            Hint = "Tekan ENTER atau klik untuk melihat daftar tombol kendali Diana."
        });
        _categories.Add(ctrlCat);
    }

    private void PopulateCategoryRows()
    {
        // Clear previous rows
        foreach (Node child in _rowsContainer.GetChildren())
        {
            child.QueueFree();
        }

        _rowButtons.Clear();
        _markerLabels.Clear();
        _titleLabels.Clear();
        _valueRtls.Clear();

        var currentItems = _categories[_currentCategoryIndex].Items;

        for (int i = 0; i < currentItems.Count; i++)
        {
            var item = currentItems[i];
            int rowIndex = i;

            var btn = new Button
            {
                CustomMinimumSize = new Vector2(0, 42),
                Flat = true,
                Alignment = HorizontalAlignment.Left
            };

            var margin = new MarginContainer
            {
                MouseFilter = MouseFilterEnum.Ignore
            };
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", 12);
            margin.AddThemeConstantOverride("margin_right", 12);

            var hbox = new HBoxContainer
            {
                MouseFilter = MouseFilterEnum.Ignore
            };
            hbox.AddThemeConstantOverride("separation", 10);

            var marker = new Label
            {
                Text = ">",
                CustomMinimumSize = new Vector2(16, 0),
                MouseFilter = MouseFilterEnum.Ignore
            };
            marker.AddThemeFontSizeOverride("font_size", 24);
            marker.AddThemeColorOverride("font_color", new Color("#8B1E1E"));

            var title = new Label
            {
                Text = item.Title,
                MouseFilter = MouseFilterEnum.Ignore
            };
            title.AddThemeFontSizeOverride("font_size", 24);
            title.AddThemeColorOverride("font_color", new Color("#D8D2C4"));

            var spacer = new Control
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                MouseFilter = MouseFilterEnum.Ignore
            };

            var valueRtl = new RichTextLabel
            {
                BbcodeEnabled = true,
                FitContent = true,
                CustomMinimumSize = new Vector2(300, 32),
                ScrollActive = false,
                AutowrapMode = TextServer.AutowrapMode.Off,
                MouseFilter = MouseFilterEnum.Ignore
            };
            valueRtl.AddThemeFontSizeOverride("normal_font_size", 24);

            hbox.AddChild(marker);
            hbox.AddChild(title);
            hbox.AddChild(spacer);
            hbox.AddChild(valueRtl);

            margin.AddChild(hbox);
            btn.AddChild(margin);

            _rowsContainer.AddChild(btn);

            _rowButtons.Add(btn);
            _markerLabels.Add(marker);
            _titleLabels.Add(title);
            _valueRtls.Add(valueRtl);

            btn.FocusEntered += () => OnRowFocused(rowIndex);
            btn.MouseEntered += () => OnRowMouseEntered(rowIndex);
            btn.Pressed += () => OnRowPressed(rowIndex);
        }

        UpdateCategoryTabsVisual();
        UpdateAllVisuals();

        CallDeferred(nameof(RestoreFocus));
    }

    private void RestoreFocus()
    {
        if (_rowButtons.Count > 0)
        {
            _currentRowIndex = Mathf.Clamp(_currentRowIndex, 0, _rowButtons.Count - 1);
            _rowButtons[_currentRowIndex].GrabFocus();
        }
    }

    private void SwitchCategory(int newIndex)
    {
        if (_currentCategoryIndex == newIndex) return;

        AudioManager.Instance?.PlayUi("click");
        _currentCategoryIndex = newIndex;
        _currentRowIndex = 0;
        PopulateCategoryRows();
    }

    private void UpdateCategoryTabsVisual()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == _currentCategoryIndex)
            {
                _categoryTabs[i].Text = $"> {_baseTabNames[i]} <";
                _categoryTabs[i].AddThemeColorOverride("font_color", new Color("#8B1E1E"));
            }
            else
            {
                _categoryTabs[i].Text = $"  {_baseTabNames[i]}  ";
                _categoryTabs[i].AddThemeColorOverride("font_color", new Color("#3E4A3A"));
            }
        }
    }

    private void OnRowFocused(int index)
    {
        _currentRowIndex = index;
        UpdateAllVisuals();
    }

    private void OnRowMouseEntered(int index)
    {
        _currentRowIndex = index;
        _rowButtons[index].GrabFocus();
        UpdateAllVisuals();
    }

    private void OnRowPressed(int index)
    {
        var item = _categories[_currentCategoryIndex].Items[index];
        if (item.ItemType == SettingItem.Type.Action)
        {
            item.OnAction?.Invoke();
            AudioManager.Instance?.PlayUi("click");
        }
        else
        {
            AdjustValue(1);
        }
    }

    public override void _Process(double delta)
    {
        _clockTimer += delta;
        if (_clockTimer >= 1.0)
        {
            _clockTimer = 0.0;
            UpdateClock();
        }
    }

    private void UpdateClock()
    {
        if (_clockLabel != null)
        {
            _clockLabel.Text = $"WAKTU: {DateTime.Now:HH:mm:ss} WIB";
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (_controlModal.Visible)
        {
            if (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("ui_accept"))
            {
                ShowModal(false);
                GetViewport().SetInputAsHandled();
            }
            return;
        }

        if (@event.IsActionPressed("ui_cancel"))
        {
            GoBack();
            GetViewport().SetInputAsHandled();
        }
        else if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
        {
            if (keyEvent.Keycode == Key.Q || @event.IsActionPressed("ui_page_up"))
            {
                SwitchCategory((_currentCategoryIndex - 1 + 3) % 3);
                GetViewport().SetInputAsHandled();
            }
            else if (keyEvent.Keycode == Key.E || @event.IsActionPressed("ui_page_down"))
            {
                SwitchCategory((_currentCategoryIndex + 1) % 3);
                GetViewport().SetInputAsHandled();
            }
        }

        if (@event.IsActionPressed("ui_left"))
        {
            AdjustValue(-1);
            GetViewport().SetInputAsHandled();
        }
        else if (@event.IsActionPressed("ui_right"))
        {
            AdjustValue(1);
            GetViewport().SetInputAsHandled();
        }
        else if (@event.IsActionPressed("ui_up"))
        {
            if (_rowButtons.Count > 0)
            {
                _currentRowIndex = (_currentRowIndex - 1 + _rowButtons.Count) % _rowButtons.Count;
                _rowButtons[_currentRowIndex].GrabFocus();
                GetViewport().SetInputAsHandled();
            }
        }
        else if (@event.IsActionPressed("ui_down"))
        {
            if (_rowButtons.Count > 0)
            {
                _currentRowIndex = (_currentRowIndex + 1) % _rowButtons.Count;
                _rowButtons[_currentRowIndex].GrabFocus();
                GetViewport().SetInputAsHandled();
            }
        }
    }

    private void AdjustValue(int direction)
    {
        var items = _categories[_currentCategoryIndex].Items;
        if (_currentRowIndex < 0 || _currentRowIndex >= items.Count) return;

        var item = items[_currentRowIndex];

        if (item.ItemType == SettingItem.Type.Action)
        {
            item.OnAction?.Invoke();
            AudioManager.Instance?.PlayUi("click");
            return;
        }

        if (_settingsManager == null) return;
        Variant currentVal = _settingsManager.GetValue(item.Section, item.Key);

        if (item.ItemType == SettingItem.Type.Slider)
        {
            int val = currentVal.AsInt32();
            val = Mathf.Clamp(val + direction * item.Step, item.MinVal, item.MaxVal);
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

        AudioManager.Instance?.PlayUi("click");
        UpdateAllVisuals();
    }

    private void UpdateAllVisuals()
    {
        var items = _categories[_currentCategoryIndex].Items;

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            bool isSelected = i == _currentRowIndex;

            // Marker
            _markerLabels[i].Text = isSelected ? ">" : " ";
            _markerLabels[i].Visible = isSelected;

            // Title
            if (isSelected)
            {
                _titleLabels[i].AddThemeColorOverride("font_color", new Color("#8B1E1E"));
            }
            else
            {
                _titleLabels[i].AddThemeColorOverride("font_color", new Color("#D8D2C4"));
            }

            // Value Text (BBCode)
            Variant val = _settingsManager != null ? _settingsManager.GetValue(item.Section, item.Key) : default;
            string valueFormatted = "";

            if (item.ItemType == SettingItem.Type.Slider)
            {
                int intVal = val.AsInt32();
                int steps = item.Key == "subtitle_size"
                    ? (intVal - 16) * 10 / 32
                    : (intVal - item.MinVal) * 10 / Mathf.Max(1, item.MaxVal - item.MinVal);
                steps = Mathf.Clamp(steps, 0, 10);

                string filled = new string('■', steps);
                string empty = new string('□', 10 - steps);

                string valSuffix = item.Key == "subtitle_size" ? "px" : "%";

                if (isSelected)
                {
                    valueFormatted = $"[right][color=#8B1E1E]{filled}[/color][color=#3E4A3A]{empty}[/color]  [color=#8B1E1E]{intVal}{valSuffix}[/color][/right]";
                }
                else
                {
                    valueFormatted = $"[right][color=#D8D2C4]{filled}[/color][color=#3E4A3A]{empty}[/color]  [color=#D8D2C4]{intVal}{valSuffix}[/color][/right]";
                }
            }
            else if (item.ItemType == SettingItem.Type.Toggle)
            {
                bool bVal = val.AsBool();
                if (bVal)
                {
                    valueFormatted = "[right][color=#9FB8C8][ AKTIF ][/color]  [color=#3E4A3A]MATI[/color][/right]";
                }
                else
                {
                    valueFormatted = "[right][color=#3E4A3A]AKTIF[/color]  [color=#9FB8C8][ MATI ][/color][/right]";
                }
            }
            else if (item.ItemType == SettingItem.Type.Choice)
            {
                int cIdx = Mathf.Clamp(val.AsInt32(), 0, item.Choices.Length - 1);
                valueFormatted = $"[right][color=#3E4A3A]<[/color] [color=#9FB8C8]{item.Choices[cIdx]}[/color] [color=#3E4A3A]>[/color][/right]";
            }
            else if (item.ItemType == SettingItem.Type.Action)
            {
                valueFormatted = isSelected
                    ? "[right][color=#8B1E1E][ TEKAN ENTER ][/color][/right]"
                    : "[right][color=#D8D2C4][ TEKAN ENTER ][/color][/right]";
            }

            _valueRtls[i].Text = valueFormatted;
        }

        // Update Hint Label
        if (_currentRowIndex >= 0 && _currentRowIndex < items.Count)
        {
            _hintLabel.Text = items[_currentRowIndex].Hint;
        }
    }

    private void ShowModal(bool show)
    {
        _controlModal.Visible = show;
        if (show)
        {
            _btnCloseModal.GrabFocus();
        }
        else
        {
            RestoreFocus();
        }
    }

    private void GoBack()
    {
        AudioManager.Instance?.PlayUi("click");

        // If instantiated inside pause menu CanvasLayer, just free itself
        if (GetParent() is CanvasLayer || GetParent()?.Name == "PauseMenu")
        {
            QueueFree();
            return;
        }

        var sceneManager = GetNodeOrNull<SceneManager>("/root/SceneManager");
        if (sceneManager != null)
        {
            sceneManager.GoTo(GameFlow.MainMenu);
        }
        else
        {
            GetTree().ChangeSceneToFile("res://scenes/menu/MainMenu.tscn");
        }
    }
}
