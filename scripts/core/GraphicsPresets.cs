using Godot;
using System;

public static class GraphicsPresets
{
    // Celeron-friendly defaults based on PRD V2
    public static readonly GraphicsPreset Low = new GraphicsPreset()
    {
        Scaling3D = 0.60f,
        FlashlightShadows = false,
        Ps1Shader = false,
        MaxLights = 4,
        FogDistance = 20.0f,
        GrainAmount = 0.0f,
        VignetteOpacity = 0.0f,
        Dither = false
    };

    public static readonly GraphicsPreset Medium = new GraphicsPreset()
    {
        Scaling3D = 0.75f,
        FlashlightShadows = true,
        ShadowResolution = 512,
        Ps1Shader = true,
        MaxLights = 8,
        FogDistance = 35.0f,
        GrainAmount = 0.03f,
        VignetteOpacity = 0.3f,
        Dither = true
    };

    public static readonly GraphicsPreset High = new GraphicsPreset()
    {
        Scaling3D = 1.0f,
        FlashlightShadows = true,
        ShadowResolution = 2048,
        Ps1Shader = true,
        MaxLights = 16,
        FogDistance = 50.0f,
        GrainAmount = 0.05f,
        VignetteOpacity = 0.5f,
        Dither = true
    };

    public static void Apply(int level, SceneTree tree)
    {
        GraphicsPreset p = level switch
        {
            0 => Low,
            1 => Medium,
            _ => High
        };

        // Apply Scaling3D to main viewport
        var viewport = tree.Root;
        viewport.Scaling3DScale = p.Scaling3D;

        // Propagate to a global event or settings so that game objects (Flashlight, WorldEnvironment, RetroOverlay) can adapt.
        // Actually SettingsManager triggers SettingChanged("graphics_preset").
        // But for things we can set directly:
        // Shadows and lights are updated via nodes listening to the event, or we can find them dynamically.
        // For now, we'll let nodes subscribe to SettingsManager's SettingChanged to apply specific properties like shadow resolution.
    }
}
