shader_type canvas_item;

uniform bool colors[421];

uniform uint colorMode = 0;

//TODO: Add blur on the borders

uniform bool lowQualityMode = false;

uniform float timescale = 0.01f;
uniform int pixelSize = 3;
uniform float hScrollSpeedFactor = 1.;
uniform float vScrollSpeedFactor = 1.;
uniform float entropyFactor = 10.;
uniform float sharpness = 10.;

const vec4 emptycolor = vec4(0.0f, 0.0f, 0.0f, 0.0f);

int provinceID(vec3 provinceColor){
	return int((provinceColor.r + provinceColor.g*256.0f + provinceColor.b*256.0f*256.0f)*255.0f) - 1;
}

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_nearest_mipmap;

vec3 hash(vec3 p)
{
    p = vec3(dot(p, vec3(127.1, 311.7, 74.7)), dot(p, vec3(269.5,183.3,246.1)), dot(p, vec3(113.5, 271.9, 124.6)));
    p = fract(sin(p) * 43758.5453123);
    return p;
}

float shard_noise(in vec3 p, in float _sharpness) {
    vec3 ip = floor(p);
    vec3 fp = fract(p);

    float v = 0., t = 0.;
    for (int z = -1; z <= 1; z++) {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                vec3 o = vec3(float(x), float(y), float(z));
                vec3 io = ip + o;
                vec3 h = hash(io);
                vec3 r = fp - (o + h);

                float w = exp2(-TAU*dot(r, r));
                // tanh deconstruction and optimization by @Xor
                float s = _sharpness * dot(r, hash(io + vec3(11, 31, 47)) - 0.5);
                v += w * s*inversesqrt(1.0+s*s);
                t += w;
            }
        }
    }
    return ((v / t) * .5) + .5;
}

vec4 mainImage( in vec2 _uv )
{

    vec3 uv = vec3(_uv.x + TIME*timescale*hScrollSpeedFactor, _uv.y + TIME*timescale*vScrollSpeedFactor, TIME*timescale * .5 * entropyFactor);

    vec4 C = vec4(vec3(1.0f-
            (shard_noise(128.0*uv,sharpness) * .03125) +
            (shard_noise(32.0*uv,sharpness) * .0625) +
            (shard_noise(16.0*uv,sharpness) * .125) +
            (shard_noise(8.0*uv,sharpness) * .25) +
            (shard_noise(4.0*uv,sharpness) * .5)
        ),1.0f);

    return C;
}

void fragment()
{
	vec4 textureColor = texture(TEXTURE, vec2(UV.x, UV.y));
	int tileId = provinceID(textureColor.rgb);
	if(lowQualityMode){
		 if(colors[tileId]){
			COLOR = vec4(0,0,0,0);
		}else{
			COLOR = vec4(0.0f, 0.0f, 0.0f, 0.3f);
		}
	}else{
		ivec2 size = textureSize(TEXTURE, 0);

		int xRes = size.x;
		int yRes = size.y;

		float xFactor = float(xRes) / float(pixelSize);
		float yFactor = float(yRes) / float(pixelSize);



		float grid_uv_x = round(UV.x * xFactor) / xFactor;
		float grid_uv_y = round(UV.y * yFactor) / yFactor;

	    vec4 noise = mainImage(vec2(grid_uv_x, grid_uv_y));
		vec4 finalColor = mix(noise, emptycolor, 0.5f);

	    if(colors[tileId]){
			COLOR = vec4(0,0,0,0);
		}else if (finalColor.r > 0.75f){
			COLOR = finalColor;
		}else{
			COLOR = mix(finalColor, vec4(0.2f,0.2f,0.2f,0.2f), 0.5f);
		}
	}

}
