using Godot;
using System;

public partial class QuanLyDeck : Node2D
{
	const int CARD_MAX = 8;
	const int SO_CARD_KHOI_DAU = 5;
	const int DECK_CARD_MAX = 100;
	public static int so_the = 7; // số loại thẻ trong bộ bài
	public Godot.Collections.Array<int> card_trong_deck = new Godot.Collections.Array<int> { };
	public RichTextLabel richTextLabel;
	DataContext dataContext = new DataContext();

	private CompressedTexture2D sword_hilt;
	private CompressedTexture2D shield;
	private CompressedTexture2D bowie_knife;
	private PackedScene card_scene;
	private Card card_moi;
	private tblCard card_lay_ra;


	public bool da_lay_card_khoi_deck = false;
	public override void _Ready()
	{
		card_scene = GD.Load<PackedScene>("res://scene/card.tscn");
		GD.Load<CompressedTexture2D>("res://assets/cards/sword_hilt.svg");
		GD.Load<CompressedTexture2D>("res://assets/cards/shield.svg");
		GD.Load<CompressedTexture2D>("res://assets/cards/bowie_knife.svg");
        GD.Load<CompressedTexture2D>("res://assets/cards/heart_plus.svg");
        GD.Load<CompressedTexture2D>("res://assets/cards/cracked_shield.svg");
        GD.Load<CompressedTexture2D>("res://assets/cards/card_pickup.svg");
        GD.Load<CompressedTexture2D>("res://assets/cards/heart_wings.svg");
        GD.Load<CompressedTexture2D>("res://assets/cards/curvy_knife.svg");

		richTextLabel = GetNode<RichTextLabel>("RichTextLabel");
        // Card card = new Card(1,10,GD.Load<CompressedTexture2D>("res://assets/cards/sword-hilt.svg"));
        
        // Cân bằng lại tỉ lệ xuất hiện các loại thẻ trong bộ bài

        // Số lượng mỗi loại thẻ muốn xuất hiện trong bộ bài 
        int[] soLuongMoiLoai = { 15, 10, 6, 7, 8, 6, 8 ,8, 8, 10}; 


        for (int i = 0; i < soLuongMoiLoai.Length; i++)
        {
            for (int j = 0; j < soLuongMoiLoai[i]; j++)
            {
                card_trong_deck.Add(i + 1); //từ id 1
            }
        }

        // Nếu tổng số thẻ nhỏ hơn thêm ngẫu nhiên cho đủ
        Random random = new Random();
        while (card_trong_deck.Count < DECK_CARD_MAX)
        {
            card_trong_deck.Add(random.Next(1, so_the + 1));
        }

        // Nếu tổng số thẻ lớn hơn loại bỏ ngẫu nhiên cho đủ
        while (card_trong_deck.Count > DECK_CARD_MAX)
        {
            int xoa = random.Next(0, card_trong_deck.Count);
            card_trong_deck.RemoveAt(xoa);
        }

        card_trong_deck.Shuffle();
		// for (int i = 0; i<3;i++){
		// 	card_trong_deck.Add(new Card("card" + 1.ToString()));
		// }
		for (int i = 0; i < SO_CARD_KHOI_DAU; i++)
		{
			da_lay_card_khoi_deck = false;
			LayCard();
		}
		da_lay_card_khoi_deck = false;




		richTextLabel.Text = card_trong_deck.Count.ToString();

		// GD.Print("mask = " + GetNode<Area2D>("Area2D").CollisionMask);
	}

	public void LayCard()
	{
		if (!da_lay_card_khoi_deck)
		{
			if (GetNode<CardNguoiChoi>("../card_nguoi_choi").card_nguoi_choi_dang_co.Count < CARD_MAX)
			{
				GetNode<AudioStreamPlayer>("../sound/Deck2").Play();
				GD.Print("Lay card");
				card_lay_ra = dataContext.tblCards.Find(card_trong_deck[0]);
				card_trong_deck.Remove(card_trong_deck[0]);
				richTextLabel.Text = card_trong_deck.Count.ToString();

				if (card_trong_deck.Count == 0)
				{

					GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Disabled = true;
					GetNode<Sprite2D>("Deck").Visible = false;
					richTextLabel.Visible = false;
				}
				card_moi = card_scene.Instantiate<Card>();
				card_moi.id_card = card_lay_ra.Id;
				card_moi.trong_so = card_lay_ra.TrongSo;

				// 1: Tấn Công, 2: Phòng Thủ, 3: Hiệu Ứng Tốt, 4: Hiệu Ứng Xấu, 5:Hiệu ứng bàn đấu
				if (card_lay_ra.LoaiCard == "1")
				{
					card_moi.loai_card = "Tấn công";
				}
				if (card_lay_ra.LoaiCard == "2")
				{
					card_moi.loai_card = "Phòng Thủ";
				}
				if (card_lay_ra.LoaiCard == "3")
				{
					card_moi.loai_card = "Hiệu Ứng Tốt";
				}
				if (card_lay_ra.LoaiCard == "4")
				{
					card_moi.loai_card = "Hiệu Ứng Xấu";
				}
				if (card_lay_ra.LoaiCard == "5")
				{
					card_moi.loai_card = "Hiệu Ứng bàn đấu";
				}

				card_moi.ten_card = card_lay_ra.TenCard;
				card_moi.mo_ta = card_lay_ra.MoTa;
				card_moi.icon_card = GD.Load<CompressedTexture2D>("res://assets/cards/" + card_lay_ra.TenCard + ".svg");
				GetNode("../quan_ly_card").AddChild(card_moi);
				card_moi.Name = "Card";
				GetNode<CardNguoiChoi>("../card_nguoi_choi").ThemCard(card_moi, 0.5f);
				da_lay_card_khoi_deck = true;
			}
			else
			{
				GD.Print("Da dat gioi han card");
				return;
			}

		}
		else {
			GD.Print("Khong the rut duoc nua");
		}
	}
}
