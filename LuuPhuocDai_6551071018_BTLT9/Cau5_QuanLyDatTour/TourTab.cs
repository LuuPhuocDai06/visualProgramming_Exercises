using Microsoft.EntityFrameworkCore;
using QuanLyDatTour.Models;
using Serilog;
using System.Globalization;
using static QuanLyDatTour.Ui;

namespace QuanLyDatTour;

public class TourTab : UserControl
{
    static readonly CultureInfo Vn = new("vi-VN");
    readonly TextBox txtTen = new() { MaxLength = 100 }, txtDiem = new() { MaxLength = 50 };
    readonly ComboBox cboNgoai = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly NumericUpDown nudGia = new() { DecimalPlaces = 2, Maximum = 1_000_000_000_000, Increment = 100, ThousandsSeparator = true };
    readonly DateTimePicker dtpKH = new() { Format = DateTimePickerFormat.Short };
    readonly NumericUpDown nudCho = new() { Minimum = 1, Maximum = 1000, Value = 10 };
    readonly TextBox txtTim = new() { PlaceholderText = "Tìm theo tên tour / điểm đến..." };
    readonly DataGridView dgv = new();
    int? selectedId;

    public TourTab()
    {
        cboNgoai.Items.AddRange(new object[] { "THB", "KRW", "JPY", "USD" }); cboNgoai.SelectedIndex = 0;
        var gb = new GroupBox { Text = "Thông tin tour", Bounds = new Rectangle(10, 8, 560, 215) };
        gb.Controls.AddRange(new Control[] {
            Lb("Tên tour", 15, 28), At(txtTen, 110, 25, 430), Lb("Điểm đến", 15, 60), At(txtDiem, 110, 57, 430),
            Lb("Ngoại tệ", 15, 92), At(cboNgoai, 110, 89, 120), Lb("Giá/người", 250, 92), At(nudGia, 320, 89, 220),
            Lb("Khởi hành", 15, 124), At(dtpKH, 110, 121, 120), Lb("Số chỗ tối đa", 250, 124), At(nudCho, 340, 121, 80),
            Btn("Thêm", 15, 165, Them), Btn("Sửa", 115, 165, Sua), Btn("Xóa", 215, 165, Xoa), Btn("Làm mới", 315, 165, LamMoi) });
        var btnTim = new Button { Text = "Tìm", Bounds = new Rectangle(330, 232, 80, 26) }; btnTim.Click += (_, _) => Reload();
        txtTim.SetBounds(10, 233, 310, 24);
        SetupGrid(dgv, new Rectangle(10, 265, 970, 330));
        dgv.CellClick += Dgv_CellClick;
        Controls.AddRange(new Control[] { gb, txtTim, btnTim, dgv });
        Load += (_, _) => Reload();
    }

    public void Reload()
    {
        try
        {
            using var db = Db.Create();
            var q = db.Tours.AsNoTracking().AsQueryable();
            var kw = txtTim.Text.Trim();
            if (kw != "") q = q.Where(t => t.TenTour.Contains(kw) || t.DiemDen.Contains(kw));
            var list = q.OrderBy(t => t.NgayKhoiHanh).ToList();
            dgv.DataSource = list.Select(t => new
            {
                t.MaTour, t.TenTour, t.DiemDen, t.NgoaiTe, t.GiaNgoaiTe,
                NgayKhoiHanh = t.NgayKhoiHanh.ToDateTime(TimeOnly.MinValue), t.SoChoToiDa
            }).ToList();
            var c = dgv.Columns;
            c["MaTour"]!.HeaderText = "Mã"; c["MaTour"]!.FillWeight = 30; c["TenTour"]!.HeaderText = "Tên tour";
            c["DiemDen"]!.HeaderText = "Điểm đến"; c["NgoaiTe"]!.HeaderText = "Ngoại tệ"; c["NgoaiTe"]!.FillWeight = 45;
            c["GiaNgoaiTe"]!.HeaderText = "Giá/người"; c["GiaNgoaiTe"]!.DefaultCellStyle.Format = "N2";
            c["GiaNgoaiTe"]!.DefaultCellStyle.FormatProvider = Vn;
            c["NgayKhoiHanh"]!.HeaderText = "Khởi hành"; c["NgayKhoiHanh"]!.DefaultCellStyle.Format = "dd/MM/yyyy";
            c["SoChoToiDa"]!.HeaderText = "Số chỗ"; c["SoChoToiDa"]!.FillWeight = 45;
            dgv.ClearSelection(); selectedId = null;
        }
        catch (Exception ex) { Fail("tải danh sách tour", ex); }
    }

    void Dgv_CellClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var r = dgv.Rows[e.RowIndex];
        selectedId = (int)r.Cells["MaTour"].Value;
        txtTen.Text = r.Cells["TenTour"].Value?.ToString(); txtDiem.Text = r.Cells["DiemDen"].Value?.ToString();
        cboNgoai.SelectedItem = r.Cells["NgoaiTe"].Value?.ToString()?.Trim();
        nudGia.Value = Math.Min(nudGia.Maximum, (decimal)r.Cells["GiaNgoaiTe"].Value);
        dtpKH.Value = (DateTime)r.Cells["NgayKhoiHanh"].Value;
        nudCho.Value = Math.Clamp((int)r.Cells["SoChoToiDa"].Value, (int)nudCho.Minimum, (int)nudCho.Maximum);
    }

    bool Valid()
    {
        if (string.IsNullOrWhiteSpace(txtTen.Text)) return Warn("Tên tour không được để trống.", txtTen);
        if (string.IsNullOrWhiteSpace(txtDiem.Text)) return Warn("Điểm đến không được để trống.", txtDiem);
        if (nudGia.Value <= 0) return Warn("Giá phải lớn hơn 0.", nudGia);
        return true;
    }

    void Apply(Tour t)
    {
        t.TenTour = txtTen.Text.Trim(); t.DiemDen = txtDiem.Text.Trim(); t.NgoaiTe = (string)cboNgoai.SelectedItem!;
        t.GiaNgoaiTe = nudGia.Value; t.NgayKhoiHanh = DateOnly.FromDateTime(dtpKH.Value); t.SoChoToiDa = (int)nudCho.Value;
    }

    void Them()
    {
        if (!Valid()) return;
        try
        {
            using var db = Db.Create(); var t = new Tour(); Apply(t); db.Tours.Add(t); db.SaveChanges();
            Log.Information("Thêm tour MaTour={Id}", t.MaTour); LamMoi();
        }
        catch (Exception ex) { Fail("thêm tour", ex); }
    }

    void Sua()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn tour cần sửa."); return; }
        if (!Valid()) return;
        try
        {
            using var db = Db.Create(); var t = db.Tours.Find(selectedId); if (t == null) return;
            int booked = db.DatTours.Where(d => d.MaTour == t.MaTour).Sum(d => (int?)d.SoNguoi) ?? 0;
            if (nudCho.Value < booked) { Warn($"Tour này đã có {booked} người đặt, số chỗ tối đa không được nhỏ hơn.", nudCho); return; }
            Apply(t); db.SaveChanges(); Log.Information("Sửa tour MaTour={Id}", t.MaTour); LamMoi();
        }
        catch (Exception ex) { Fail("sửa tour", ex); }
    }

    void Xoa()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn tour cần xóa."); return; }
        if (MessageBox.Show("Bạn có chắc muốn xóa tour này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            using var db = Db.Create();
            if (db.DatTours.Any(d => d.MaTour == selectedId)) { MessageBox.Show("Tour này đã có người đặt nên không thể xóa."); return; }
            var t = db.Tours.Find(selectedId); if (t != null) { db.Tours.Remove(t); db.SaveChanges(); }
            Log.Information("Xóa tour MaTour={Id}", selectedId); LamMoi();
        }
        catch (Exception ex) { Fail("xóa tour", ex); }
    }

    void LamMoi()
    {
        selectedId = null; txtTen.Clear(); txtDiem.Clear(); cboNgoai.SelectedIndex = 0; nudGia.Value = 0;
        dtpKH.Value = DateTime.Today; nudCho.Value = 10; txtTim.Clear(); Reload();
    }
}
