using Godot;
using System;
using System.Collections.Generic;

public partial class QuanLyCSDL : Node2D
{
	public Godot.Collections.Array<string> ten_card = new Godot.Collections.Array<string>{
		"bowie_knife","sword_hilt","shield","heart_plus","cracked_shield","card_pickup","heart_wings"
	};
	public Godot.Collections.Array<string> loai_card = new Godot.Collections.Array<string>{
		"1","1","2","2","4","3","3"
	};
	public Godot.Collections.Array<int> trong_so = new Godot.Collections.Array<int>{
		4,3,4,4,0,0,2
	};
	public Godot.Collections.Array<string> mo_ta = new Godot.Collections.Array<string>{
		"Tấn công vào kẻ địch gây sát thương","Tấn công vào kẻ địch gây sát thương xuyên giáp","Tạo giáp chắn một lượng sát thương","Hồi phục một lượng máu","Phá hủy giáp của kẻ địch ngay lập tức","Bốc thêm hai lá bài ngẫu nhiên trong bộ bài","Hồi phục 2 máu mỗi vòng đến hết hiệu ứng"
	};

	DataContext dataContext = new DataContext();
	public override void _Ready()
	{
		// 1 TanCong || 2 PhongThu || 3 HieuUngTot || 4 HieuUngXau || 4 HieuUngBanDau

		// for (int i = 0; i < ten_card.Count;i++){
		// //them
		// 	var card = new tblCard();
		// 	card.TrongSo = trong_so[i];
		// 	card.TenCard = ten_card[i];
		// 	card.LoaiCard = loai_card[i];
		// 	card.MoTa = mo_ta[i];
		// 	dataContext.Add(card);
		// 	dataContext.SaveChanges();
		// }
		
		
		// //sua
		// var card_sua = dataContext.tblCards.Find(2);
		// card_sua.TenCard = "sword_hilt";

		// dataContext.Update(card_sua);
		// dataContext.SaveChanges();

		// //xoa
		// var card_xoa = dataContext.tblCards.Find(1);

		// dataContext.Remove(card_sua);
		// dataContext.SaveChanges();
	}

}
