using Godot;
using System;

public partial class Nhac : AudioStreamPlayer
{
	public void _on_finished(){
		PitchScale += 0.3f;
	}
}
