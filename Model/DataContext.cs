using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

public class DataContext:DbContext{
    public DbSet<tblCard> tblCards { get; set;}
    public DbSet<tblNguoiChoi> tblNguoiChois { get; set;}
    // public DbSet<tblQuanLyTranDau> tblQuanLyTranDaus { get; set;}


    public DataContext(){
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=GameTheBai.db");
    }
}