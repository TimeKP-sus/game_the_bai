shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
	COLOR = vec4(0.4, 0.6, 0.9, 1.0);

}

void fragment() {
	// Called for every pixel the material is visible on.
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
