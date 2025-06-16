using Godot;
using System;

public partial class CanhLoad2 : Node2D
{
	public static string node_moi = "res://scene/menu/menu.tscn";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadCanhCho.scene_hien_tai.LoadThanhPhan(node_moi);
	}

}
