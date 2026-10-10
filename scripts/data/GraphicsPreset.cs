using Godot;
using System;

[GlobalClass]
public partial class GraphicsPreset : Resource
{
    [Export] public float Scaling3D { get; set; } = 1.0f;
    [Export] public bool FlashlightShadows { get; set; } = true;
    [Export] public int ShadowResolution { get; set; } = 2048;
    [Export] public bool Ps1Shader { get; set; } = true;
    [Export] public int MaxLights { get; set; } = 16;
    [Export] public float FogDistance { get; set; } = 50.0f;
    [Export] public float GrainAmount { get; set; } = 0.05f;
    [Export] public float VignetteOpacity { get; set; } = 0.5f;
    [Export] public bool Dither { get; set; } = true;
}
