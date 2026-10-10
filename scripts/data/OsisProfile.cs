using Godot;

/// <summary>
/// Profil karakteristik anggota OSIS (Cecillia, Adam, Ferdi).
/// Ditetapkan dalam bentuk Resource agar tuning tidak di-hardcode.
/// </summary>
[GlobalClass]
public partial class OsisProfile : Resource
{
    [ExportGroup("Identitas")]
    [Export] public string AgentName { get; set; } = "OSIS";
    [Export] public string Traits { get; set; } = "";

    [ExportGroup("Pergerakan")]
    [Export] public float PatrolSpeed { get; set; } = 1.8f;
    [Export] public float InvestigateSpeed { get; set; } = 2.4f;
    [Export] public float ChaseSpeed { get; set; } = 4.0f;
    [Export] public float PanicSpeed { get; set; } = 4.2f;

    [ExportGroup("Kewaspadaan (Perception)")]
    [Export] public float SightRange { get; set; } = 9f;
    [Export] public float AlertRiseRate { get; set; } = 0.4f;
    [Export] public float SuspiciousThreshold { get; set; } = 0.3f;
    [Export] public float InvestigateThreshold { get; set; } = 0.6f;

    [ExportGroup("Suara (VoiceStyle)")]
    [Export] public float VoicePitch { get; set; } = 1.0f;
    [Export] public string AudioBusName { get; set; } = "VO";
    
    // Dialog lines
    [Export] public string VoInvestigatePath { get; set; } = "vo_osis_curious";
    [Export] public string VoInvestigateText { get; set; } = "Siapa di situ? Ada yang belum pulang?";
    [Export] public string VoChasePath { get; set; } = "vo_osis_chase";
    [Export] public string VoChaseText { get; set; } = "Hei! Berhenti kamu!";
    [Export] public string VoPanicPath { get; set; } = "vo_osis_panic";
    [Export] public string VoPanicText { get; set; } = "A-apaan itu?! LARI!!";
}
