using Godot;
using System;

[GlobalClass]
public partial class VoLine : Resource
{
    [Export] public string Id { get; set; }
    [Export] public string Speaker { get; set; }
    [Export] public AudioStream Stream { get; set; }
    [Export(PropertyHint.MultilineText)] public string SubtitleText { get; set; }
    [Export] public bool Radio { get; set; }
    [Export] public float Duration { get; set; } = 0f;
}
