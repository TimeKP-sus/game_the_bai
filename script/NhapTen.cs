using Godot;
using System;

public partial class NhapTen : Control
{

	// Called when the node enters the scene tree for the first time.
	DataContext db = new DataContext();

	public void _on_xac_nhan_button_down()
	{
		tblNguoiChoi nguoiChoi = new tblNguoiChoi();
		nguoiChoi.TenNguoiChoi = GetNode<LineEdit>("ten").Text;
		nguoiChoi.SoTranThangBot = 0;
		nguoiChoi.SoTranThuaBot = 0;
		nguoiChoi.SoTranThangOnline = 0;
		nguoiChoi.SoTranThuaOnline = 0;

		db.tblNguoiChois.Add(nguoiChoi);
		db.SaveChanges();

		NodeCanhLoad.node_truoc = this.Name;
		NodeCanhLoad.node_moi = "res://scene/menu/menu.tscn";
		GetTree().ChangeSceneToFile("res://scene/node_canh_load.tscn");
	}

}
