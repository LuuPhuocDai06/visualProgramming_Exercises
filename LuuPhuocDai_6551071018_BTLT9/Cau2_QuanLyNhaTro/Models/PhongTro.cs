namespace NhaTro.Models;

public partial class PhongTro
{
    public int MaPhong { get; set; }
    public string TenPhong { get; set; } = null!;
    public decimal GiaThue { get; set; }
    public string TinhTrang { get; set; } = null!;
    public virtual ICollection<KhachThue> KhachThues { get; set; } = new List<KhachThue>();
}

public partial class KhachThue
{
    public int MaKhach { get; set; }
    public string HoTen { get; set; } = null!;
    public string GioiTinh { get; set; } = null!;
    public DateOnly NgaySinh { get; set; }
    public string SoDienThoai { get; set; } = null!;
    public int? MaTinh { get; set; }
    public string? TenTinh { get; set; }
    public int? MaPhuongXa { get; set; }
    public string? TenPhuongXa { get; set; }
    public bool DaDangKyTamTru { get; set; }
    public int MaPhong { get; set; }
    public virtual PhongTro MaPhongNavigation { get; set; } = null!;   // navigation property
}
