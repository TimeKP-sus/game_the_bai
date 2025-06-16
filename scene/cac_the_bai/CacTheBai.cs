using Godot;
using System;
using System.Linq;

public partial class CacTheBai : Control
{
	private GridContainer gridContainer;
	private PackedScene cardScene;
	DataContext dataContext = new DataContext();

	private Card_menu card_dang_chon;

	public override void _Ready()
	{
		cardScene = GD.Load<PackedScene>("res://scene/cac_the_bai/Card.tscn");
		gridContainer = GetNode<GridContainer>("NinePatchRect/ScrollContainer/GridContainer");

		for (int i = 1; i <= dataContext.tblCards.Count(); i++)
		{
			Card_menu card = cardScene.Instantiate<Card_menu>();
			card.id_card = i;
			card.trong_so = dataContext.tblCards.FirstOrDefault(x => x.Id == i).TrongSo;

			string loai_card_moi = dataContext.tblCards.FirstOrDefault(x => x.Id == i).LoaiCard;

			if (loai_card_moi == "1")
			{
				card.loai_card = "Tấn công";
			}
			if (loai_card_moi == "2")
			{
				card.loai_card = "Phòng Thủ";
			}
			if (loai_card_moi == "3")
			{
				card.loai_card = "Hiệu Ứng Tốt";
			}
			if (loai_card_moi == "4")
			{
				card.loai_card = "Hiệu Ứng Xấu";
			}
			if (loai_card_moi == "5")
			{
				card.loai_card = "Hiệu Ứng bàn đấu";
			}

			// card.loai_card = dataContext.tblCards.FirstOrDefault(x => x.Id == i).LoaiCard;

			card.ten_card = dataContext.tblCards.FirstOrDefault(x => x.Id == i).TenCard;
			card.mo_ta = dataContext.tblCards.FirstOrDefault(x => x.Id == i).MoTa;
			card.icon_card = GD.Load<CompressedTexture2D>("res://assets/cards/" + dataContext.tblCards.FirstOrDefault(x => x.Id == i).TenCard + ".svg");
			gridContainer.AddChild(card);
		}
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.IsPressed() && mouseButtonEvent.ButtonIndex == MouseButton.Left)
			{
				Card_menu card = ClickCheck_Card();
				if (card != null)
				{
					if (card_dang_chon != null && card_dang_chon != card)
					{
						card_dang_chon.Scale = new Vector2(1f, 1f);
					}
					card_dang_chon = card;
					GD.Print(card.ten_card);
					card.Scale = new Vector2(1.1f, 1.1f);

					GetNode<RichTextLabel>("thong_tin/RichTextLabel").Text = "[b]" + card.ten_card + "[/b]\n[color=white][i]" + card.loai_card + "\n" + card.trong_so + "\n" + "[/i][/color]\n" + card.mo_ta;
				}
			}
			else
			{

			}
		}
	}
	public Card_menu ClickCheck_Card()
	{
		PhysicsDirectSpaceState2D kiem_tra_khong_gian = GetWorld2D().DirectSpaceState;
		PhysicsPointQueryParameters2D loai_ket_qua = new PhysicsPointQueryParameters2D();
		loai_ket_qua.Position = GetGlobalMousePosition();
		loai_ket_qua.CollideWithAreas = true;
		loai_ket_qua.CollisionMask = 1;
		Godot.Collections.Array<Godot.Collections.Dictionary> ket_qua = kiem_tra_khong_gian.IntersectPoint(loai_ket_qua);
		if (ket_qua.Count > 0)
		{
			// GD.Print(((Area2D)ket_qua[0]["collider"]).GetParent());
			return (Card_menu)((Area2D)ket_qua[0]["collider"]).GetParent();
			// return ClickLay_Card_CaoNhat(ket_qua);
		}
		return null; // Return null explicitly when no collider is found
	}

	public void _on_tim_kiem_text_changed(string new_text)
	{
		foreach (Card_menu card in gridContainer.GetChildren())
		{
			if (card.ten_card.ToLower().Contains(new_text.ToLower()))
			{
				card.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = false;
				card.Visible = true;
			}
			else
			{
				card.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
				card.Visible = false;
			}
		}
	}

	public void _on_quay_ve_button_down(){
		GD.Print("Quay ve");
		NodeCanhLoad.node_moi = "res://scene/menu/menu.tscn";
		NodeCanhLoad.node_truoc = this.Name;
		GetTree().ChangeSceneToFile("res://scene/node_canh_load.tscn");
		// QueueFree(); // Ensure this is called after the scene change
	}

	
}
