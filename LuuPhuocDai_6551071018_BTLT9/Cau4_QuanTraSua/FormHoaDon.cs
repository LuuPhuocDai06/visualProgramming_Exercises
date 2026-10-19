using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;

namespace TraSua;

public class FormHoaDon : Form
{
    static readonly CultureInfo Vn = new("vi-VN");
    readonly DateTimePicker dtp = new() { Format = DateTimePickerFormat.Short };
    readonly DataGridView dgvHD = new(), dgvCT = new();
    readonly Label lblDoanhThu = new() { AutoSize = false, TextAlign = ContentAlignment.MiddleRight,
        Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.Firebrick };

    public FormHoaDon()
    {
        var lbl = new Label { Text = "Ngày:", Left = 10, Top = 14, AutoSize = true };
        dtp.SetBounds(55, 10, 130, 24); dtp.ValueChanged += (_, _) => Reload();
        lblDoanhThu.SetBounds(450, 8, 430, 30);

        Setup(dgvHD, new Rectangle(10, 45, 870, 230));
        Setup(dgvCT, new Rectangle(10, 285, 870, 230));
        dgvHD.SelectionChanged += (_, _) => LoadChiTiet();
        Controls.AddRange(new Control[] { lbl, dtp, lblDoanhThu, dgvHD, dgvCT });
        Load += (_, _) => Reload();
    }

    static void Setup(DataGridView g, Rectangle b)
    {
        g.Bounds = b; g.ReadOnly = true; g.AllowUserToAddRows = false; g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }

    public void Reload()
    {
        try
        {
            var from = dtp.Value.Date; var to = from.AddDays(1);
            using var db = Db.Create();
            var list = db.HoaDons.AsNoTracking().Where(h => h.NgayLap >= from && h.NgayLap < to)
                .OrderByDescending(h => h.NgayLap).ToList();

            dgvHD.DataSource = list.Select(h => new
            {
                h.MaHD, GioLap = h.NgayLap, HinhThuc = h.HinhThucTT, h.TongTien
            }).ToList();
            dgvHD.Columns["MaHD"]!.HeaderText = "Mã HD";
            dgvHD.Columns["GioLap"]!.HeaderText = "Giờ lập"; dgvHD.Columns["GioLap"]!.DefaultCellStyle.Format = "HH:mm:ss";
            dgvHD.Columns["HinhThuc"]!.HeaderText = "Hình thức";
            dgvHD.Columns["TongTien"]!.HeaderText = "Tổng tiền";
            dgvHD.Columns["TongTien"]!.DefaultCellStyle.Format = "N0"; dgvHD.Columns["TongTien"]!.DefaultCellStyle.FormatProvider = Vn;

            lblDoanhThu.Text = $"Doanh thu ngày: {list.Where(h => h.DaThanhToan).Sum(h => h.TongTien).ToString("N0", Vn)} đ";
            LoadChiTiet();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Lỗi CSDL khi tải danh sách hóa đơn");
            MessageBox.Show("Không thể tải danh sách hóa đơn lúc này.", "Có lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    void LoadChiTiet()
    {
        if (dgvHD.CurrentRow == null || dgvHD.Rows.Count == 0) { dgvCT.DataSource = null; return; }
        try
        {
            int id = (int)dgvHD.CurrentRow.Cells["MaHD"].Value;
            using var db = Db.Create();
            dgvCT.DataSource = db.ChiTietHoaDons.AsNoTracking().Where(c => c.MaHD == id)
                .Select(c => new { TenMon = c.MaMonNavigation.TenMon, c.SoLy, c.DonGia, ThanhTien = c.SoLy * c.DonGia }).ToList();
            dgvCT.Columns["TenMon"]!.HeaderText = "Tên món"; dgvCT.Columns["SoLy"]!.HeaderText = "SL";
            dgvCT.Columns["DonGia"]!.HeaderText = "Đơn giá"; dgvCT.Columns["ThanhTien"]!.HeaderText = "Thành tiền";
            foreach (var n in new[] { "DonGia", "ThanhTien" })
            { dgvCT.Columns[n]!.DefaultCellStyle.Format = "N0"; dgvCT.Columns[n]!.DefaultCellStyle.FormatProvider = Vn; }
        }
        catch (Exception ex) { Log.Error(ex, "Lỗi CSDL khi tải chi tiết hóa đơn"); }
    }
}
