using Godot;
using System;

public partial class Card_menu : Panel
{
	public int id_card = 1;
	public int trong_so = 12;
	public string loai_card;
	public string ten_card;
	public string mo_ta;
	public CompressedTexture2D icon_card;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		GetNode<Label>("trong_so").Text = trong_so.ToString();
		GetNode<Sprite2D>("icon_card").Texture = icon_card;
	}
	public void _on_area_2d_mouse_entered(){
		GD.Print("da vao");
		GetNode<Sprite2D>("icon_card").Scale = new Vector2(1.1f, 1.1f);
	}
	public void _on_area_2d_mouse_exited(){
		GD.Print("da ra");
		GetNode<Sprite2D>("icon_card").Scale = new Vector2(1f, 1f);
	}
}
