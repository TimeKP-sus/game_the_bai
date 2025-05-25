using Godot;
using System;

public partial class QuanLyNoiDeBai : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private Godot.Collections.Array<int> i = new Godot.Collections.Array<int> { 1, 2, 3, 4 };
	public NguoiChoi nguoiChoi; //1
	public DoiThu doiThu; //2
	public static bool dang_danh_voi_bot = false;
	public static bool dang_danh_online = false;
	public static int den_luot_choi_cua_so;
	DataContext dataContext = new DataContext();
	private int[] id_card_bot = { 1, 2, 3, 4, 5, 7 };

	public int id_card_hieu_ung_ban_dau;
	private void TatMoODeBai(int o_so, bool tatmo)
	{
		if (tatmo == false)
		{
			GetNode<ODeBai>("o_de_bai" + i[o_so]).Modulate = new Color(1, 1, 1, 0.5f);
			GetNode<CollisionShape2D>("o_de_bai" + i[o_so] + "/Area2D/CollisionShape2D").Disabled = true;
		}
		else
		{
			GetNode<ODeBai>("o_de_bai" + i[o_so]).Modulate = new Color(1, 1, 1, 1f);
			GetNode<CollisionShape2D>("o_de_bai" + i[o_so] + "/Area2D/CollisionShape2D").Disabled = false;
		}
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ThongBaoThua()
	{
		GetNode<Label>("../thong_bao_luot").Text = "Bạn đã thua!";
	}
	public override void _Ready()
	{
		if (GetNode<DoiThu>("../quan_ly_nguoi_choi/doi_thu").mau <= 0)
		{
			GetNode<Label>("../thong_bao_luot").Text = "Bạn đã thắng!";
			GetNode<Button>("../quan_ly_dau_vao/reload").Visible = true;
			if (dang_danh_online)
			{
				Rpc("ThongBaoThua");
				return;
			}
		}
		if (GetNode<NguoiChoi>("../quan_ly_nguoi_choi/nguoi_choi").mau <= 0)
		{

			GetNode<Label>("../thong_bao_luot").Text = "Bạn đã thua cuộc...";
			GetNode<Button>("../quan_ly_dau_vao/reload").Visible = true;
			return;
		}
		GD.Print("thong tin mang_ready: " + i);
		Random random_luot_choi = new Random();
		den_luot_choi_cua_so = random_luot_choi.Next(1, 3);


		nguoiChoi = GetNode<NguoiChoi>("../quan_ly_nguoi_choi/nguoi_choi");
		doiThu = GetNode<DoiThu>("../quan_ly_nguoi_choi/doi_thu");
		// Godot.Collections.Array nguoi = new Godot.Collections.Array { nguoiChoi, doiThu };

		TatMoODeBai(0, false);

		TatMoODeBai(1, false);

		TatMoODeBai(2, false);

		TatMoODeBai(3, false);

		//bot
		if (dang_danh_voi_bot)
		{
			i.Shuffle();
			GetNode<ODeBai>("o_de_bai" + i[0]).luot_cua_nguoi_choi_so = 1;
			GetNode<ODeBai>("o_de_bai" + i[1]).luot_cua_nguoi_choi_so = 1;

			GetNode<ODeBai>("o_de_bai" + i[2]).luot_cua_nguoi_choi_so = 2;
			GetNode<ODeBai>("o_de_bai" + i[3]).luot_cua_nguoi_choi_so = 2;
			PackedScene card_scene = GD.Load<PackedScene>("res://scene/card.tscn");
			Random random = new Random();

			Card card_moi1 = card_scene.Instantiate<Card>();
			card_moi1.id_card = id_card_bot[random.Next(id_card_bot.Length)];
			card_moi1.icon_card = GD.Load<CompressedTexture2D>("res://assets/cards/" + dataContext.tblCards.Find(card_moi1.id_card).TenCard + ".svg");
			card_moi1.trong_so = dataContext.tblCards.Find(card_moi1.id_card).TrongSo;
			card_moi1.Position = GetNode<ODeBai>("o_de_bai" + i[2]).Position;
			AddChild(card_moi1);
			GetNode<ODeBai>("o_de_bai" + i[2]).card_trong_odebai = card_moi1;
			GetNode<ODeBai>("o_de_bai" + i[2]).co_card_trong_odebai = true;
			GetNode<ODeBai>("o_de_bai" + i[2]).card_trong_odebai.Scale = new Vector2(1.1f, 1.1f);
			GetNode<ODeBai>("o_de_bai" + i[2]).card_trong_odebai.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;

			Card card_moi2 = card_scene.Instantiate<Card>();
			card_moi2.id_card = id_card_bot[random.Next(id_card_bot.Length)];
			card_moi2.icon_card = GD.Load<CompressedTexture2D>("res://assets/cards/" + dataContext.tblCards.Find(card_moi2.id_card).TenCard + ".svg");
			card_moi2.trong_so = dataContext.tblCards.Find(card_moi2.id_card).TrongSo;
			card_moi2.Position = GetNode<ODeBai>("o_de_bai" + i[3]).Position;
			AddChild(card_moi2);
			GetNode<ODeBai>("o_de_bai" + i[3]).card_trong_odebai = card_moi2;
			GetNode<ODeBai>("o_de_bai" + i[3]).co_card_trong_odebai = true;
			GetNode<ODeBai>("o_de_bai" + i[3]).card_trong_odebai.Scale = new Vector2(1.1f, 1.1f);
			GetNode<ODeBai>("o_de_bai" + i[3]).card_trong_odebai.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;


			TatMoODeBai(2, false);
			TatMoODeBai(3, false);

			TatMoODeBai(0, true);
			TatMoODeBai(1, true);
		} //het bot
		else
		{
			if (QuanLyNguoiThamGiaOnline.luot_choi == 1)
			{
				i.Shuffle();

				Rpc("GuiThongTinOBai", i);
				GetNode<ODeBai>("o_de_bai" + i[0]).luot_cua_nguoi_choi_so = 1;
				GetNode<ODeBai>("o_de_bai" + i[1]).luot_cua_nguoi_choi_so = 1;

				GetNode<ODeBai>("o_de_bai" + i[2]).luot_cua_nguoi_choi_so = 2;
				GetNode<ODeBai>("o_de_bai" + i[3]).luot_cua_nguoi_choi_so = 2;


				TatMoODeBai(0, true);
				TatMoODeBai(1, true);
				GetNode<Label>("../thong_bao_luot").Text = "Lượt đánh của bạn";
				GD.Print("thong tin mang_1_ready: " + i);
			}
			if (QuanLyNguoiThamGiaOnline.luot_choi == 2)
			{
				GetNode<Label>("../thong_bao_luot").Text = "Lượt đánh của đối thủ";
				GD.Print("thong tin mang_2_ready: " + i);
			}
		}

	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void GuiThongTinOBai(Godot.Collections.Array<int> mang)
	{
		this.i = mang;
		GetNode<ODeBai>("o_de_bai" + i[0]).luot_cua_nguoi_choi_so = 2;
		GetNode<ODeBai>("o_de_bai" + i[1]).luot_cua_nguoi_choi_so = 2;

		GetNode<ODeBai>("o_de_bai" + i[2]).luot_cua_nguoi_choi_so = 1;
		GetNode<ODeBai>("o_de_bai" + i[3]).luot_cua_nguoi_choi_so = 1;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void GuiTinHieuDoiLuot()
	{
		DoiLuot();
	}
	public void DoiLuot()
	{
		if (dang_danh_online)
		{
			if (QuanLyNguoiThamGiaOnline.luot_choi == 1)
			{
				GetNode<Label>("../thong_bao_luot").Text = "Lượt đánh của đối thủ";
				Rpc("GuiTinHieuDoiLuot");
			}
			if (QuanLyNguoiThamGiaOnline.luot_choi == 2)
			{
				GD.Print("thong tin mang_2: " + i);
				TatMoODeBai(2, true);
				TatMoODeBai(3, true);
				GetNode<Label>("../thong_bao_luot").Text = "Lượt đánh của bạn";
			}
		}
		if (dang_danh_voi_bot)
		{
			TatMoODeBai(2, true);
			TatMoODeBai(3, true);

			TatMoODeBai(0, false);
			TatMoODeBai(1, false);

			GetNode<Label>("thong_bao_luot").Text = "Luợt của đối thủ";
		}
	}

	int so;
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void GuiTinHieuKetThucVong()
	{
		// i = new Godot.Collections.Array<int> { 1, 2, 3, 4 };
		so = 1;
		GetNode<Timer>("Timer").Start();
	}
	public void KetThucVong()
	{
		Rpc("GuiTinHieuKetThucVong");
		// i = new Godot.Collections.Array<int> { 1, 2, 3, 4 };
		so = 1;
		GetNode<Timer>("Timer").Start();
	}
	public void KiemTraHieuUng()
	{
		//tot
		//7
		if (nguoiChoi.id_hieu_ung_tot == 7 && nguoiChoi.so_hieu_ung_tot > 0)
		{
			if (nguoiChoi.mau >= nguoiChoi.mau_toi_da)
			{
				nguoiChoi.mau += 0;
			}
			else if (nguoiChoi.mau + 2 < nguoiChoi.mau_toi_da)
			{
				nguoiChoi.mau += 2;
				nguoiChoi.GetNode<ProgressBar>("mau_bar").Value = nguoiChoi.mau;
				nguoiChoi.GetNode<Label>("mau_bar/mau").Text = nguoiChoi.mau.ToString();
			}
			else
			{
				nguoiChoi.mau = nguoiChoi.mau_toi_da;
				nguoiChoi.GetNode<ProgressBar>("mau_bar").Value = nguoiChoi.mau;
				nguoiChoi.GetNode<Label>("mau_bar/mau").Text = nguoiChoi.mau.ToString();
			}
			nguoiChoi.so_hieu_ung_tot -=1;
			nguoiChoi.GetNode<Label>("hieu_ung_tot/trong_so").Text = nguoiChoi.so_hieu_ung_tot.ToString();
		}
		if (doiThu.id_hieu_ung_tot == 7 && doiThu.so_hieu_ung_tot > 0)
		{
			if (doiThu.mau >= doiThu.mau_toi_da)
			{
				doiThu.mau += 0;
			}
			else if (doiThu.mau + 2 < doiThu.mau_toi_da)
			{
				doiThu.mau += 2;
				doiThu.GetNode<ProgressBar>("mau_bar").Value = doiThu.mau;
				doiThu.GetNode<Label>("mau_bar/mau").Text = doiThu.mau.ToString();
			}
			else
			{
				doiThu.mau = doiThu.mau_toi_da;
				doiThu.GetNode<ProgressBar>("mau_bar").Value = doiThu.mau;
				doiThu.GetNode<Label>("mau_bar/mau").Text = doiThu.mau.ToString();
			}
			doiThu.so_hieu_ung_tot -=1;
			doiThu.GetNode<Label>("hieu_ung_tot/trong_so").Text = doiThu.so_hieu_ung_tot.ToString();
		}
		//8




		//xau

		//kiem tra tong the
		if (nguoiChoi.so_hieu_ung_tot == 0 && nguoiChoi.id_hieu_ung_tot > 0)
		{
			nguoiChoi.id_hieu_ung_tot = 0;
			nguoiChoi.DoiIconTrongSoHieuUng("tot", 0);
		}
		if (doiThu.so_hieu_ung_tot == 0 && doiThu.id_hieu_ung_tot > 0)
		{
			doiThu.id_hieu_ung_tot = 0;
			doiThu.DoiIconTrongSoHieuUng("tot", 0);
		}
		if (nguoiChoi.so_hieu_ung_xau == 0 && nguoiChoi.id_hieu_ung_xau > 0)
		{
			nguoiChoi.id_hieu_ung_xau = 0;
			nguoiChoi.DoiIconTrongSoHieuUng("xau", 0);
		}
		if (doiThu.so_hieu_ung_xau == 0 && doiThu.id_hieu_ung_xau > 0)
		{
			doiThu.id_hieu_ung_xau = 0;
			doiThu.DoiIconTrongSoHieuUng("xau", 0);
		}
		//
	}
	public void _on_timer_timeout()
	{
		if (so > 4)
		{
			GetNode<Timer>("Timer").Stop();
			so = 1;
			GetNode<QuanLyDauVao>("../quan_ly_dau_vao").so_vong += 1;
			GetNode<Label>("../vong_dau/Label").Text = "" + GetNode<QuanLyDauVao>("../quan_ly_dau_vao").so_vong;

			GetNode<QuanLyDeck>("../quan_ly_deck").da_lay_card_khoi_deck = false;
			GetNode<QuanLyDeck>("../quan_ly_deck").LayCard();
			GetNode<QuanLyDeck>("../quan_ly_deck").da_lay_card_khoi_deck = false;
			GetNode<QuanLyDeck>("../quan_ly_deck").LayCard();
			GetNode<QuanLyDeck>("../quan_ly_deck").da_lay_card_khoi_deck = false;
			KiemTraHieuUng();
			_Ready();
			return;
		}
		if (GetNode<ODeBai>("o_de_bai" + so).co_card_trong_odebai == false)
		{
			so += 1;
		}
		else
		{
			GD.Print("da loai o" + so);
			GetNode<AudioStreamPlayer>("../sound/Card2").Play();
			// Tween tween = CreateTween();
			// tween.Parallel().TweenProperty(GetNode<ODeBai>("o_de_bai" + ).card_trong_odebai,"modulate:a",0,2.0f);
			Card card = GetNode<ODeBai>("o_de_bai" + so).card_trong_odebai;

			if (GetNode<ODeBai>("o_de_bai" + so).luot_cua_nguoi_choi_so == 1)
			{
				if (card.id_card == 1)
				{
					if (doiThu.khien <= 4)
					{
						doiThu.mau = doiThu.mau - 4 + doiThu.khien;
						doiThu.GetNode<ProgressBar>("mau_bar").Value = doiThu.mau;
						doiThu.GetNode<Label>("mau_bar/mau").Text = doiThu.mau.ToString();
						doiThu.khien = 0;
						doiThu.GetNode<Label>("khien_bar/khien").Text = "+0";
					}
					else
					{
						doiThu.khien -= 4;
						doiThu.GetNode<Label>("khien_bar/khien").Text = "+" + doiThu.khien;
					}
				}
				if (card.id_card == 2)
				{
					doiThu.mau = doiThu.mau - 3;
					doiThu.GetNode<ProgressBar>("mau_bar").Value = doiThu.mau;
					doiThu.GetNode<Label>("mau_bar/mau").Text = doiThu.mau.ToString();
				}
				if (card.id_card == 3)
				{
					nguoiChoi.khien += 4;
					nguoiChoi.GetNode<Label>("khien_bar/khien").Text = "+" + nguoiChoi.khien;
				}
				if (card.id_card == 4)
				{
					if (nguoiChoi.mau >= nguoiChoi.mau_toi_da)
					{
						nguoiChoi.mau += 0;
					}
					else if (nguoiChoi.mau + 4 < nguoiChoi.mau_toi_da)
					{
						nguoiChoi.mau += 4;
						nguoiChoi.GetNode<ProgressBar>("mau_bar").Value = nguoiChoi.mau;
						nguoiChoi.GetNode<Label>("mau_bar/mau").Text = nguoiChoi.mau.ToString();
					}
					else
					{
						nguoiChoi.mau = nguoiChoi.mau_toi_da;
						nguoiChoi.GetNode<ProgressBar>("mau_bar").Value = nguoiChoi.mau;
						nguoiChoi.GetNode<Label>("mau_bar/mau").Text = nguoiChoi.mau.ToString();
					}
				}
				if (card.id_card == 5)
				{
					doiThu.khien = 0;
					doiThu.GetNode<Label>("khien_bar/khien").Text = "+" + doiThu.khien;
				}
				if (card.id_card == 6)
				{
					GetNode<QuanLyDeck>("../quan_ly_deck").da_lay_card_khoi_deck = false;
					GetNode<QuanLyDeck>("../quan_ly_deck").LayCard();
					GetNode<QuanLyDeck>("../quan_ly_deck").da_lay_card_khoi_deck = false;
					GetNode<QuanLyDeck>("../quan_ly_deck").LayCard();
					GetNode<QuanLyDeck>("../quan_ly_deck").da_lay_card_khoi_deck = false;
				}
				if (card.id_card == 7)
				{
					if (nguoiChoi.id_hieu_ung_tot == 7 && nguoiChoi.so_hieu_ung_tot > 0)
					{
						nguoiChoi.so_hieu_ung_tot += 2;
						nguoiChoi.GetNode<Label>("hieu_ung_tot/trong_so").Text = nguoiChoi.so_hieu_ung_tot.ToString();
					}
					else
					{
						nguoiChoi.DoiIconTrongSoHieuUng("tot", 7);
					}
				}

			}
			else
			{
				if (card.id_card == 1)
				{
					if (nguoiChoi.khien <= 4)
					{
						nguoiChoi.mau = nguoiChoi.mau - 4 + nguoiChoi.khien;
						nguoiChoi.GetNode<ProgressBar>("mau_bar").Value = nguoiChoi.mau;
						nguoiChoi.GetNode<Label>("mau_bar/mau").Text = nguoiChoi.mau.ToString();
						nguoiChoi.khien = 0;
						nguoiChoi.GetNode<Label>("khien_bar/khien").Text = "+0";
					}
					else
					{
						nguoiChoi.khien -= 4;
						nguoiChoi.GetNode<Label>("khien_bar/khien").Text = "+" + nguoiChoi.khien;
					}
				}
				if (card.id_card == 2)
				{
					nguoiChoi.mau = nguoiChoi.mau - 3;
					nguoiChoi.GetNode<ProgressBar>("mau_bar").Value = nguoiChoi.mau;
					nguoiChoi.GetNode<Label>("mau_bar/mau").Text = nguoiChoi.mau.ToString();
				}
				if (card.id_card == 3)
				{
					doiThu.khien += 4;
					doiThu.GetNode<Label>("khien_bar/khien").Text = "+" + doiThu.khien;
				}
				if (card.id_card == 4)
				{
					if (doiThu.mau >= doiThu.mau_toi_da)
					{
						doiThu.mau += 0;
					}
					else if (doiThu.mau + 4 < doiThu.mau_toi_da)
					{
						doiThu.mau += 4;
						doiThu.GetNode<ProgressBar>("mau_bar").Value = doiThu.mau;
						doiThu.GetNode<Label>("mau_bar/mau").Text = doiThu.mau.ToString();
					}
					else
					{
						doiThu.mau = doiThu.mau_toi_da;
						doiThu.GetNode<ProgressBar>("mau_bar").Value = doiThu.mau;
						doiThu.GetNode<Label>("mau_bar/mau").Text = doiThu.mau.ToString();
					}
				}
				if (card.id_card == 5)
				{
					nguoiChoi.khien = 0;
					nguoiChoi.GetNode<Label>("khien_bar/khien").Text = "+" + nguoiChoi.khien;
				}
				if (card.id_card == 7)
				{
					if (doiThu.id_hieu_ung_tot == 7 && doiThu.so_hieu_ung_tot > 0)
					{
						doiThu.so_hieu_ung_tot += 2;
						doiThu.GetNode<Label>("hieu_ung_tot/trong_so").Text = doiThu.so_hieu_ung_tot.ToString();
					}
					else
					{
						doiThu.DoiIconTrongSoHieuUng("tot", 7);
					}
				}
			}




			GetNode<ODeBai>("o_de_bai" + so).card_trong_odebai.XoaBai();
			// GetNode<ODeBai>("o_de_bai" + so).card_trong_odebai._Ready();
			GetNode<ODeBai>("o_de_bai" + so).card_trong_odebai = null;
			GetNode<ODeBai>("o_de_bai" + so).co_card_trong_odebai = false;

			//
			so += 1;
		}
	}
	private void HieuUng_mau()
	{

	}

}
