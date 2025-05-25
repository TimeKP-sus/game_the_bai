using Godot;
using System;
using System.Linq;

public partial class QuanLyKetNoiOnline : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] public int id_ket_noi = 7777;
	[Export] public string dia_chi_ket_noi = "127.0.0.1";
	private Label thong_bao_text;
	private TextEdit bang_nguoi_choi_text;
	private LineEdit ten_text;

	private ENetMultiplayerPeer peer;

	private Main main_game;

	


	public override void _Ready()
	{
		// MouseFilter = MouseFilterEnum.Stop; 
		main_game = ResourceLoader.Load<PackedScene>("res://scene/main/main.tscn").Instantiate<Main>();
		thong_bao_text = GetNode<Label>("thong_bao");
		bang_nguoi_choi_text = GetNode<TextEdit>("bang_nguoi_choi");
		ten_text = GetNode<LineEdit>("ten");


		Multiplayer.PeerConnected += KetNoi;
		Multiplayer.PeerDisconnected += NgatKetNoi;
		Multiplayer.ConnectedToServer += KetNoiThanhCong;

		Multiplayer.ConnectionFailed += KetNoiThatBai;

		GD.Print("kiem tra " + OS.GetCmdlineArgs());
		// if(OS.GetCmdlineArgs().Contains("--server")){
		// 	HostGame();
		// }
	}
	public void _on_tham_gia_button_down(){
		dia_chi_ket_noi = GetNode<LineEdit>("dia_chi_ket_noi").Text;
		peer = new ENetMultiplayerPeer();
		peer.CreateClient(dia_chi_ket_noi,id_ket_noi);

		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;
		GetNode<Button>("host").Visible = false;
	   	GetNode<Button>("join").Visible = false;
		CatNhatBanNguoiChoi();
		QuanLyNguoiThamGiaOnline.luot_choi = 2;
		thong_bao_text.Text = "Đang tìm kiếm...";
		
	}
	public void _on_dau_voi_bot_button_down(){
		QuanLyNoiDeBai.dang_danh_voi_bot = true;
		ManHinhCho.canh_moi = "res://scene/online/quan_ly_ket_noi_online.tscn";
		GetTree().ChangeSceneToFile("res://scene/main/main.tscn");
	}


	
	private void KetNoi(long id)
	{
		GD.Print("Người chơi kết nối thành công: " + id.ToString());
		
	}
	private void NgatKetNoi(long id)
	{
		GD.Print("Người chơi đã ngắt kết nối: " + id.ToString());
	}
	private void KetNoiThatBai()
	{
		thong_bao_text.Text = "Không có máy chủ nào đang mở";
	}


	private void KetNoiThanhCong()
	{
		thong_bao_text.Text = "Kết nối thành công";
		RpcId(1,nameof(GuiThongTinNguoiChoi), ten_text.Text , Multiplayer.GetUniqueId());
		RpcId(1,nameof(DuNguoiChoi));
		Rpc(nameof(DuNguoiChoi));
		// CatNhatBanNguoiChoi();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void CatNhatBanNguoiChoi(){
		bang_nguoi_choi_text.Text = "";
		foreach (NguoiChoiOnline item in QuanLyNguoiThamGiaOnline.nguoi_choi){
			bang_nguoi_choi_text.Text += item.Id.ToString() + "__" + item.Ten + "\n";
			GD.Print(item.Id.ToString() + "__" + item.Ten);
		}
		if (QuanLyNguoiThamGiaOnline.nguoi_choi.Count > 1){
			thong_bao_text.Text = "Đã đủ người chơi";
		}
	}

	public void _on_host_button_down()
    {
       HostGame();
	   GuiThongTinNguoiChoi(ten_text.Text,1);
	   GetNode<Button>("host").Visible = false;
	   GetNode<Button>("join").Visible = false;
	   GetNode<Button>("dau_voi_bot").Visible = false;
	   CatNhatBanNguoiChoi();
    }
	public void _on_button_button_down(){
		CatNhatBanNguoiChoi();
	}

    private void HostGame()
    {
        peer = new ENetMultiplayerPeer();
        Error err = peer.CreateServer(id_ket_noi, 3);
        if (err != Error.Ok)
        {
            GD.Print("Không thể tạo phòng id:" + err.ToString());

        }
        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;
        thong_bao_text.Text = "Chờ người chơi...";
		QuanLyNguoiThamGiaOnline.luot_choi = 1;
    }

    public void _on_join_button_down()
	{
		dia_chi_ket_noi = "127.0.0.1";
		peer = new ENetMultiplayerPeer();
		peer.CreateClient(dia_chi_ket_noi,id_ket_noi);

		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;
		GetNode<Button>("host").Visible = false;
	   	GetNode<Button>("join").Visible = false;
		CatNhatBanNguoiChoi();
		QuanLyNguoiThamGiaOnline.luot_choi = 2;
		thong_bao_text.Text = "Đang tìm kiếm...";
	}

	public void _on_start_button_down()
	{
		
		Rpc(nameof(BatDau));
	}
	//rpc

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void BatDau(){
		// GetTree().Root.AddChild(main_game);
		// Hide();
		QuanLyNoiDeBai.dang_danh_online = true;
		GetTree().ChangeSceneToFile("res://scene/main/main.tscn");
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void GuiThongTinNguoiChoi(string name, int id){
		NguoiChoiOnline nguoiChoiOnline =  new NguoiChoiOnline(){
			Ten = name,
			Id = id,
		};
		if(!QuanLyNguoiThamGiaOnline.nguoi_choi.Contains(nguoiChoiOnline)){
			
			QuanLyNguoiThamGiaOnline.nguoi_choi.Add(nguoiChoiOnline);
		}
		if (Multiplayer.IsServer()){
			foreach (NguoiChoiOnline item in QuanLyNguoiThamGiaOnline.nguoi_choi){
				Rpc(nameof(GuiThongTinNguoiChoi),item.Ten,item.Id);
			}
		}
		CatNhatBanNguoiChoi();
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void DuNguoiChoi(){
		GetNode<Button>("start").Visible = true;
	}
	// [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	// private void LayThongTinPhong(string tt){
	// 	tt = 
	// }
	


}
