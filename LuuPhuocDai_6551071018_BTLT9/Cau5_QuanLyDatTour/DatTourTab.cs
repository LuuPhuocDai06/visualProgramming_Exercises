using Microsoft.EntityFrameworkCore;
using QuanLyDatTour.Models;
using Serilog;
using System.Globalization;
using static QuanLyDatTour.Ui;

namespace QuanLyDatTour;

public class DatTourTab : UserControl
{
    static readonly CultureInfo Vn = new("vi-VN");
    sealed record KhachItem(int MaKH, string Text) { public override string ToString() => Text; }
    sealed record TourItem(Tour T) { public override string ToString() => T.TenTour; }

    readonly ComboBox cboKhach = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly ComboBox cboTour = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly NumericUpDown nudNguoi = new() { Minimum = 1, Maximum = 1, Value = 1 };
    readonly Label lblCon = new() { AutoSize = true, ForeColor = Color.DimGray };
    readonly Label lblGiaNgoai = new() { AutoSize = true }, lblTyGia = new() { AutoSize = true };
    readonly Label lblGiaVnd = new() { AutoSize = true };
    readonly Label lblTong = new() { AutoSize = true, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.DarkRed };
    readonly Label lblNguon = new() { AutoSize = true, ForeColor = Color.DimGray, BackColor = Color.Gainsboro };
    readonly Button btnDat = new() { Text = "Đặt tour", Enabled = false, BackColor = Color.SeaGreen, ForeColor = Color.White };

    readonly TextBox txtTim = new() { PlaceholderText = "Tìm theo tên khách hoặc tên tour" };
    readonly ComboBox cboThang = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly DataGridView dgv = new();
    readonly Label lblDoanhThu = new() { AutoSize = false, TextAlign = ContentAlignment.MiddleRight,
        Font = new Font("Segoe UI", 11F, FontStyle.Bold) };

    decimal? currentRate; int remaining;

    public DatTourTab()
    {
        cboThang.Items.Add("Tất cả");
        for (int m = 1; m <= 12; m++) cboThang.Items.Add($"Tháng {m}");
        cboThang.SelectedIndex = 0;

        var gb = new GroupBox { Text = "Thông tin đặt tour", Bounds = new Rectangle(10, 8, 600, 250) };
        gb.Controls.AddRange(new Control[] {
            Lb("Khách hàng:", 15, 28), At(cboKhach, 120, 25, 450),
            Lb("Tour:", 15, 60), At(cboTour, 120, 57, 450),
            Lb("Số người:", 15, 92), At(nudNguoi, 120, 89, 80), At(lblCon, 210, 93, 300) });
        lblGiaNgoai.Location = new Point(15, 126); lblTyGia.Location = new Point(15, 150); lblNguon.Location = new Point(330, 150);
        lblGiaVnd.Location = new Point(15, 174); lblTong.Location = new Point(15, 198);
        btnDat.SetBounds(450, 195, 120, 34);
        gb.Controls.AddRange(new Control[] { lblGiaNgoai, lblTyGia, lblNguon, lblGiaVnd, lblTong, btnDat });

        cboTour.SelectedIndexChanged += async (_, _) => await RefreshRateAsync();
        nudNguoi.ValueChanged += (_, _) => UpdateTotals();
        btnDat.Click += (_, _) => DoDatTour();

        txtTim.SetBounds(10, 268, 300, 24);
        var lbl = Lb("Tháng khởi hành:", 325, 268); cboThang.SetBounds(440, 268, 100, 24);
        var btnTim = new Button { Text = "Tìm", Bounds = new Rectangle(550, 267, 70, 26) }; btnTim.Click += (_, _) => LoadGrid();
        SetupGrid(dgv, new Rectangle(10, 300, 970, 260));
        lblDoanhThu.SetBounds(10, 565, 970, 30); lblDoanhThu.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        Controls.AddRange(new Control[] { gb, txtTim, lbl, cboThang, btnTim, dgv, lblDoanhThu });
        Load += (_, _) => ReloadAll();
    }

    public void ReloadAll() { LoadCombos(); LoadGrid(); }

    void LoadCombos()
    {
        try
        {
            using var db = Db.Create();
            var kh = db.KhachHangs.AsNoTracking().OrderBy(k => k.HoTen).ToList().Select(k => new KhachItem(k.MaKH, $"{k.HoTen} - {k.SoDienThoai}")).ToArray();
            var tours = db.Tours.AsNoTracking().OrderBy(t => t.NgayKhoiHanh).ToList().Select(t => new TourItem(t)).ToArray();
            cboKhach.Items.Clear(); cboKhach.Items.AddRange(kh); if (kh.Length > 0) cboKhach.SelectedIndex = 0;
            int keep = (cboTour.SelectedItem as TourItem)?.T.MaTour ?? -1;
            cboTour.Items.Clear(); cboTour.Items.AddRange(tours);
            var sel = tours.FirstOrDefault(t => t.T.MaTour == keep) ?? tours.FirstOrDefault();
            if (sel != null) cboTour.SelectedItem = sel;      // kích hoạt SelectedIndexChanged -> lấy tỷ giá
        }
        catch (Exception ex) { Fail("tải danh sách khách/tour", ex); }
    }

    static int Remaining(QlDatTourContext db, Tour t) =>
        t.SoChoToiDa - (db.DatTours.Where(d => d.MaTour == t.MaTour).Sum(d => (int?)d.SoNguoi) ?? 0);

    async Task RefreshRateAsync()
    {
        currentRate = null; btnDat.Enabled = false;
        if (cboTour.SelectedItem is not TourItem ti) return;
        var t = ti.T;
        try { using var db = Db.Create(); remaining = Remaining(db, t); }
        catch (Exception ex) { Fail("kiểm tra số chỗ còn lại", ex); return; }

        nudNguoi.Maximum = Math.Max(1, remaining); nudNguoi.Value = 1;     // không vượt quá số chỗ còn lại
        lblCon.Text = remaining > 0 ? $"(còn {remaining} chỗ)" : "(HẾT CHỖ)";
        lblGiaNgoai.Text = $"Giá/người: {t.GiaNgoaiTe.ToString("N2", Vn)} {t.NgoaiTe.Trim()}";
        lblTyGia.Text = "Tỷ giá hôm nay: đang lấy…"; lblGiaVnd.Text = ""; lblTong.Text = ""; lblNguon.Text = "";

        try
        {
            var r = await TyGiaService.Instance.LayTyGiaAsync(t.NgoaiTe);
            if (cboTour.SelectedItem is not TourItem cur || cur.T.MaTour != t.MaTour) return;   // người dùng đã đổi tour
            currentRate = r.Rate;
            lblTyGia.Text = $"Tỷ giá hôm nay: 1 {t.NgoaiTe.Trim()} = {r.Rate.ToString("N2", Vn)} VND";
            lblNguon.Text = $" Tỷ giá lấy từ: {r.Source}{(r.FromCache ? " (bộ nhớ đệm)" : "")} ";
            UpdateTotals();
        }
        catch (TyGiaException)
        {
            lblTyGia.Text = "Tỷ giá hôm nay: không lấy được";
            MessageBox.Show("Không lấy được tỷ giá, vui lòng kiểm tra kết nối mạng rồi chọn lại tour.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    void UpdateTotals()
    {
        if (cboTour.SelectedItem is not TourItem ti || currentRate is not decimal rate) return;
        decimal perPerson = Math.Round(ti.T.GiaNgoaiTe * rate, 0);
        decimal total = Math.Round(ti.T.GiaNgoaiTe * nudNguoi.Value * rate, 0);
        lblGiaVnd.Text = $"Giá/người: {perPerson.ToString("N0", Vn)} VND";
        lblTong.Text = $"Tổng tiền: {total.ToString("N0", Vn)} đ";
        btnDat.Enabled = remaining > 0;
    }

    void DoDatTour()
    {
        if (cboKhach.SelectedItem is not KhachItem k) { MessageBox.Show("Hãy chọn khách hàng."); return; }
        if (cboTour.SelectedItem is not TourItem ti || currentRate is not decimal rate) { MessageBox.Show("Chưa có tỷ giá để đặt tour."); return; }
        int n = (int)nudNguoi.Value;
        try
        {
            using var db = Db.Create();
            var t = db.Tours.Find(ti.T.MaTour)!;
            int left = Remaining(db, t);                       // kiểm tra lại lúc lưu
            if (n > left) { MessageBox.Show($"Tour chỉ còn {left} chỗ, không thể đặt {n} người.", "Không đủ chỗ", MessageBoxButtons.OK, MessageBoxIcon.Warning); _ = RefreshRateAsync(); return; }

            var d = new DatTour
            {
                MaKH = k.MaKH, MaTour = t.MaTour, SoNguoi = n, NgayDat = DateTime.Now,
                TyGia = rate,                                   // lưu tỷ giá tại thời điểm đặt -> đơn cũ không đổi khi tỷ giá thay đổi
                TongTienVND = Math.Round(t.GiaNgoaiTe * n * rate, 0)
            };
            db.DatTours.Add(d); db.SaveChanges();
            Log.Information("Đặt tour MaDat={Id}, MaTour={Tour}, {N} người, tỷ giá {Rate}, tổng {Tong} VND", d.MaDat, t.MaTour, n, rate, d.TongTienVND);
            MessageBox.Show($"Đặt tour thành công. Tổng tiền: {d.TongTienVND.ToString("N0", Vn)} đ", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadGrid(); _ = RefreshRateAsync();
        }
        catch (Exception ex) { Fail("đặt tour", ex); }
    }

    void LoadGrid()
    {
        try
        {
            using var db = Db.Create();
            var q = db.DatTours.AsNoTracking().AsQueryable();
            var kw = txtTim.Text.Trim();
            if (kw != "") q = q.Where(d => d.MaKHNavigation.HoTen.Contains(kw) || d.MaTourNavigation.TenTour.Contains(kw));
            if (cboThang.SelectedIndex > 0) { int m = cboThang.SelectedIndex; q = q.Where(d => d.MaTourNavigation.NgayKhoiHanh.Month == m); }

            // JOIN DatTour - KhachHang - Tour
            var rows = q.OrderByDescending(d => d.NgayDat).Select(d => new
            {
                d.MaDat, KhachHang = d.MaKHNavigation.HoTen, Tour = d.MaTourNavigation.TenTour,
                d.SoNguoi, d.TyGia, d.TongTienVND, d.NgayDat
            }).ToList();
            dgv.DataSource = rows;
            var c = dgv.Columns;
            c["MaDat"]!.HeaderText = "Mã"; c["MaDat"]!.FillWeight = 30; c["KhachHang"]!.HeaderText = "Khách hàng";
            c["Tour"]!.HeaderText = "Tour"; c["SoNguoi"]!.HeaderText = "Số người"; c["SoNguoi"]!.FillWeight = 40;
            c["TyGia"]!.HeaderText = "Tỷ giá"; c["TyGia"]!.DefaultCellStyle.Format = "N2"; c["TyGia"]!.DefaultCellStyle.FormatProvider = Vn;
            c["TongTienVND"]!.HeaderText = "Tổng tiền (VND)"; c["TongTienVND"]!.DefaultCellStyle.Format = "N0"; c["TongTienVND"]!.DefaultCellStyle.FormatProvider = Vn;
            c["NgayDat"]!.HeaderText = "Ngày đặt"; c["NgayDat"]!.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            lblDoanhThu.Text = $"Tổng doanh thu: {rows.Sum(r => r.TongTienVND).ToString("N0", Vn)} đ";
        }
        catch (Exception ex) { Fail("tải danh sách đặt tour", ex); }
    }
}
