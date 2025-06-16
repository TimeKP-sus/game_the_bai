using Godot;
using System;

public partial class NguoiChoi : CharacterBody2D
{
    public int mau = 20;
    public int mau_toi_da = 20;
    public int khien = 0;

    public int so_hieu_ung_tot = 0;
    public int so_hieu_ung_xau = 0;
    public int id_hieu_ung_tot;
    public int id_hieu_ung_xau;

    public static string ten_nguoi_choi;
    DataContext dataContext = new DataContext();
    public void DoiIconTrongSoHieuUng(string loai_hieu_ung, int id_hieu_ung)
    {
        if (loai_hieu_ung == "tot")
        {
            if (id_hieu_ung == 0)
            {
                GetNode<Sprite2D>("hieu_ung_tot/icon").Texture = null;
                GetNode<Label>("hieu_ung_tot/trong_so").Text = "-";
            }
            else
            {
                id_hieu_ung_tot = id_hieu_ung;
                so_hieu_ung_tot = dataContext.tblCards.Find(id_hieu_ung).TrongSo;
                GetNode<Sprite2D>("hieu_ung_tot/icon").Texture = GD.Load<CompressedTexture2D>("res://assets/cards/" + dataContext.tblCards.Find(id_hieu_ung).TenCard + ".svg");
                GetNode<Label>("hieu_ung_tot/trong_so").Text = so_hieu_ung_tot.ToString();
            }
        }
        if (loai_hieu_ung == "xau")
        {
            if (id_hieu_ung == 0)
            {
                GetNode<Sprite2D>("hieu_ung_xau/icon").Texture = null;
                GetNode<Label>("hieu_ung_xau/trong_so").Text = "-";
            }
            else
            {
                id_hieu_ung_xau = id_hieu_ung;
                so_hieu_ung_xau = dataContext.tblCards.Find(id_hieu_ung).TrongSo;
                GetNode<Sprite2D>("hieu_ung_xau/icon").Texture = GD.Load<CompressedTexture2D>("res://assets/cards/" + dataContext.tblCards.Find(id_hieu_ung).TenCard + ".svg");
                GetNode<Label>("hieu_ung_xau/trong_so").Text = so_hieu_ung_xau.ToString();
            }
        }
    }
    public override void _Ready()
    {
        // GetNode<AnimatedSprite2D>("icon").GlobalPosition = new Vector2(407, 255);
        GD.Print(GetNode<AnimatedSprite2D>("icon").GlobalPosition);

        GetNode<AnimatedSprite2D>("icon").Play("idle");
        GetNode<Label>("khien_bar/khien").Text = "+" + khien.ToString();
        GetNode<ProgressBar>("mau_bar").Value = mau;
        GetNode<ProgressBar>("mau_bar").MaxValue = mau_toi_da;
        GetNode<Label>("mau_bar/mau").Text = mau.ToString();
        GetNode<Label>("mau_bar/max").Text = "/" + mau_toi_da.ToString();

        if (QuanLyNoiDeBai.dang_danh_online == true)
        {
            GD.Print("online");
            var player = QuanLyNguoiThamGiaOnline.nguoi_choi.Find(player => player.Id == Multiplayer.GetUniqueId());
            if (player != null)
            {
                ten_nguoi_choi = player.Ten.ToString();
                GetNode<Label>("ten").Text = ten_nguoi_choi;
            }
            else
            {
                GD.Print("No valid player found.");
            }
        }

    }
    public void BinhThuong()
    {
        GetNode<AnimatedSprite2D>("icon").Play("idle");
    }
    public void TanCong()
    {
        Tween tancong = GetTree().CreateTween();
        tancong.SetParallel().TweenProperty(GetNode<AnimatedSprite2D>("icon"), "global_position", GetNode<AnimatedSprite2D>("icon").GlobalPosition + new Vector2(900, 0), 0.8f);
        tancong.Play();
        GetNode<AnimatedSprite2D>("icon").Play("attack");
        tancong.Finished += () =>
        {
            GetNode<AnimatedSprite2D>("icon").GlobalPosition = new Vector2(407, 255);
            GetNode<DoiThu>("../doi_thu").BinhThuong();
            BinhThuong();
        };
        
        
    }
    public void PhongThu()
    {
        GetNode<AnimatedSprite2D>("icon").Play("defend");
    }
    public void HieuUngTot()
    {
        GetNode<AnimatedSprite2D>("icon").Play("effect_good");
    }
    public void BiThuong()
    {
        GetNode<AnimatedSprite2D>("icon").Play("hurt");

    }
    public void Chet()
    {
        var icon = GetNode<AnimatedSprite2D>("icon");
        icon.Play("dead");
        icon.Stop();
    }

}
