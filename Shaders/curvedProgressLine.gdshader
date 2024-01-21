shader_type canvas_item;
render_mode unshaded;

uniform float value: hint_range(0.0, 1.0, 0.01);
uniform float smooth_size: hint_range(0.0, 1.0, 0.0001);
uniform sampler2D mask: source_color;
uniform vec4 color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
	vec4 mask_texture = texture(mask, UV);
	float alpha;
	alpha = 1.0 - smoothstep(value, value + smooth_size, mask_texture.r * (1.0 - smooth_size) + smooth_size);
	COLOR.rgb = color.rgb;
	COLOR = vec4(color.rgb, mask_texture.a * alpha);
}