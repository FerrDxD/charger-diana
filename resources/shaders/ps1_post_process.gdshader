shader_type canvas_item;

// PS1 / Retro CRT filter: dithering and subtle scanlines.
// Designed for Compatibility renderer (low GPU cost on Intel Celeron).
uniform bool enabled = true;
uniform float dither_strength : hint_range(0.0, 0.2) = 0.04;
uniform float scanline_strength : hint_range(0.0, 0.2) = 0.03;

void fragment() {
	if (!enabled) {
		COLOR = vec4(0.0, 0.0, 0.0, 0.0);
	} else {
		// 4x4 Bayer dither matrix
		int x = int(mod(FRAGCOORD.x, 4.0));
		int y = int(mod(FRAGCOORD.y, 4.0));
		int index = x + y * 4;

		float pattern = 0.0;
		if (index == 0) pattern = 0.0 / 16.0;
		else if (index == 1) pattern = 8.0 / 16.0;
		else if (index == 2) pattern = 2.0 / 16.0;
		else if (index == 3) pattern = 10.0 / 16.0;
		else if (index == 4) pattern = 12.0 / 16.0;
		else if (index == 5) pattern = 4.0 / 16.0;
		else if (index == 6) pattern = 14.0 / 16.0;
		else if (index == 7) pattern = 6.0 / 16.0;
		else if (index == 8) pattern = 3.0 / 16.0;
		else if (index == 9) pattern = 11.0 / 16.0;
		else if (index == 10) pattern = 1.0 / 16.0;
		else if (index == 11) pattern = 9.0 / 16.0;
		else if (index == 12) pattern = 15.0 / 16.0;
		else if (index == 13) pattern = 7.0 / 16.0;
		else if (index == 14) pattern = 13.0 / 16.0;
		else pattern = 5.0 / 16.0;

		float scanline = sin(FRAGCOORD.y * 1.5) * scanline_strength;
		float noise = (pattern - 0.5) * dither_strength;

		// Output subtle dark overlay
		COLOR = vec4(0.0, 0.0, 0.0, clamp(noise + scanline, 0.0, 0.15));
	}
}
