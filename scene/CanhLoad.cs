using Godot;
using System;
using static Godot.ResourceLoader;
public partial class CanhLoad : Node2D
{
	// public bool da_load_canh = false;
	public static string dia_chi;
	public Node node_load;
	private Tween tween;
	public bool dang_load = false;
	public static CanhLoad scene_hien_tai;
	public Godot.ProgressBar progressBar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		progressBar = GetNode<Godot.ProgressBar>("ProgressBar");
		scene_hien_tai = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!dang_load) return;
		if (dia_chi == "") return;

		Godot.Collections.Array phan_tram_load = new();
		ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(dia_chi, phan_tram_load);
		double phan_tram = (double)phan_tram_load[0];

		if (status == ResourceLoader.ThreadLoadStatus.Loaded)
		{
			// if(tween.IsRunning()) return;
			dang_load = false;
			tween = GetTree().CreateTween();
			tween.TweenProperty(progressBar, "value", phan_tram, 2.0);
			tween.TweenCallback(Callable.From(ThemCanhMoi));

		}

		if (progressBar.Value == phan_tram) return;
		if (tween == null) return;

		tween = GetTree().CreateTween();
		tween.TweenProperty(progressBar, "value", phan_tram, 2.0);
	}

	private void ThemCanhMoi()
	{
		var node = ResourceLoader.LoadThreadedGet(dia_chi) as PackedScene;
		node_load = node.Instantiate();
		GetTree().Root.AddChild(node_load);
		foreach (Node child in GetTree().Root.GetChildren())
		{
			if (child.Name == "NodeCanhLoad")
			{
				child.QueueFree();
				break;
			}
		}

	}

	public void LoadThanhPhan(string dia_chi_tp, string node_truoc)
	{
		if (node_truoc != null)
		{
			foreach (Node child in GetTree().Root.GetChildren())
			{
				if (child.Name == node_truoc)
				{
					child.QueueFree();
					break;
				}
			}
		}


		if (dang_load) return;
		dia_chi = dia_chi_tp;
		ResourceLoader.LoadThreadedRequest(dia_chi);
		dang_load = true;
		Visible = true;
		progressBar.Value = 0;
		if (node_load == null) return;
		node_load.QueueFree();
		QueueFree();
	}
}
