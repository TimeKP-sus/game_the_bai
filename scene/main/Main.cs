using Godot;
using System;

public partial class Main : Node2D
{
    public static bool dang_huong_dan = false;
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        if (QuanLyNoiDeBai.dang_danh_online)
        {
            GD.Print(QuanLyKetNoiOnline.peer.GetUniqueId().ToString());
            GD.Print(QuanLyKetNoiOnline.peer.GetConnectionStatus().ToString());
        }

        GD.Load<PackedScene>("res://scene/card.tscn");
        GD.Load<PackedScene>("res://scene/o_de_bai.tscn");
    }

    public void _on_huong_dan_button_down()
    {
        dang_huong_dan = true;
        GetNode<Control>("huongdan/buoc1").Visible = true;
    }

	public void _on_quay_ve_button_down()
    {

        if (QuanLyNoiDeBai.dang_danh_online)
        {
            // QuanLyKetNoiOnline.NgatKetNoi(QuanLyKetNoiOnline.peer.GetUniqueId());
            if (Multiplayer.MultiplayerPeer != null)
            {
                Multiplayer.MultiplayerPeer.Close();
            }
        }

        NodeCanhLoad.node_moi = "res://scene/online/quan_ly_ket_noi_online.tscn";
        NodeCanhLoad.node_truoc = this.Name;
        GetTree().ChangeSceneToFile("res://scene/node_canh_load.tscn");
    }
    
    public int so_buoc;

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void ThongBaoNgatKetNoi()
	{
		GetTree().Root.GetNode<QuanLyKetNoiOnline>("QuanLyKetNoiOnline").NgatKetNoi(QuanLyKetNoiOnline.peer.GetUniqueId());
		
		GetTree().ChangeSceneToFile("res://scene/online/quan_ly_ket_noi_online.tscn");
		GD.Print("Ngắt kết nối");
	}
}
