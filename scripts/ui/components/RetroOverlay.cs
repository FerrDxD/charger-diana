using Godot;
using System;

public partial class RetroOverlay : CanvasLayer
{
    private ColorRect _colorRect;
    private ShaderMaterial _material;

    public override void _Ready()
    {
        _colorRect = GetNode<ColorRect>("ColorRect");
        _material = _colorRect.Material as ShaderMaterial;
        
        var settingsManager = GetNodeOrNull<SettingsManager>("/root/SettingsManager");
        if (settingsManager != null)
        {
            settingsManager.SettingChanged += OnSettingChanged;
            ApplySettings(settingsManager);
        }
    }

    private void OnSettingChanged(string key)
    {
        var settingsManager = GetNodeOrNull<SettingsManager>("/root/SettingsManager");
        if (settingsManager != null && (key == "graphics_preset" || key == "retro_effects"))
        {
            ApplySettings(settingsManager);
        }
    }

    private void ApplySettings(SettingsManager settings)
    {
        bool retroEffects = (bool)settings.GetValue("effects", "retro_effects", true);
        SetEnabled(retroEffects);

        if (retroEffects)
        {
            int preset = (int)settings.GetValue("graphics", "graphics_preset", 1);
            GraphicsPreset p = preset switch
            {
                0 => GraphicsPresets.Low,
                1 => GraphicsPresets.Medium,
                _ => GraphicsPresets.High
            };

            SetIntensity(p.GrainAmount, p.VignetteOpacity, p.Dither);
        }
    }

    public void SetEnabled(bool enabled)
    {
        _colorRect.Visible = enabled;
    }

    public void SetIntensity(float grainAmount, float vignetteOpacity, bool dither)
    {
        if (_material != null)
        {
            _material.SetShaderParameter("grain_amount", grainAmount);
            _material.SetShaderParameter("vignette_opacity", vignetteOpacity);
            _material.SetShaderParameter("enable_dithering", dither);
        }
    }
}
