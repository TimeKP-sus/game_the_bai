using System.ComponentModel.DataAnnotations;
using Godot;

public class tblNguoiChoi{
    [Key]
    public int Id { get; set;}
    public string TenNguoiChoi { get; set;}
    public System.Collections.Generic.List<tblCard> KhoCard { get; set;}
    public string DuLieuChoiGame{ get; set;}
    public string MatKhau { get; set; }
    // public string Icon { get; set;}

}