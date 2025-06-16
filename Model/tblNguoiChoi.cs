using System.ComponentModel.DataAnnotations;
using Godot;

public class tblNguoiChoi{
    [Key]
    public int Id { get; set;}
    public string TenNguoiChoi { get; set;}
    public System.Collections.Generic.List<tblCard> KhoCard { get; set;}
    public string DuLieuChoiGame{ get; set;}
    public int SoTranThangOnline{ get; set;}
    public int SoTranThuaOnline{ get; set;}
    public int SoTranThangBot{ get; set;}
    public int SoTranThuaBot{ get; set;}
    public string MatKhau { get; set; }
    // public string Icon { get; set;}

}