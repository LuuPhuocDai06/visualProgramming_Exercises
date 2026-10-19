using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using TraSua.Models;

namespace TraSua;

public class FormThucDon : Form
{
    static readonly CultureInfo Vn = new("vi-VN");
    readonly TextBox txtTen = new() { MaxLength = 100 };
    readonly ComboBox cboDM = new() { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "TenDM", ValueMember = "MaDM" };
    readonly NumericUpDown nudGia = new() { Maximum = 10_000_000, Increment = 1000, ThousandsSeparator = true };
    readonly CheckBox chkBan = new() { Text = "Đang bán", Checked = true, AutoSize = true };
    readonly TextBox txtTim = new() { PlaceholderText = "Tìm theo tên món..." };
    readonly ComboBox cboLoc = new() { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "TenDM", ValueMember = "MaDM" };
    readonly DataGridView dgv = new();
    int? selectedId; bool loadingCats;

    public FormThucDon()
    {
        var gb = new GroupBox { Text = "Món", Bounds = new Rectangle(10, 8, 430, 190) };
        gb.Controls.AddRange(new Control[] {
            Lb("Tên món", 15, 28), At(txtTen, 100, 25, 310), Lb("Danh mục", 15, 62), At(cboDM, 100, 59, 310),
            Lb("Giá bán", 15, 96), At(nudGia, 100, 93, 310), At(chkBan, 100, 128, 150) });
        var btns = new[] { Btn("Thêm", 15, Them), Btn("Sửa", 110, Sua), Btn("Xóa", 205, Xoa), Btn("Làm mới", 300, LamMoi) };
        foreach (var b in btns) { b.Top = 150; gb.Controls.Add(b); }

        var gbTim = new GroupBox { Text = "Tìm kiếm", Bounds = new Rectangle(450, 8, 430, 190) };
        var btnTim = new Button { Text = "Tìm", Bounds = new Rectangle(330, 100, 80, 28) }; btnTim.Click += (_, _) => Reload();
        gbTim.Controls.AddRange(new Control[] { Lb("Tên món", 15, 28), At(txtTim, 100, 25, 310), Lb("Danh mục", 15, 62), At(cboLoc, 100, 59, 310), btnTim });

        dgv.Bounds = new Rectangle(10, 210, 870, 310);
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv.ReadOnly = true; dgv.AllowUserToAddRows = false; dgv.MultiSelect = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.CellClick += Dgv_CellClick;
        Controls.AddRange(new Control[] { gb, gbTim, dgv });
        Load += (_, _) => { LoadCategories(); Reload(); };
    }

    static Label Lb(string t, int x, int y) => new() { Text = t, Left = x, Top = y + 3, AutoSize = true };
    static T At<T>(T c, int x, int y, int w) where T : Control { c.Left = x; c.Top = y; c.Width = w; return c; }
    static Button Btn(string t, int x, Action a) { var b = new Button { Text = t, Left = x, Width = 90, Height = 28 }; b.Click += (_, _) => a(); return b; }

    void LoadCategories()
    {
        try
        {
            loadingCats = true;
            using var db = Db.Create();
            var cats = db.DanhMucMons.AsNoTracking().OrderBy(d => d.MaDM).ToList();
            var keep = cboDM.SelectedValue;
            cboDM.DataSource = cats.ToList();
            var filter = new List<DanhMucMon> { new() { MaDM = 0, TenDM = "Tất cả" } }; filter.AddRange(cats);
            cboLoc.DataSource = filter;
            if (keep is int k) cboDM.SelectedValue = k;
        }
        catch (Exception ex) { Fail("tải danh mục", ex); }
        finally { loadingCats = false; }
    }

    public void Reload()
    {
        if (loadingCats) return;
        try
        {
            using var db = Db.Create();
            var q = db.MonUongs.AsNoTracking().AsQueryable();
            var kw = txtTim.Text.Trim();
            if (kw != "") q = q.Where(m => m.TenMon.Contains(kw));
            if (cboLoc.SelectedValue is int dm && dm != 0) q = q.Where(m => m.MaDM == dm);

            // JOIN MonUong - DanhMucMon
            dgv.DataSource = q.OrderBy(m => m.MaDMNavigation.TenDM).ThenBy(m => m.TenMon)
                .Select(m => new { m.MaMon, m.TenMon, m.MaDM, TenDM = m.MaDMNavigation.TenDM, m.GiaBan, m.DangBan }).ToList();
            dgv.Columns["MaMon"]!.HeaderText = "Mã"; dgv.Columns["MaMon"]!.FillWeight = 30;
            dgv.Columns["MaDM"]!.Visible = false;
            dgv.Columns["TenMon"]!.HeaderText = "Tên món"; dgv.Columns["TenDM"]!.HeaderText = "Danh mục";
            dgv.Columns["GiaBan"]!.HeaderText = "Giá bán";
            dgv.Columns["GiaBan"]!.DefaultCellStyle.Format = "N0"; dgv.Columns["GiaBan"]!.DefaultCellStyle.FormatProvider = Vn;
            dgv.Columns["DangBan"]!.HeaderText = "Đang bán"; dgv.Columns["DangBan"]!.FillWeight = 50;
            dgv.ClearSelection(); selectedId = null;
        }
        catch (Exception ex) { Fail("tải thực đơn", ex); }
    }

    void Dgv_CellClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var r = dgv.Rows[e.RowIndex];
        selectedId = (int)r.Cells["MaMon"].Value;
        txtTen.Text = r.Cells["TenMon"].Value?.ToString();
        cboDM.SelectedValue = (int)r.Cells["MaDM"].Value;
        nudGia.Value = Math.Min(nudGia.Maximum, (decimal)r.Cells["GiaBan"].Value);
        chkBan.Checked = (bool)r.Cells["DangBan"].Value;
    }

    bool Valid()
    {
        string? err = string.IsNullOrWhiteSpace(txtTen.Text) ? "Tên món không được để trống."
            : cboDM.SelectedValue is not int ? "Hãy chọn danh mục."
            : nudGia.Value <= 0 ? "Giá bán phải lớn hơn 0." : null;
        if (err != null) MessageBox.Show(err, "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return err == null;
    }

    void Them()
    {
        if (!Valid()) return;
        try
        {
            using var db = Db.Create();
            var m = new MonUong { TenMon = txtTen.Text.Trim(), MaDM = (int)cboDM.SelectedValue!, GiaBan = nudGia.Value, DangBan = chkBan.Checked };
            db.MonUongs.Add(m); db.SaveChanges();
            Log.Information("Thêm món MaMon={Id}", m.MaMon); LamMoi();
        }
        catch (Exception ex) { Fail("thêm món", ex); }
    }

    void Sua()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn món cần sửa."); return; }
        if (!Valid()) return;
        try
        {
            using var db = Db.Create(); var m = db.MonUongs.Find(selectedId);
            if (m != null) { m.TenMon = txtTen.Text.Trim(); m.MaDM = (int)cboDM.SelectedValue!; m.GiaBan = nudGia.Value; m.DangBan = chkBan.Checked; db.SaveChanges(); }
            Log.Information("Sửa món MaMon={Id}", selectedId); LamMoi();
        }
        catch (Exception ex) { Fail("sửa món", ex); }
    }

    void Xoa()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn món cần xóa."); return; }
        if (MessageBox.Show("Bạn có chắc muốn xóa món này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            using var db = Db.Create();
            if (db.ChiTietHoaDons.Any(c => c.MaMon == selectedId))
            {
                MessageBox.Show("Món này đã có trong hóa đơn nên không thể xóa.\nHãy bỏ chọn \"Đang bán\" để ngừng bán món.", "Không thể xóa");
                return;
            }
            var m = db.MonUongs.Find(selectedId);
            if (m != null) { db.MonUongs.Remove(m); db.SaveChanges(); }
            Log.Information("Xóa món MaMon={Id}", selectedId); LamMoi();
        }
        catch (Exception ex) { Fail("xóa món", ex); }
    }

    void LamMoi()
    {
        selectedId = null; txtTen.Clear(); nudGia.Value = 0; chkBan.Checked = true;
        txtTim.Clear(); if (cboLoc.Items.Count > 0) cboLoc.SelectedIndex = 0;
        Reload();
    }

    static void Fail(string action, Exception ex)
    {
        Log.Error(ex, "Lỗi CSDL khi {Action}", action);
        MessageBox.Show($"Không thể {action} lúc này. Vui lòng thử lại.", "Có lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
