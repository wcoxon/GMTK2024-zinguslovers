shader_type spatial;

varying vec3 vertex_pos;
varying vec3 normal;

instance uniform vec4 albedo : source_color = vec4(1.0f);

void vertex() {
	vertex_pos = VERTEX;
	normal = NORMAL;
}

void fragment() {
	
	
	ALBEDO = albedo.rgb;
}