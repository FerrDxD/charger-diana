using Godot;

/// <summary>
/// Level controller for TestLevel.
/// Starts background ambience and plays introductory Diana monolog.
/// </summary>
public partial class TestLevel : Node3D
{
	public override void _Ready()
	{
		// Start school night ambience
		AudioManager.Instance?.PlayAmbience("amb_night_school");

		// Intro monolog Diana
		GetTree().CreateTimer(1.2).Timeout += () =>
		{
			AudioManager.Instance?.PlayVo(
				"vo_diana_intro",
				"Diana",
				"Sial... baterai tinggal segini dan charger malah ketinggalan di kolong meja kelas.",
				4.0f
			);
		};
	}
}
