using Godot;
using System;

public partial class ManHinhCho : Control
{
	// Called when the node enters the scene tree for the first time.
	public static string canh_moi = "res://scene/menu/menu.tscn";

	public PackedScene canh_load;
	public override void _Ready()
	{
		canh_load = GD.Load<PackedScene>(canh_moi);

		Node node = canh_load.Instantiate();


		GetTree().CreateTimer(3.0).Timeout += () =>
		{
			GetTree().Root.AddChild(node);
			this.Hide();
		};

	}
	public void LoadNode()
	{

	}
}
