shader_type spatial;

varying vec3 vertex_pos;
varying vec3 normal;

uniform vec3 albedo : source_color = vec3(1.0f);

void vertex() {
	vertex_pos = VERTEX;
	normal = NORMAL;
}

void fragment() {

	ALBEDO = albedo;
}