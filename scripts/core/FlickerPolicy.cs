using Godot;

/// <summary>
/// Helper untuk mengatur intensitas efek berkedip agar menghormati opsi "reduce_flicker" pemain.
/// Dipakai oleh lampu rusak, telegraph Kuntilanak, event terskrip, dan menu.
/// </summary>
public static class FlickerPolicy
{
    private static bool _reduceFlickerEnabled = false;

    public static void SetReduceFlicker(bool enabled)
    {
        _reduceFlickerEnabled = enabled;
    }

    /// <summary>
    /// Mengembalikan intensitas efek berkedip yang sudah diskalakan (0.0 - 1.0).
    /// </summary>
    public static float Scale(float intensity)
    {
        if (_reduceFlickerEnabled)
        {
            // Jika reduce_flicker aktif, hilangkan kedip sepenuhnya
            return 0f;
        }
        return intensity;
    }
}
