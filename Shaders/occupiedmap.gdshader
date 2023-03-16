shader_type canvas_item;


uniform vec3 colors[14];


int provinceID(vec3 color){
	return int((color.r + color.g*256.0f)*255.0f) - 1;
}

vec4 colorForProvince(vec3 color){
	return vec4(colors[provinceID(color.rgb)], 0.5f); 
}


const float frequency = 60.0;
void fragment() {
	if(COLOR.a > 0.0f){
		COLOR = colorForProvince(COLOR.rgb);
	}
	float stripes = frequency * UV.x +  frequency * UV.y;
    float rounded = floor(stripes);
    
    if (mod(rounded, 2.0) != 0.0)
    {
     	COLOR.a = 0.0f;   
    }
}
