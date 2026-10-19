using Microsoft.EntityFrameworkCore;

namespace QuanLyDatTour.Models;

public partial class Tour
{
    public int MaTour { get; set; }
    public string TenTour { get; set; } = null!;
    public string DiemDen { get; set; } = null!;
    public string NgoaiTe { get; set; } = null!;
    public decimal GiaNgoaiTe { get; set; }
    public DateOnly NgayKhoiHanh { get; set; }
    public int SoChoToiDa { get; set; }
    public virtual ICollection<DatTour> DatTours { get; set; } = new List<DatTour>();
}

public partial class KhachHang
{
    public int MaKH { get; set; }
    public string HoTen { get; set; } = null!;
    public string SoDienThoai { get; set; } = null!;
    public string? Email { get; set; }
    public virtual ICollection<DatTour> DatTours { get; set; } = new List<DatTour>();
}

public partial class DatTour
{
    public int MaDat { get; set; }
    public int MaKH { get; set; }
    public int MaTour { get; set; }
    public int SoNguoi { get; set; }
    public DateTime NgayDat { get; set; }
    public decimal TyGia { get; set; }
    public decimal TongTienVND { get; set; }
    public virtual KhachHang MaKHNavigation { get; set; } = null!;
    public virtual Tour MaTourNavigation { get; set; } = null!;
}

public partial class QlDatTourContext : DbContext
{
    public QlDatTourContext(DbContextOptions<QlDatTourContext> options) : base(options) { }
    public virtual DbSet<Tour> Tours { get; set; }
    public virtual DbSet<KhachHang> KhachHangs { get; set; }
    public virtual DbSet<DatTour> DatTours { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Tour>(e =>
        {
            e.HasKey(x => x.MaTour); e.ToTable("Tour");
            e.Property(x => x.TenTour).HasMaxLength(100);
            e.Property(x => x.DiemDen).HasMaxLength(50);
            e.Property(x => x.NgoaiTe).HasMaxLength(3).IsUnicode(false).IsFixedLength();
            e.Property(x => x.GiaNgoaiTe).HasColumnType("decimal(18, 2)");
        });
        mb.Entity<KhachHang>(e =>
        {
            e.HasKey(x => x.MaKH); e.ToTable("KhachHang");
            e.Property(x => x.HoTen).HasMaxLength(100);
            e.Property(x => x.SoDienThoai).HasMaxLength(15).IsUnicode(false);
            e.Property(x => x.Email).HasMaxLength(100);
        });
        mb.Entity<DatTour>(e =>
        {
            e.HasKey(x => x.MaDat); e.ToTable("DatTour");
            e.Property(x => x.NgayDat).HasColumnType("datetime");
            e.Property(x => x.TyGia).HasColumnType("decimal(18, 4)");
            e.Property(x => x.TongTienVND).HasColumnType("decimal(18, 0)");
            e.HasOne(x => x.MaKHNavigation).WithMany(k => k.DatTours).HasForeignKey(x => x.MaKH).OnDelete(DeleteBehavior.ClientSetNull);
            e.HasOne(x => x.MaTourNavigation).WithMany(t => t.DatTours).HasForeignKey(x => x.MaTour).OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
