shader_type canvas_item;


uniform vec3 colors[14];
uniform int viewMod: hint_range(0, 1);
uniform int selectedID: hint_range(1,14);
const vec4 emptycolor = vec4(0.0f, 0.0f, 0.0f, 1.0f);
const vec4 whiteColor = vec4(1.0f, 1.0f, 1.0f, 0.3f);
const float thickness = 1.0f;


int provinceID(vec3 color){
	return int((color.r + color.g*256.0f)*255.0f) - 1;
}

vec4 colorForProvince(vec3 color){
	return vec4(colors[provinceID(color.rgb)], 1); 
}

void fragment() {
	if(COLOR.a > 0.0f){
		COLOR = colorForProvince(COLOR.rgb);
	}
	
	vec2 uv = UV;
	vec4 color = texture(TEXTURE, vec2(UV.x, UV.y));
	vec4 color1 = emptycolor;
	vec2 pixel_size = 1.0 / vec2(textureSize(TEXTURE, 0));
	float sum = color.a;
    for(int y = -1; y <= 1; y++)
    for(int x = -1; x <= 1; x++)
    {
        vec2 pixel_off = vec2(float(x), float(y));
        vec4 tex = texture(TEXTURE, UV + pixel_off * pixel_size);
		sum += tex.a;
		sum += texture(TEXTURE, UV + pixel_size * vec2(pixel_off.x + thickness, pixel_off.y)).a;
		sum += texture(TEXTURE, UV + pixel_size * vec2(pixel_off.x, pixel_off.y + thickness)).a;
		sum += texture(TEXTURE, UV + pixel_size * vec2(pixel_off.x + thickness, pixel_off.y + thickness)).a;
		sum += texture(TEXTURE, UV + pixel_size * vec2(pixel_off.x - thickness, pixel_off.y + thickness)).a;
		sum += texture(TEXTURE, UV + pixel_size * vec2(pixel_off.x - thickness, pixel_off.y - thickness)).a;
		sum += texture(TEXTURE, UV + pixel_size * vec2(pixel_off.x - thickness, pixel_off.y)).a;
		sum += texture(TEXTURE, UV + pixel_size * vec2(pixel_off.x + thickness, pixel_off.y-thickness)).a;
		sum += texture(TEXTURE, UV + pixel_size * vec2(pixel_off.x , pixel_off.y-thickness)).a;
		if(tex.rgba != color)
        {
			color1 = tex.rgba;
			break;
        }
    }
	vec4 colorFind = colorForProvince(color.rgb);
	vec4 color1Find = colorForProvince(color1.rgb);
	if(color1 != emptycolor && color1.a > 0.0f){
		if(colorFind == color1Find && viewMod == 0){
			COLOR = mix(colorFind, emptycolor, 0.3f);
		}else if (colorFind != color1Find){
			COLOR = mix(mix(colorFind, color1Find, 0.5f), emptycolor, 0.2f);
		}
		
	}else if(color1 != emptycolor && color.a > 0.0f){
		COLOR = mix(colorFind, emptycolor, 0.4f);
	}
	if(selectedID == provinceID(color.rgb) && color.a > 0.0f){
		COLOR = mix(COLOR, vec4(1.0f, 1.0f, 1.0f, 1.0f), cos(TIME*2.0f)/3.0f);
	}
	if(sum>0.0f && color.a == 0.0f){
		COLOR = emptycolor;
	}
  	
}


