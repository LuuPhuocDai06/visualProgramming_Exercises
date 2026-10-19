using Microsoft.EntityFrameworkCore;

namespace NhaTro.Models;

public partial class QlNhaTroContext : DbContext
{
    public QlNhaTroContext(DbContextOptions<QlNhaTroContext> options) : base(options) { }

    public virtual DbSet<PhongTro> PhongTros { get; set; }
    public virtual DbSet<KhachThue> KhachThues { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<PhongTro>(e =>
        {
            e.HasKey(x => x.MaPhong); e.ToTable("PhongTro");
            e.Property(x => x.TenPhong).HasMaxLength(20);
            e.Property(x => x.GiaThue).HasColumnType("decimal(18, 0)");
            e.Property(x => x.TinhTrang).HasMaxLength(20);
        });
        mb.Entity<KhachThue>(e =>
        {
            e.HasKey(x => x.MaKhach); e.ToTable("KhachThue");
            e.Property(x => x.HoTen).HasMaxLength(100);
            e.Property(x => x.GioiTinh).HasMaxLength(5);
            e.Property(x => x.SoDienThoai).HasMaxLength(15).IsUnicode(false);
            e.Property(x => x.TenTinh).HasMaxLength(100);
            e.Property(x => x.TenPhuongXa).HasMaxLength(100);
            e.HasOne(d => d.MaPhongNavigation).WithMany(p => p.KhachThues)
                .HasForeignKey(d => d.MaPhong).OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
