using Godot;
using System;

public partial class SettingsManager : Node
{
    private const string SettingsPath = "user://settings.cfg";
    private ConfigFile _config;

    [Signal]
    public delegate void SettingChangedEventHandler(string key);

    public override void _Ready()
    {
        _config = new ConfigFile();
        LoadSettings();
        ApplyAllSettings();
    }

    private void LoadSettings()
    {
        Error err = _config.Load(SettingsPath);
        if (err != Error.Ok)
        {
            // Audio Defaults
            SetValue("audio", "volume_master", 100);
            SetValue("audio", "volume_sfx", 100);
            SetValue("audio", "volume_ambience", 100);
            SetValue("audio", "volume_vo", 100);
            
            // Subtitle Defaults
            SetValue("subtitle", "subtitle_on", true);
            SetValue("subtitle", "subtitle_size", 24);
            
            // Display & Graphics
            SetValue("display", "brightness", 100);
            SetValue("graphics", "graphics_preset", 1); // 0=Low, 1=Medium, 2=High
            
            // Effects
            SetValue("effects", "retro_effects", true);
            SetValue("effects", "reduce_flicker", false);
            
            // Controls
            SetValue("controls", "mouse_sensitivity", 50);
            
            SaveSettings();
        }
    }

    public Variant GetValue(string section, string key, Variant defaultValue = default)
    {
        return _config.GetValue(section, key, defaultValue);
    }

    public void SetValue(string section, string key, Variant value)
    {
        _config.SetValue(section, key, value);
        SaveSettings();
        ApplySetting(section, key, value);
        EmitSignal(SignalName.SettingChanged, key);
    }

    private void SaveSettings()
    {
        _config.Save(SettingsPath);
    }

    private void ApplyAllSettings()
    {
        foreach (string section in _config.GetSections())
        {
            foreach (string key in _config.GetSectionKeys(section))
            {
                ApplySetting(section, key, _config.GetValue(section, key));
            }
        }
    }

    private void ApplySetting(string section, string key, Variant value)
    {
        if (section == "audio" && key.StartsWith("volume_"))
        {
            string busName = key.Replace("volume_", "");
            // Match exactly with proper casing
            switch(busName) {
                case "master": busName = "Master"; break;
                case "sfx": busName = "SFX"; break;
                case "ambience": busName = "Ambience"; break;
                case "vo": busName = "VO"; break;
            }

            int busIndex = AudioServer.GetBusIndex(busName);
            if (busIndex >= 0)
            {
                float percent = (int)value / 100f;
                float db = percent > 0.01f ? Mathf.LinearToDb(percent) : -80f;
                AudioServer.SetBusVolumeDb(busIndex, db);
            }
        }
        else if (section == "graphics" && key == "graphics_preset")
        {
            GraphicsPresets.Apply((int)value, GetTree());
        }
        else if (section == "effects" && key == "reduce_flicker")
        {
            FlickerPolicy.SetReduceFlicker((bool)value);
        }
    }
}
