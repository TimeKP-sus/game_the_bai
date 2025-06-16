using Godot;
using System;
using System.Linq;

public partial class NodeCanhLoad : Control
{
	// Called when the node enters the scene tree for the first time.
	public static string node_moi;
	public static string node_truoc;
	DataContext db = new DataContext();
	
	public override void _Ready()
	{
		if (!db.tblNguoiChois.Any())
		{
			node_moi = "res://scene/nhap_ten.tscn";
		}
		else if (string.IsNullOrEmpty(node_moi))
		{
			node_moi = "res://scene/menu/menu.tscn";
		}

		// QuadMesh
		// node_moi = "res://scene/menu/menu.tscn";

		CanhLoad.scene_hien_tai.LoadThanhPhan(node_moi, node_truoc);
	}

}
