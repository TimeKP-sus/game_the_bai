using Godot;
using System;

public partial class ODeBai : Node2D
{
	public bool co_card_trong_odebai = false;
	public Card card_trong_odebai;
	public int luot_cua_nguoi_choi_so;
	[Export]
	public int so_o;

	
	public override void _Ready() {
		// GetNode<Sprite2D>("CardSlot").SelfModulate = new Color(255f, 255f, 255f, 1.0f);
		GetNode<Label>("Label").Text = so_o.ToString();
	}

}


	
	// // public string ten_bai;

	// // public bool con_slot = true;


	// // public override void _Ready()
	// // {

	// // }
	// // public void _on_area_2d_area_entered(Area2D area)
	// // {
	// // 	if (area.GetParent() is Card card)
	// // 	{
	// // 		ten_bai = card.Name;
	// // 		// GD.Print("Card: " + Card.Name + " " + card.dang_duoc_chon + card.Position);
	// // 		if (card.dang_duoc_chon == true){
	// // 			GD.Print("da vao");
	// // 			GD.Print("Card: " + card.Name + card.dang_duoc_chon + card.Position);
	// // 			card.o_trong_noi_de_bai = true;
	// // 			GD.Print("Card: " + card.o_trong_noi_de_bai);
	// // 		}
	// // 	}
	// // }
	// // public void _on_area_2d_area_exited(Area2D area)
	// // {
	// // 	if (area.GetParent() is Card card)
	// // 	{
	// // 		if (card.dang_duoc_chon == true){
	// // 			GD.Print("da thoat");
	// // 			card.o_trong_noi_de_bai = false;
	// // 			ten_bai = "";
	// // 			card.Position = Position;
	// // 		}
	// // 	}
	// }


	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {

	// }