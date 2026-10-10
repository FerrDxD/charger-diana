shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

// Grain parameters
uniform float grain_amount : hint_range(0.0, 1.0) = 0.05;
uniform float grain_size : hint_range(1.0, 10.0) = 1.0;

// Vignette parameters
uniform float vignette_intensity : hint_range(0.0, 2.0) = 0.4;
uniform float vignette_opacity : hint_range(0.0, 1.0) = 0.5;
uniform vec4 vignette_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);

// Dither parameters (simple color banding/quantization to simulate dither)
uniform float color_depth : hint_range(1.0, 256.0) = 32.0;
uniform bool enable_dithering = true;

// A pseudo-random generator
float random(vec2 uv) {
    return fract(sin(dot(uv.xy, vec2(12.9898, 78.233))) * 43758.5453123);
}

void fragment() {
    // Basic screen texture
    vec2 uv = SCREEN_UV;
    vec4 base_color = texture(screen_texture, uv);

    // 1. Dithering / Color Banding
    if (enable_dithering) {
        base_color.rgb = floor(base_color.rgb * color_depth + 0.5) / color_depth;
    }

    // 2. Film Grain
    vec2 grain_uv = uv * (1.0 / grain_size);
    // Add time to make grain animated
    grain_uv.x += TIME * 10.123;
    grain_uv.y += TIME * 14.432;
    float noise = (random(grain_uv) - 0.5) * grain_amount;
    base_color.rgb += vec3(noise);

    // 3. Vignette
    float dist = distance(uv, vec2(0.5, 0.5));
    float vignette = smoothstep(0.5, 0.5 + vignette_intensity, dist);
    base_color.rgb = mix(base_color.rgb, vignette_color.rgb, vignette * vignette_opacity);

    COLOR = vec4(base_color.rgb, base_color.a);
}
