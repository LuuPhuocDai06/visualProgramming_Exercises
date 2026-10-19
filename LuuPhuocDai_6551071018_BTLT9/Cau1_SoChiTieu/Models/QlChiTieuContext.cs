using Microsoft.EntityFrameworkCore;

namespace SoChiTieu.Models;

public partial class QlChiTieuContext : DbContext
{
    public QlChiTieuContext(DbContextOptions<QlChiTieuContext> options) : base(options) { }

    public virtual DbSet<ChiTieu> ChiTieus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTieu>(e =>
        {
            e.HasKey(x => x.MaCT);
            e.ToTable("ChiTieu");
            e.Property(x => x.NoiDung).HasMaxLength(100);
            e.Property(x => x.LoaiChi).HasMaxLength(30);
            e.Property(x => x.SoTien).HasColumnType("decimal(18, 0)");
            e.Property(x => x.GhiChu).HasMaxLength(200);
        });
    }
}
