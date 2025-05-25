
using System.ComponentModel.DataAnnotations;
using Godot;

public class tblCard{
    [Key]
    public int Id { get; set;}
    public string TenCard { get; set;}
    public int TrongSo{ get; set;}
    public string LoaiCard { get; set;}
    public string MoTa{get; set;}
    // public string Icon { get; set;}

}