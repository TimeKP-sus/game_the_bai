using Godot;
using System;
using static Godot.ResourceLoader;

public partial class LoadCanhCho : Godot.ProgressBar
{
	public string dia_chi;
	public Node node_load;
	private Tween tween;
	public bool dang_load = false;  
	public static LoadCanhCho scene_hien_tai;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scene_hien_tai = this;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!dang_load) return;
		if(dia_chi == "") return;

		Godot.Collections.Array phan_tram_load = new();
		ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(dia_chi,phan_tram_load);
		double phan_tram = (double)phan_tram_load[0];

		if(status == ResourceLoader.ThreadLoadStatus.Loaded){
			// if(tween.IsRunning()) return;
			dang_load = false;
			tween = GetTree().CreateTween();
			tween.TweenProperty(this,"value", phan_tram, 0.5);
			tween.TweenCallback(Callable.From(ThemCanhMoi));

		}

		if (Value == phan_tram) return;
		if (tween == null) return;

		 tween = GetTree().CreateTween();
		 tween.TweenProperty(this,"value", phan_tram, 0.5);
	}

    private void ThemCanhMoi()
    {
        var node = ResourceLoader.LoadThreadedGet(dia_chi) as PackedScene;
		node_load = node.Instantiate();
		GetTree().Root.AddChild(node_load);
		this.Visible = false;
    }

	public void LoadThanhPhan(string dia_chi_tp){
		GD.Print(dia_chi_tp);
		
		if(dang_load) return;
		dia_chi = dia_chi_tp;
		ResourceLoader.LoadThreadedRequest(dia_chi);
		dang_load = true;
		Visible = true;
		Value = 0;
		if (node_load == null) return;
		node_load.QueueFree();
		this.QueueFree();
	}

}
