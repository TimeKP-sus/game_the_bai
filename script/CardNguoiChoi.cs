using Godot;
using System;

public partial class CardNguoiChoi : Node2D
{
	
	// Called when the node enters the scene tree for the first time.
	public float vi_tri_trung_tam_x;

	const float VI_TRI_Y = 950f;

	public int khoang_cach = 110;
	// public float toc_do_card = 0.6f;
	public Godot.Collections.Array<Card> card_nguoi_choi_dang_co = [];

	
	public override void _Ready()
	{
		vi_tri_trung_tam_x = GetViewport().GetVisibleRect().Size.X / 2;
		// GD.Print(vi_tri_trung_tam_x.ToString());
		CatNhatCardNguoiChoi();
	}
	public void ThemCard(Card card,float toc_do)
	{
		if (!card_nguoi_choi_dang_co.Contains(card)){
			card_nguoi_choi_dang_co.Insert(0, card);
			CatNhatCardNguoiChoi();
		}
		else{
			GetNode<AudioStreamPlayer>("../sound/Card3").Play();
            if (Main.dang_huong_dan)
            {
                GetNode<Control>("../huongdan/buoc1").Visible = true;
                GetNode<Control>("../huongdan/buoc2").Visible = false;
            }
			AnimationThemCard(card, card.vi_tri_khoi_dau,toc_do);
		}
	}
	public void XoaCard(Card card){
		if (card_nguoi_choi_dang_co.Contains(card)){
			card_nguoi_choi_dang_co.Remove(card);
			CatNhatCardNguoiChoi();
		}
	}

	public void CatNhatCardNguoiChoi()
	{
		for (int i = 0; i < card_nguoi_choi_dang_co.Count; i++)
		{
			Vector2 vi_tri_moi = new Vector2(TinhToanViTri(i),VI_TRI_Y);
			// GD.Print("vi tri moi: " + vi_tri_moi.ToString());
			Card card = card_nguoi_choi_dang_co[i];
			card.vi_tri_khoi_dau = vi_tri_moi;
			AnimationThemCard(card,vi_tri_moi,0.3f);
		}
		// GD.Print("card count: "+ card_nguoi_choi_dang_co.Count);
	}
	public float TinhToanViTri(int index){
		// return 400 + (index);
		float do_dai = (card_nguoi_choi_dang_co.Count - 1) * khoang_cach;
		float x_off = vi_tri_trung_tam_x + index * khoang_cach - do_dai / 2;
		// GD.Print("x_off: " + x_off);
		return x_off;
	}

	public void AnimationThemCard(Card card,Vector2 vi_tri_moi,float toc_do){
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(card,"position",vi_tri_moi,toc_do);
	}
	// public void LayCard(Card card){
	// 	if (card_nguoi_choi_dang_co.Contains(card)){
	// 		card_nguoi_choi_dang_co.Remove(card);
	// 		CatNhatCardNguoiChoi(toc_do_card);
	// 	}
	// }
}
