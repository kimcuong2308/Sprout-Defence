shader_type canvas_item;

uniform	float red: hint_range(0.0, 1.0, 0.01);
uniform	float green: hint_range(0.0, 1.0, 0.01);
uniform	float blue: hint_range(0.0, 1.0, 0.01);
uniform	float alpha: hint_range(0.0, 1.0, 0.01);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	COLOR = vec4(red, green, blue, alpha);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
