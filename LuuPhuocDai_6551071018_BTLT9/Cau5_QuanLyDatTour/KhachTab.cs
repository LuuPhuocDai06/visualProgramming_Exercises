using Microsoft.EntityFrameworkCore;
using QuanLyDatTour.Models;
using Serilog;
using System.Text.RegularExpressions;
using static QuanLyDatTour.Ui;

namespace QuanLyDatTour;

public class KhachTab : UserControl
{
    readonly TextBox txtTen = new() { MaxLength = 100 }, txtSdt = new() { MaxLength = 15 }, txtEmail = new() { MaxLength = 100 };
    readonly TextBox txtTim = new() { PlaceholderText = "Tìm theo tên / SĐT..." };
    readonly DataGridView dgv = new();
    int? selectedId;

    public KhachTab()
    {
        var gb = new GroupBox { Text = "Thông tin khách hàng", Bounds = new Rectangle(10, 8, 560, 170) };
        gb.Controls.AddRange(new Control[] {
            Lb("Họ tên", 15, 28), At(txtTen, 110, 25, 430), Lb("Số điện thoại", 15, 60), At(txtSdt, 110, 57, 200),
            Lb("Email", 15, 92), At(txtEmail, 110, 89, 430),
            Btn("Thêm", 15, 130, Them), Btn("Sửa", 115, 130, Sua), Btn("Xóa", 215, 130, Xoa), Btn("Làm mới", 315, 130, LamMoi) });
        txtTim.SetBounds(10, 188, 310, 24);
        var btnTim = new Button { Text = "Tìm", Bounds = new Rectangle(330, 187, 80, 26) }; btnTim.Click += (_, _) => Reload();
        SetupGrid(dgv, new Rectangle(10, 220, 970, 375));
        dgv.CellClick += Dgv_CellClick;
        Controls.AddRange(new Control[] { gb, txtTim, btnTim, dgv });
        Load += (_, _) => Reload();
    }

    public void Reload()
    {
        try
        {
            using var db = Db.Create();
            var q = db.KhachHangs.AsNoTracking().AsQueryable();
            var kw = txtTim.Text.Trim();
            if (kw != "") q = q.Where(k => k.HoTen.Contains(kw) || k.SoDienThoai.Contains(kw));
            dgv.DataSource = q.OrderBy(k => k.HoTen).Select(k => new { k.MaKH, k.HoTen, k.SoDienThoai, k.Email }).ToList();
            dgv.Columns["MaKH"]!.HeaderText = "Mã"; dgv.Columns["MaKH"]!.FillWeight = 30;
            dgv.Columns["HoTen"]!.HeaderText = "Họ tên"; dgv.Columns["SoDienThoai"]!.HeaderText = "SĐT";
            dgv.ClearSelection(); selectedId = null;
        }
        catch (Exception ex) { Fail("tải danh sách khách hàng", ex); }
    }

    void Dgv_CellClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var r = dgv.Rows[e.RowIndex];
        selectedId = (int)r.Cells["MaKH"].Value;
        txtTen.Text = r.Cells["HoTen"].Value?.ToString(); txtSdt.Text = r.Cells["SoDienThoai"].Value?.ToString();
        txtEmail.Text = r.Cells["Email"].Value?.ToString();
    }

    bool Valid()
    {
        if (string.IsNullOrWhiteSpace(txtTen.Text)) return Warn("Họ tên không được để trống.", txtTen);
        if (!Regex.IsMatch(txtSdt.Text.Trim(), @"^0\d{9,10}$")) return Warn("Số điện thoại phải gồm 10-11 chữ số và bắt đầu bằng 0.", txtSdt);
        if (txtEmail.Text.Trim() != "" && !Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return Warn("Email không hợp lệ.", txtEmail);
        return true;
    }

    void Apply(KhachHang k)
    {
        k.HoTen = txtTen.Text.Trim(); k.SoDienThoai = txtSdt.Text.Trim();
        k.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
    }

    void Them()
    {
        if (!Valid()) return;
        try { using var db = Db.Create(); var k = new KhachHang(); Apply(k); db.KhachHangs.Add(k); db.SaveChanges();
              Log.Information("Thêm khách MaKH={Id}", k.MaKH); LamMoi(); }
        catch (Exception ex) { Fail("thêm khách hàng", ex); }
    }

    void Sua()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn khách cần sửa."); return; }
        if (!Valid()) return;
        try { using var db = Db.Create(); var k = db.KhachHangs.Find(selectedId); if (k != null) { Apply(k); db.SaveChanges(); }
              Log.Information("Sửa khách MaKH={Id}", selectedId); LamMoi(); }
        catch (Exception ex) { Fail("sửa khách hàng", ex); }
    }

    void Xoa()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn khách cần xóa."); return; }
        if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            using var db = Db.Create();
            if (db.DatTours.Any(d => d.MaKH == selectedId)) { MessageBox.Show("Khách này đã có đơn đặt tour nên không thể xóa."); return; }
            var k = db.KhachHangs.Find(selectedId); if (k != null) { db.KhachHangs.Remove(k); db.SaveChanges(); }
            Log.Information("Xóa khách MaKH={Id}", selectedId); LamMoi();
        }
        catch (Exception ex) { Fail("xóa khách hàng", ex); }
    }

    void LamMoi() { selectedId = null; txtTen.Clear(); txtSdt.Clear(); txtEmail.Clear(); txtTim.Clear(); Reload(); }
}
