using Godot;

/// <summary>
/// Static utility for noise calculations.
/// AI enemies connect to EventBus.NoiseEmitted and use these helpers
/// to decide if they can hear the noise.
/// </summary>
public static class NoiseSystem
{
	public const float MaxHearingRadius = 20f;

	/// <summary>Radius = loudness * MaxHearingRadius.</summary>
	public static float GetHearingRadius(float loudness)
	{
		return loudness * MaxHearingRadius;
	}

	/// <summary>True if listener is within hearing range of the noise.</summary>
	public static bool CanHear(Vector3 listenerPos, Vector3 noisePos, float loudness)
	{
		return listenerPos.DistanceTo(noisePos) <= GetHearingRadius(loudness);
	}
}
