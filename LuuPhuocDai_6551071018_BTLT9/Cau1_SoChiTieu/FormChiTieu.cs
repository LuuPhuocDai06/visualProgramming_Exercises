using Microsoft.EntityFrameworkCore;
using Serilog;
using SoChiTieu.Models;
using System.Globalization;

namespace SoChiTieu;

public class FormChiTieu : Form
{
    static readonly string[] Loai = { "Ăn uống", "Đi lại", "Học tập", "Giải trí", "Khác" };
    static readonly CultureInfo Vn = new("vi-VN");

    // --- thông tin khoản chi
    readonly DateTimePicker dtpNgay = new() { Format = DateTimePickerFormat.Short };
    readonly TextBox txtNoiDung = new() { MaxLength = 100 };
    readonly ComboBox cboLoai = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly NumericUpDown nudTien = new() { Maximum = 1_000_000_000, Increment = 1000, ThousandsSeparator = true };
    readonly TextBox txtGhiChu = new() { Multiline = true, MaxLength = 200, ScrollBars = ScrollBars.Vertical };
    // --- tìm kiếm
    readonly TextBox txtTim = new() { PlaceholderText = "Nhập nội dung..." };
    readonly ComboBox cboLoc = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly DateTimePicker dtpTu = new() { Format = DateTimePickerFormat.Short };
    readonly DateTimePicker dtpDen = new() { Format = DateTimePickerFormat.Short };
    // --- còn lại
    readonly DataGridView dgv = new();
    readonly Label lblTong = new() { AutoSize = false, TextAlign = ContentAlignment.MiddleRight,
        Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.Red };
    int? selectedId;

    public FormChiTieu()
    {
        Text = "Sổ chi tiêu cá nhân";
        ClientSize = new Size(900, 650);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 650);

        cboLoai.Items.AddRange(Loai); cboLoai.SelectedIndex = 0;
        cboLoc.Items.Add("Tất cả"); cboLoc.Items.AddRange(Loai); cboLoc.SelectedIndex = 0;

        var gbInfo = new GroupBox { Text = "Thông tin khoản chi", Bounds = new Rectangle(10, 8, 500, 210) };
        gbInfo.Controls.AddRange(new Control[] {
            Lb("Ngày chi", 15, 28),   At(dtpNgay, 95, 25, 150),
            Lb("Nội dung", 15, 60),   At(txtNoiDung, 95, 57, 390),
            Lb("Loại chi", 15, 92),   At(cboLoai, 95, 89, 150),
            Lb("Số tiền", 15, 124),   At(nudTien, 95, 121, 150),
            Lb("Ghi chú", 15, 156),   At(txtGhiChu, 95, 153, 390) });
        txtGhiChu.Height = 48;

        var gbTim = new GroupBox { Text = "Tìm kiếm", Bounds = new Rectangle(520, 8, 370, 210) };
        var btnTim = new Button { Text = "Tìm", Bounds = new Rectangle(270, 165, 80, 28) };
        btnTim.Click += (_, _) => Reload();
        gbTim.Controls.AddRange(new Control[] {
            Lb("Nội dung", 15, 28), At(txtTim, 95, 25, 255),
            Lb("Loại chi", 15, 62), At(cboLoc, 95, 59, 255),
            Lb("Từ ngày", 15, 96),  At(dtpTu, 95, 93, 150),
            Lb("Đến ngày", 15, 130),At(dtpDen, 95, 127, 150), btnTim });

        var btnThem = Btn("Thêm", 150, Them); var btnSua = Btn("Sửa", 250, Sua);
        var btnXoa = Btn("Xóa", 350, Xoa);    var btnMoi = Btn("Làm mới", 450, LamMoi);

        dgv.Bounds = new Rectangle(10, 262, 880, 340);
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv.ReadOnly = true; dgv.AllowUserToAddRows = false; dgv.AllowUserToDeleteRows = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgv.MultiSelect = false;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgv.RowHeadersVisible = true;
        dgv.CellClick += Dgv_CellClick;

        lblTong.Bounds = new Rectangle(10, 610, 880, 30);
        lblTong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        Controls.AddRange(new Control[] { gbInfo, gbTim, btnThem, btnSua, btnXoa, btnMoi, dgv, lblTong });

        ResetFilter();
        Load += (_, _) => Reload();
    }

    // ---------- helpers dựng giao diện ----------
    static Label Lb(string t, int x, int y) => new() { Text = t, Left = x, Top = y + 3, AutoSize = true };
    static T At<T>(T c, int x, int y, int w) where T : Control { c.Left = x; c.Top = y; c.Width = w; return c; }
    static Button Btn(string t, int x, Action a)
    {
        var b = new Button { Text = t, Bounds = new Rectangle(x, 225, 90, 30) };
        b.Click += (_, _) => a();
        return b;
    }

    // ---------- nghiệp vụ ----------
    void ResetFilter()
    {
        txtTim.Clear(); cboLoc.SelectedIndex = 0;
        dtpTu.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpDen.Value = DateTime.Today;
    }

    void Reload()
    {
        try
        {
            using var db = Db.Create();
            IQueryable<ChiTieu> q = db.ChiTieus.AsNoTracking();
            var kw = txtTim.Text.Trim();
            if (kw != "") q = q.Where(x => x.NoiDung.Contains(kw));
            if (cboLoc.SelectedIndex > 0) { var l = (string)cboLoc.SelectedItem!; q = q.Where(x => x.LoaiChi == l); }
            var tu = DateOnly.FromDateTime(dtpTu.Value); var den = DateOnly.FromDateTime(dtpDen.Value);
            q = q.Where(x => x.NgayChi >= tu && x.NgayChi <= den);

            var list = q.OrderByDescending(x => x.NgayChi).ThenByDescending(x => x.MaCT).ToList();
            dgv.DataSource = list.Select(x => new
            {
                x.MaCT,
                NgayChi = x.NgayChi.ToDateTime(TimeOnly.MinValue),
                x.NoiDung, x.LoaiChi, x.SoTien, x.GhiChu
            }).ToList();
            FormatGrid();
            selectedId = null; dgv.ClearSelection();
            lblTong.Text = $"Tổng chi: {list.Sum(x => x.SoTien).ToString("N0", Vn)} đ";
        }
        catch (Exception ex) { Fail("tải danh sách chi tiêu", ex); }
    }

    void FormatGrid()
    {
        dgv.Columns["MaCT"]!.HeaderText = "Mã";
        dgv.Columns["NgayChi"]!.HeaderText = "Ngày chi";
        dgv.Columns["NgayChi"]!.DefaultCellStyle.Format = "dd/MM/yyyy";
        dgv.Columns["NoiDung"]!.HeaderText = "Nội dung";
        dgv.Columns["LoaiChi"]!.HeaderText = "Loại chi";
        dgv.Columns["SoTien"]!.HeaderText = "Số tiền";
        dgv.Columns["SoTien"]!.DefaultCellStyle.Format = "N0";
        dgv.Columns["SoTien"]!.DefaultCellStyle.FormatProvider = Vn;
        dgv.Columns["SoTien"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgv.Columns["GhiChu"]!.HeaderText = "Ghi chú";
        dgv.Columns["MaCT"]!.FillWeight = 40;
    }

    void Dgv_CellClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var r = dgv.Rows[e.RowIndex];
        selectedId = (int)r.Cells["MaCT"].Value;
        dtpNgay.Value = (DateTime)r.Cells["NgayChi"].Value;
        txtNoiDung.Text = r.Cells["NoiDung"].Value?.ToString();
        cboLoai.SelectedItem = r.Cells["LoaiChi"].Value?.ToString();
        nudTien.Value = Math.Min(nudTien.Maximum, (decimal)r.Cells["SoTien"].Value);
        txtGhiChu.Text = r.Cells["GhiChu"].Value?.ToString();
    }

    bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtNoiDung.Text)) return Warn("Nội dung không được để trống.", txtNoiDung);
        if (nudTien.Value <= 0) return Warn("Số tiền phải lớn hơn 0.", nudTien);
        if (dtpNgay.Value.Date > DateTime.Today) return Warn("Ngày chi không được lớn hơn hôm nay.", dtpNgay);
        return true;
    }
    static bool Warn(string msg, Control c)
    {
        MessageBox.Show(msg, "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        c.Focus(); return false;
    }

    void Them()
    {
        if (!ValidateInput()) return;
        try
        {
            using var db = Db.Create();
            var ct = new ChiTieu
            {
                NgayChi = DateOnly.FromDateTime(dtpNgay.Value), NoiDung = txtNoiDung.Text.Trim(),
                LoaiChi = (string)cboLoai.SelectedItem!, SoTien = nudTien.Value,
                GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
            };
            db.ChiTieus.Add(ct);
            db.SaveChanges();
            Log.Information("Thêm khoản chi MaCT={MaCT}, SoTien={SoTien}", ct.MaCT, ct.SoTien);
            ClearInput(); Reload();
        }
        catch (Exception ex) { Fail("thêm khoản chi", ex); }
    }

    void Sua()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn một dòng cần sửa."); return; }
        if (!ValidateInput()) return;
        try
        {
            using var db = Db.Create();
            var ct = db.ChiTieus.Find(selectedId);
            if (ct == null) { MessageBox.Show("Khoản chi này không còn tồn tại."); Reload(); return; }
            ct.NgayChi = DateOnly.FromDateTime(dtpNgay.Value); ct.NoiDung = txtNoiDung.Text.Trim();
            ct.LoaiChi = (string)cboLoai.SelectedItem!; ct.SoTien = nudTien.Value;
            ct.GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim();
            db.SaveChanges();
            Log.Information("Sửa khoản chi MaCT={MaCT}", ct.MaCT);
            Reload();
        }
        catch (Exception ex) { Fail("sửa khoản chi", ex); }
    }

    void Xoa()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn một dòng cần xóa."); return; }
        if (MessageBox.Show("Bạn có chắc muốn xóa khoản chi này?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            using var db = Db.Create();
            var ct = db.ChiTieus.Find(selectedId);
            if (ct != null) { db.ChiTieus.Remove(ct); db.SaveChanges(); }
            Log.Information("Xóa khoản chi MaCT={MaCT}", selectedId);
            ClearInput(); Reload();
        }
        catch (Exception ex) { Fail("xóa khoản chi", ex); }
    }

    void LamMoi() { ClearInput(); ResetFilter(); Reload(); }

    void ClearInput()
    {
        selectedId = null; dtpNgay.Value = DateTime.Today; txtNoiDung.Clear();
        cboLoai.SelectedIndex = 0; nudTien.Value = 0; txtGhiChu.Clear();
    }

    static void Fail(string action, Exception ex)
    {
        Log.Error(ex, "Lỗi truy cập CSDL khi {Action}", action);
        MessageBox.Show($"Không thể {action} lúc này. Vui lòng kiểm tra kết nối cơ sở dữ liệu và thử lại.",
            "Có lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
