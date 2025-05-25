using Godot;
using System;

public partial class Menu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void _on_choi_button_down(){
		ManHinhCho.canh_moi = "res://scene/online/quan_ly_ket_noi_online.tscn";
		GetTree().ChangeSceneToFile("res://scene/man_hinh_cho.tscn");
		this.Hide();
	}
}
