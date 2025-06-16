using Godot;
using System;
using System.Linq;
using System.Reflection;

public partial class QuanLyCard : Node2D
{

	const int MASK_CARD = 1;
	const int MASK_ODEBAI = 2;
	public Card card_dang_bi_chon;
	// private CustomSignal _customsignal;
	public bool dang_bi_cam_vao = false;
	public CardNguoiChoi cardNguoiChoi;

	public ODeBai o_de_bai_chua_card;
	
	// private PackedScene card_scene;
	DataContext dataContext = new DataContext();
	public override void _Ready()
	{
		
		cardNguoiChoi = GetNode<CardNguoiChoi>("../card_nguoi_choi");
		
		GetNode<QuanLyDauVao>("../quan_ly_dau_vao").Connect(QuanLyDauVao.SignalName.chuot_trai_click_vao, new Callable(this,nameof(ChuotClickVao)));
		GetNode<QuanLyDauVao>("../quan_ly_dau_vao").Connect(QuanLyDauVao.SignalName.chuot_trai_tha_click, new Callable(this,nameof(ChuotThaClick)));
		
	}
	public void ChuotClickVao(){
		// GD.Print("signal chuot click vao");
	}
	public void ChuotThaClick(){
		// GD.Print("signal tha chuot");
		if (card_dang_bi_chon != null){
			card_dang_bi_chon.Scale = new Vector2(1f,1f);
			BatDauThaRa();
		}
	}


	public override void _Process(double delta)
	{
		if (card_dang_bi_chon != null)
		{
			card_dang_bi_chon.Position = GetGlobalMousePosition();
		}
	}

	// public override void _Input(InputEvent @event)
	// {
	// 	if (@event is InputEventMouseButton mouseButtonEvent)
	// 	{
	// 		if (mouseButtonEvent.IsPressed() && mouseButtonEvent.ButtonIndex == MouseButton.Left)
	// 		{
	// 			ClickCheck_Card();
	// 			Card card = ClickCheck_Card();
	// 			if (card != null)
	// 			{
	// 				BatDauCamVao(card);
	// 			}
	// 		}
	// 		else
	// 		{
	// 			if (card_dang_bi_chon != null){
	// 				BatDauThaRa();
	// 			}
	// 		}
	// 	}
	// }
	public void BatDauCamVao(Card card){
		GetNode<AudioStreamPlayer>("../sound/Card4").Play();

        if (Main.dang_huong_dan)
        {
            GetNode<Control>("../huongdan/buoc1").Visible = false;
            GetNode<Control>("../huongdan/buoc2").Visible = true;
        }
        
		card_dang_bi_chon = card;
		card.Scale = new Vector2(1f,1f);
		GetNode<RichTextLabel>("../thong_tin/RichTextLabel").Text = "[b]" + card.ten_card +"[/b]\n[color=white][i]"+  card.loai_card + "\n" + card.trong_so + "\n"+"[/i][/color]\n"  + card.mo_ta;
	}


	//online
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void GuiDuLieuCard(int id_card_gui,int so_oDeBai){
		PackedScene card_scene = GD.Load<PackedScene>("res://scene/card.tscn");
		Card card_gui = card_scene.Instantiate<Card>();
		card_gui.Name = "Card";
		card_gui.id_card = id_card_gui;
		card_gui.icon_card = GD.Load<CompressedTexture2D>("res://assets/cards/" + dataContext.tblCards.Find(id_card_gui).TenCard + ".svg");
		card_gui.trong_so = dataContext.tblCards.Find(card_gui.id_card).TrongSo;

		GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai" + so_oDeBai).card_trong_odebai = card_gui;
		GetNode<QuanLyNoiDeBai>("../quan_ly_noi_de_bai").AddChild(card_gui);
		GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai" + so_oDeBai).card_trong_odebai = card_gui;
		GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai" + so_oDeBai).co_card_trong_odebai = true;
		card_gui.Position = GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai" + so_oDeBai).Position;

		GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai" + so_oDeBai).card_trong_odebai.Scale = new Vector2(1.1f, 1.1f);
		GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai" + so_oDeBai).card_trong_odebai.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
	}
	//dong
	public void BatDauThaRa(){
		card_dang_bi_chon.Scale = new Vector2(1.1f,1.1f);
		ODeBai oDeBai_timthay =  ClickCheck_ODebai();

		if (oDeBai_timthay != null && oDeBai_timthay.co_card_trong_odebai == false){
			GetNode<AudioStreamPlayer>("../sound/Deck1").Play();
            
			cardNguoiChoi.XoaCard(card_dang_bi_chon);
			card_dang_bi_chon.o_de_bai_tim_thay = oDeBai_timthay;
			card_dang_bi_chon.ZIndex = -1;
			card_dang_bi_chon.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
			// card_dang_bi_chon.Position = oDeBai_timthay.Position;
			// oDeBai_timthay.co_card_trong_odebai = true;
			// oDeBai_timthay.card_trong_odebai = card_dang_bi_chon;
			Rpc("GuiDuLieuCard",card_dang_bi_chon.id_card,oDeBai_timthay.so_o);
			card_dang_bi_chon.QueueFree();

			//neu co it nhat 2 o da co bai
			int count = 0;
			if (GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai1").co_card_trong_odebai) count++;
			if (GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai2").co_card_trong_odebai) count++;
			if (GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai3").co_card_trong_odebai) count++;
			if (GetNode<ODeBai>("../quan_ly_noi_de_bai/o_de_bai4").co_card_trong_odebai) count++;

			if (count == 2) {
				GD.Print("Co it nhat 2 o da co bai");

				GetNode<QuanLyNoiDeBai>("../quan_ly_noi_de_bai").DoiLuot();
				// GetNode<QuanLyNoiDeBai>("../quan_ly_noi_de_bai")._Ready();
			}
			if (count == 4) {
				GD.Print("Co 4 o da co bai");
                if (Main.dang_huong_dan)
                {
                    GetNode<Control>("../huongdan/buoc1").Visible = false;
                    GetNode<Control>("../huongdan/buoc2").Visible = false;
                    GetNode<Control>("../huongdan/buoc3").Visible = true;
                }
				GetNode<QuanLyNoiDeBai>("../quan_ly_noi_de_bai").KetThucVong();
				// GetNode<QuanLyNoiDeBai>("../quan_ly_noi_de_bai")._Ready();
			}

		}

		else{
			cardNguoiChoi.ThemCard(card_dang_bi_chon,0.3f);
		}
		card_dang_bi_chon = null;
	}
	public ODeBai ClickCheck_ODebai()
	{
		PhysicsDirectSpaceState2D kiem_tra_khong_gian = GetWorld2D().DirectSpaceState;
		PhysicsPointQueryParameters2D loai_ket_qua = new PhysicsPointQueryParameters2D();
		loai_ket_qua.Position = GetGlobalMousePosition();
		loai_ket_qua.CollideWithAreas = true;
		loai_ket_qua.CollisionMask = MASK_ODEBAI;
		Godot.Collections.Array<Godot.Collections.Dictionary> ket_qua = kiem_tra_khong_gian.IntersectPoint(loai_ket_qua);
		if (ket_qua.Count > 0)
		{
			// GD.Print(((Area2D)ket_qua[0]["collider"]).GetParent());
			return (ODeBai)((Area2D)ket_qua[0]["collider"]).GetParent();
		}
		return null; // Return null explicitly when no collider is found
	}




	public Card ClickCheck_Card()
	{
		PhysicsDirectSpaceState2D kiem_tra_khong_gian = GetWorld2D().DirectSpaceState;
		PhysicsPointQueryParameters2D loai_ket_qua = new PhysicsPointQueryParameters2D();
		loai_ket_qua.Position = GetGlobalMousePosition();
		loai_ket_qua.CollideWithAreas = true;
		loai_ket_qua.CollisionMask = MASK_CARD;
		Godot.Collections.Array<Godot.Collections.Dictionary> ket_qua = kiem_tra_khong_gian.IntersectPoint(loai_ket_qua);
		if (ket_qua.Count > 0)
		{
			// GD.Print(((Area2D)ket_qua[0]["collider"]).GetParent());
			// return (Card)((Area2D)ket_qua[0]["collider"]).GetParent();
			return ClickLay_Card_CaoNhat(ket_qua);
		}
		return null; // Return null explicitly when no collider is found
	}
	public Card ClickLay_Card_CaoNhat(Godot.Collections.Array<Godot.Collections.Dictionary> ket_qua_moi){
		Card card_cao_nhat = (Card)((Area2D)ket_qua_moi[0]["collider"]).GetParent();
		int so_cao_nhat = card_cao_nhat.ZIndex;
		for (int i = 0; i < ket_qua_moi.Count ; i++){
			Card card_hien_tai = (Card)((Area2D)ket_qua_moi[i]["collider"]).GetParent();
			if (card_hien_tai.ZIndex > so_cao_nhat){
				card_cao_nhat =card_hien_tai;
				so_cao_nhat = card_hien_tai.ZIndex;
			}
		}
		return card_cao_nhat;
	}

	public void KetNoiCardSignal(Card card)
	{
		card.Connect(Card.SignalName.cam_vao, new Callable(this, nameof(HieuUng_cam_vao_Card)));
		card.Connect(Card.SignalName.cam_vao, new Callable(this, nameof(HieuUng_tha_ra_Card)));
		// card.Connect(Card.SignalName.cam_vao, Callable.From(() => HieuUng_tha_ra_Card(card)), (uint)GodotObject.ConnectFlags.OneShot);
	}
	//Hieu ung
	public void HieuUng_cam_vao_Card(Card card)
	{
		if (o_de_bai_chua_card == null){
			if (!dang_bi_cam_vao)
			{
				dang_bi_cam_vao = true;
				HieuUng_Card(card, true);
				// GD.Print("da cam");
			}
		}
	}
	public void HieuUng_tha_ra_Card(Card card)
	{
		HieuUng_Card(card, false);
		// GD.Print("da tha");
		Card card_cam_vao_moi = ClickCheck_Card();
		if (card_cam_vao_moi != null){
			HieuUng_Card(card_cam_vao_moi,true);
		}else{
			dang_bi_cam_vao = false;
		}
	}
	public void HieuUng_Card(Card card, bool dang_bi_cam_vao)
	{
		if (dang_bi_cam_vao == true)
		{
			card.ZIndex = 2;
		}
		else
		{
			// GD.Print("card name:" + card.Name);
			card.ZIndex = 1;
		}
	}
}


// player hand
// const int MAX_CARD = 8;
// private PackedScene CARD_SCENE;
// public float vi_tri_trung_tam_x;

// const float vi_tri_y = 600f;

// public int khoang_cach = 10;
// public float toc_do_card = 1f;

// public Godot.Collections.Array<Card> cards_nguoichoi = new Godot.Collections.Array<Card>();
// // Called when the node enters the scene tree for the first time.
// public override void _Ready()
// {	
// 	vi_tri_trung_tam_x = GetViewport().GetVisibleRect().Size.X;
// 	GD.Print("vi tri trung tam x: " + vi_tri_trung_tam_x.ToString());
// 	CARD_SCENE = GD.Load<PackedScene>("res://scene/card.tscn");
// 	for (int i = 1; i <= MAX_CARD; i++)
// 	{
// 		Card card = CARD_SCENE.Instantiate<Card>();
// 		card.Name = "Card" + i;
// 		AddChild(card);
// 		// card.Position = new Vector2(0, 0);
// 		card.test = "Bai so: " + i;
// 		ThemCard(card,toc_do_card);
// 	}

// }
// private void ThemCard(Card card,float toc_do)
// {
// 	cards_nguoichoi.Insert(0, card);
// 	CatNhatCardNguoiChoi(toc_do);
// }

// private void CatNhatCardNguoiChoi(float toc_do)
// {
// 	GD.Print(cards_nguoichoi.Count);
// 	for (int i = 0; i < cards_nguoichoi.Count; i++)
// 	{
// 		Vector2 vi_tri_moi = new Vector2(TinhToanViTri(i),vi_tri_y);
// 		Card card = cards_nguoichoi[i];
// 		card.vi_tri_bai_ban_dau = vi_tri_moi;
// 		AnimationThemCard(card,vi_tri_moi,toc_do);
// 	}
// }
// public float TinhToanViTri(int index){
// 	float do_dai = (cards_nguoichoi.Count - 1) * khoang_cach;
// 	float x_off = vi_tri_trung_tam_x + index * cards_nguoichoi.Count - do_dai / 2;
// 	return x_off;
// }

// public void AnimationThemCard(Card card,Vector2 vi_tri_moi, float toc_do){
// 	Tween tween = GetTree().CreateTween();
// 	tween.TweenProperty(card,"position",vi_tri_moi,toc_do);
// }
// public void LayCard(Card card){
// 	if (cards_nguoichoi.Contains(card)){
// 		cards_nguoichoi.Remove(card);
// 		CatNhatCardNguoiChoi(toc_do_card);
// 	}
// }
