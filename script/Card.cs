
using Godot;
using System;

public partial class Card : Node2D
{
    public int id_card;
	public int trong_so;
	public string loai_card;
	public string ten_card;
	public string mo_ta;
    public CompressedTexture2D icon_card;

	// public int id_card;
    // public int trong_so;
    // public CompressedTexture2D icon_card;
    // public Card(){
    // 	// id_card = id;
    // 	// trong_so = so;
    // 	// icon_card = icon;
    // }
    public ODeBai o_de_bai_tim_thay;
	public bool da_bi_dung = false;

	public void XoaBai() {
		Tween tween = CreateTween();
		tween.TweenProperty(this,"modulate:a",0,0.5f);
		tween.Finished += () => {
			QueueFree();
		};
	}


	[Signal]
	public delegate void cam_vaoEventHandler(Card card);

	[Signal]
	public delegate void tha_raEventHandler(Card card);

	

	
	
	// public bool chuot_vao = false;
	// public bool chuot_ra;
	public Vector2 vi_tri_khoi_dau = new Vector2();
    public override void _Ready()
    {
        // AddUserSignal("");
        // Position = new Vector2(0,890);
        if (GetParent() is QuanLyCard quanly)
        {
            quanly.KetNoiCardSignal(this);
        }
        GetNode<Label>("trong_so").Text = trong_so.ToString();
        GetNode<Sprite2D>("icon_card").Texture = icon_card;
        
	}

	public void _on_area_2d_mouse_entered(){
		EmitSignal(SignalName.cam_vao, this);
	
		// Scale = new Vector2(1.1f,1.1f);

	}
	public void _on_area_2d_mouse_exited(){	
		EmitSignal(SignalName.tha_ra,this);
		// GD.Print("out");
		// ZIndex = 1;
	}

}
