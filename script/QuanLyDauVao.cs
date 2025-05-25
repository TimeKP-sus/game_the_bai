using Godot;
using System;

public partial class QuanLyDauVao : Node2D
{
	[Signal]
	public delegate void chuot_trai_click_vaoEventHandler(Card card);
	[Signal]
	public delegate void chuot_trai_tha_clickEventHandler(Card card);

	public int so_vong = 1;


	const int MASK_CARD = 1;
	const int MASK_DECK = 4;
	public QuanLyDeck quanLyDeck;
	public QuanLyCard quanLyCard;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		quanLyCard = GetNode<QuanLyCard>("../quan_ly_card");
		quanLyDeck = GetNode<QuanLyDeck>("../quan_ly_deck");
		
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.IsPressed() && mouseButtonEvent.ButtonIndex == MouseButton.Left)
			{
				ClickCheck_Card();
				EmitSignal(SignalName.chuot_trai_click_vao);
				
			}
			else
			{
				EmitSignal(SignalName.chuot_trai_tha_click);
			}
		}
	}
	public Card ClickCheck_Card()
	{
		PhysicsDirectSpaceState2D kiem_tra_khong_gian = GetWorld2D().DirectSpaceState;
		PhysicsPointQueryParameters2D loai_ket_qua = new PhysicsPointQueryParameters2D();
		loai_ket_qua.Position = GetGlobalMousePosition();
		loai_ket_qua.CollideWithAreas = true;
		Godot.Collections.Array<Godot.Collections.Dictionary> ket_qua = kiem_tra_khong_gian.IntersectPoint(loai_ket_qua);
		if (ket_qua.Count > 0)
		{
			// GD.Print(((Area2D)ket_qua[0]["collider"]).GetParent());
			// // return (Card)((Area2D)ket_qua[0]["collider"]).GetParent();
			// return ClickLay_Card_CaoNhat(ket_qua);
			var ket_qua_mask = ((Area2D)ket_qua[0]["collider"]).CollisionMask;
			if (ket_qua_mask == MASK_CARD){
				Card card_tim_thay = (Card)((Area2D)ket_qua[0]["collider"]).GetParent();
				if (card_tim_thay != null){
					quanLyCard.BatDauCamVao(card_tim_thay);
				}
			}
			else if(ket_qua_mask == MASK_DECK){
				quanLyDeck.LayCard();
			}
			
		}
		return null; // Return null explicitly when no collider is found
	}

	public void _on_bat_dau_pressed(){
		GetNode<QuanLyNoiDeBai>("../quan_ly_noi_de_bai").DoiLuot();
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void GuiYeuCauReload(){
		GetTree().ReloadCurrentScene();
	}

	public void _on_reload_pressed(){
		Rpc("GuiYeuCauReload");
		GetTree().ReloadCurrentScene();
	}

	
}
