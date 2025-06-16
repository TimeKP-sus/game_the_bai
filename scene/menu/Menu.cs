using Godot;
using System;
using System.Linq;

public partial class Menu : Node2D
{
	public static string ten_nguoi_choi = "";
	DataContext db = new DataContext();
	public override void _Ready() {
		ten_nguoi_choi = db.tblNguoiChois.FirstOrDefault().TenNguoiChoi;
		GetNode<Button>("ten_tai_khoan").Text = "Tên người chơi: " + ten_nguoi_choi;
	}
	public void _on_bat_dau_button_down()
	{
		// CanhLoad.dia_chi = "res://scene/online/quan_ly_ket_noi_online.tscn";
		NodeCanhLoad.node_moi = "res://scene/online/quan_ly_ket_noi_online.tscn";
		// this.QueueFree();
		NodeCanhLoad.node_truoc = this.Name;
		GetTree().ChangeSceneToFile("res://scene/node_canh_load.tscn");
		// this.QueueFree();
	}

	public void _on_cac_the_bai_button_down()
	{
		NodeCanhLoad.node_moi = "res://scene/cac_the_bai/cac_the_bai.tscn";
		NodeCanhLoad.node_truoc = this.Name;
		GetTree().ChangeSceneToFile("res://scene/node_canh_load.tscn");
	}
	public void _on_ten_tai_khoan_button_down()
	{
		//ten tai khoan
	}
}
