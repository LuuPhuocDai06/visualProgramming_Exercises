using Microsoft.EntityFrameworkCore;

namespace TraSua.Models;

public partial class DanhMucMon
{
    public int MaDM { get; set; }
    public string TenDM { get; set; } = null!;
    public virtual ICollection<MonUong> MonUongs { get; set; } = new List<MonUong>();
}

public partial class MonUong
{
    public int MaMon { get; set; }
    public string TenMon { get; set; } = null!;
    public decimal GiaBan { get; set; }
    public bool DangBan { get; set; }
    public int MaDM { get; set; }
    public virtual DanhMucMon MaDMNavigation { get; set; } = null!;
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
}

public partial class HoaDon
{
    public int MaHD { get; set; }
    public DateTime NgayLap { get; set; }
    public decimal TongTien { get; set; }
    public string HinhThucTT { get; set; } = null!;
    public bool DaThanhToan { get; set; }
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
}

public partial class ChiTietHoaDon
{
    public int MaHD { get; set; }
    public int MaMon { get; set; }
    public int SoLy { get; set; }
    public decimal DonGia { get; set; }
    public virtual HoaDon MaHDNavigation { get; set; } = null!;
    public virtual MonUong MaMonNavigation { get; set; } = null!;
}

public partial class QlTraSuaContext : DbContext
{
    public QlTraSuaContext(DbContextOptions<QlTraSuaContext> options) : base(options) { }
    public virtual DbSet<DanhMucMon> DanhMucMons { get; set; }
    public virtual DbSet<MonUong> MonUongs { get; set; }
    public virtual DbSet<HoaDon> HoaDons { get; set; }
    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DanhMucMon>(e => { e.HasKey(x => x.MaDM); e.ToTable("DanhMucMon"); e.Property(x => x.TenDM).HasMaxLength(50); });
        mb.Entity<MonUong>(e =>
        {
            e.HasKey(x => x.MaMon); e.ToTable("MonUong");
            e.Property(x => x.TenMon).HasMaxLength(100);
            e.Property(x => x.GiaBan).HasColumnType("decimal(18, 0)");
            e.HasOne(x => x.MaDMNavigation).WithMany(d => d.MonUongs).HasForeignKey(x => x.MaDM).OnDelete(DeleteBehavior.ClientSetNull);
        });
        mb.Entity<HoaDon>(e =>
        {
            e.HasKey(x => x.MaHD); e.ToTable("HoaDon");
            e.Property(x => x.NgayLap).HasColumnType("datetime");
            e.Property(x => x.TongTien).HasColumnType("decimal(18, 0)");
            e.Property(x => x.HinhThucTT).HasMaxLength(20);
        });
        mb.Entity<ChiTietHoaDon>(e =>
        {
            e.HasKey(x => new { x.MaHD, x.MaMon }); e.ToTable("ChiTietHoaDon");   // khóa chính kép
            e.Property(x => x.DonGia).HasColumnType("decimal(18, 0)");
            e.HasOne(x => x.MaHDNavigation).WithMany(h => h.ChiTietHoaDons).HasForeignKey(x => x.MaHD).OnDelete(DeleteBehavior.ClientSetNull);
            e.HasOne(x => x.MaMonNavigation).WithMany(m => m.ChiTietHoaDons).HasForeignKey(x => x.MaMon).OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
