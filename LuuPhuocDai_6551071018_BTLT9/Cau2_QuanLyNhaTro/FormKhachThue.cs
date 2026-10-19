using Microsoft.EntityFrameworkCore;
using NhaTro.Models;
using Serilog;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NhaTro;

public class FormKhachThue : Form
{
    // true: cho ở ghép (phòng "Đang thuê" vẫn được chọn). false: khi thêm mới chỉ liệt kê phòng "Trống".
    const bool AllowShareRoom = true;

    record RoomItem(int MaPhong, string Text) { public override string ToString() => Text; }

    // --- nhập liệu
    readonly TextBox txtHoTen = new();
    readonly RadioButton rdoNam = new() { Text = "Nam", Checked = true, AutoSize = true };
    readonly RadioButton rdoNu = new() { Text = "Nữ", AutoSize = true };
    readonly DateTimePicker dtpNgaySinh = new() { Format = DateTimePickerFormat.Short };
    readonly TextBox txtSdt = new() { MaxLength = 15 };
    readonly ComboBox cboPhong = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly ComboBox cboTinh = new() { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "name", ValueMember = "code" };
    readonly ComboBox cboXa = new() { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "name", ValueMember = "code" };
    readonly CheckBox chkTamTru = new() { Text = "Đã đăng ký tạm trú", AutoSize = true };
    // --- tìm kiếm
    readonly TextBox txtTim = new() { PlaceholderText = "Tìm theo tên hoặc SĐT" };
    readonly ComboBox cboLocPhong = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly ComboBox cboLocTinh = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly DataGridView dgv = new();
    readonly StatusStrip status = new();
    readonly ToolStripStatusLabel lblStatus = new() { Text = "Sẵn sàng" };
    readonly Button btnLoi = new() { Text = "Gây lỗi thử", Visible = false, BackColor = Color.MistyRose };

    bool apiOk, suppressTinh;
    int? selectedId;

    public FormKhachThue()
    {
        Text = "Quản lý nhà trọ - Khách thuê";
        ClientSize = new Size(1000, 620); MinimumSize = new Size(1000, 620);
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;
        KeyDown += (_, e) => { if (e.Control && e.Shift && e.KeyCode == Keys.E) btnLoi.Visible = !btnLoi.Visible; };

        var gbInfo = new GroupBox { Text = "Thông tin khách thuê", Bounds = new Rectangle(10, 8, 380, 530) };
        var gbGt = new GroupBox { Bounds = new Rectangle(115, 52, 200, 34) };
        rdoNam.Location = new Point(10, 10); rdoNu.Location = new Point(90, 10);
        gbGt.Controls.AddRange(new Control[] { rdoNam, rdoNu });
        gbInfo.Controls.AddRange(new Control[] {
            Lb("Họ tên", 15, 28), At(txtHoTen, 115, 25, 245), gbGt,
            Lb("Ngày sinh", 15, 100), At(dtpNgaySinh, 115, 97, 245),
            Lb("Số điện thoại", 15, 135), At(txtSdt, 115, 132, 245),
            Lb("Phòng", 15, 170), At(cboPhong, 115, 167, 245),
            Lb("Tỉnh/Thành", 15, 205), At(cboTinh, 115, 202, 245),
            Lb("Phường/Xã", 15, 240), At(cboXa, 115, 237, 245),
            At(chkTamTru, 115, 275, 245) });
        var btnThem = Btn("Thêm", 15, Them); var btnSua = Btn("Sửa", 100, Sua);
        var btnXoa = Btn("Xóa", 185, Xoa); var btnMoi = Btn("Làm mới", 270, LamMoi);
        foreach (var b in new[] { btnThem, btnSua, btnXoa, btnMoi }) { b.Top = 330; gbInfo.Controls.Add(b); }
        btnLoi.Bounds = new Rectangle(15, 480, 120, 28);
        btnLoi.Click += (_, _) => throw new InvalidOperationException("Lỗi thử nghiệm do người dùng bấm 'Gây lỗi thử'");
        gbInfo.Controls.Add(btnLoi);

        var btnTim = new Button { Text = "Tìm", Bounds = new Rectangle(530, 10, 60, 27) };
        var btnBoLoc = new Button { Text = "Bỏ lọc", Bounds = new Rectangle(600, 10, 70, 27) };
        btnTim.Click += (_, _) => LoadGrid(); btnBoLoc.Click += (_, _) => BoLoc();
        var pnlTim = new Panel { Bounds = new Rectangle(400, 8, 590, 70) };
        pnlTim.Controls.AddRange(new Control[] {
            At(txtTim, 0, 10, 180),
            Lb("Phòng:", 190, 10), At(cboLocPhong, 240, 10, 110),
            Lb("Tỉnh:", 360, 10),  At(cboLocTinh, 400, 10, 120) });
        btnTim.Left = 0; btnTim.Top = 40; btnBoLoc.Left = 70; btnBoLoc.Top = 40;
        pnlTim.Controls.AddRange(new Control[] { btnTim, btnBoLoc });

        dgv.Bounds = new Rectangle(400, 82, 590, 456);
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv.ReadOnly = true; dgv.AllowUserToAddRows = false; dgv.MultiSelect = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.CellClick += Dgv_CellClick;

        status.Items.Add(lblStatus);
        Controls.AddRange(new Control[] { gbInfo, pnlTim, dgv, status });
        cboTinh.SelectedIndexChanged += async (_, _) => { if (!suppressTinh) await LoadWardsAsync(); };

        Load += async (_, _) =>
        {
            ReloadRooms(null); LoadFilters(); LoadGrid();
            await LoadProvincesAsync();
        };
    }

    // ---------- helpers UI ----------
    static Label Lb(string t, int x, int y) => new() { Text = t, Left = x, Top = y + 3, AutoSize = true };
    static T At<T>(T c, int x, int y, int w) where T : Control { c.Left = x; c.Top = y; c.Width = w; return c; }
    static Button Btn(string t, int x, Action a)
    { var b = new Button { Text = t, Left = x, Width = 80, Height = 28 }; b.Click += (_, _) => a(); return b; }

    // ---------- API Tỉnh / Phường-Xã ----------
    async Task LoadProvincesAsync()
    {
        cboTinh.Enabled = false;
        cboTinh.DataSource = new List<Province> { new() { code = 0, name = "Đang tải…" } };
        lblStatus.Text = "Đang tải danh sách tỉnh/thành…";
        try
        {
            var ps = await AddressApi.GetProvincesAsync();
            suppressTinh = true;
            cboTinh.DataSource = ps; cboTinh.SelectedIndex = -1;
            suppressTinh = false;
            apiOk = true; cboTinh.Enabled = true;
            lblStatus.Text = $"Đã tải {ps.Count} tỉnh/thành từ provinces.open-api.vn";
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            apiOk = false;
            Log.Warning(ex, "Không tải được danh sách tỉnh/thành từ API");
            suppressTinh = true; cboTinh.DataSource = null; suppressTinh = false;
            EnableManualEntry(cboTinh); EnableManualEntry(cboXa);
            lblStatus.Text = "Không có mạng - nhập tay Tỉnh/Thành và Phường/Xã";
            MessageBox.Show("Không tải được danh sách địa chỉ, vui lòng kiểm tra mạng.\nBạn vẫn có thể gõ tay Tỉnh/Thành và Phường/Xã để lưu khách.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    static void EnableManualEntry(ComboBox c) { c.DropDownStyle = ComboBoxStyle.DropDown; c.Enabled = true; c.Text = ""; }

    async Task LoadWardsAsync()
    {
        if (!apiOk || cboTinh.SelectedItem is not Province p || p.code == 0) { cboXa.DataSource = null; return; }
        int want = p.code;
        cboXa.DropDownStyle = ComboBoxStyle.DropDownList;
        cboXa.Enabled = false;
        cboXa.DataSource = new List<Ward> { new() { code = 0, name = "Đang tải…" } };
        try
        {
            var d = await AddressApi.GetWardsAsync(want);
            if (cboTinh.SelectedItem is Province cur && cur.code == want)   // bỏ kết quả cũ nếu người dùng đã đổi tỉnh
            {
                cboXa.DataSource = d.wards; cboXa.SelectedIndex = -1; cboXa.Enabled = true;
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            Log.Warning(ex, "Không tải được phường/xã của tỉnh {Code}", want);
            cboXa.DataSource = null; EnableManualEntry(cboXa);
            MessageBox.Show("Không tải được danh sách địa chỉ, vui lòng kiểm tra mạng.\nBạn có thể gõ tay Phường/Xã.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    // ---------- nạp dữ liệu ----------
    void ReloadRooms(int? currentRoom)
    {
        try
        {
            using var db = Db.Create();
            var rooms = db.PhongTros.AsNoTracking().OrderBy(r => r.TenPhong).ToList()
                .Where(r => AllowShareRoom || r.TinhTrang == "Trống" || r.MaPhong == currentRoom)
                .Select(r => new RoomItem(r.MaPhong, $"{r.TenPhong} - {r.GiaThue:N0} đ ({r.TinhTrang})")).ToList();
            cboPhong.DataSource = rooms; cboPhong.SelectedIndex = rooms.Count > 0 ? 0 : -1;
            if (currentRoom != null) cboPhong.SelectedItem = rooms.FirstOrDefault(r => r.MaPhong == currentRoom);
        }
        catch (Exception ex) { Fail("tải danh sách phòng", ex); }
    }

    void LoadFilters()
    {
        try
        {
            using var db = Db.Create();
            var phong = new List<RoomItem> { new(0, "Tất cả") };
            phong.AddRange(db.PhongTros.AsNoTracking().OrderBy(r => r.TenPhong).ToList().Select(r => new RoomItem(r.MaPhong, r.TenPhong)));
            cboLocPhong.DataSource = phong;
            var tinh = new List<string> { "Tất cả" };
            tinh.AddRange(db.KhachThues.Where(k => k.TenTinh != null).Select(k => k.TenTinh!).Distinct().OrderBy(s => s).ToList());
            cboLocTinh.DataSource = tinh;
        }
        catch (Exception ex) { Fail("tải bộ lọc", ex); }
    }

    void LoadGrid()
    {
        try
        {
            using var db = Db.Create();
            var q = db.KhachThues.AsNoTracking().AsQueryable();
            var kw = txtTim.Text.Trim();
            if (kw != "") q = q.Where(k => k.HoTen.Contains(kw) || k.SoDienThoai.Contains(kw));
            if (cboLocPhong.SelectedItem is RoomItem ri && ri.MaPhong != 0) q = q.Where(k => k.MaPhong == ri.MaPhong);
            if (cboLocTinh.SelectedIndex > 0) { var t = (string)cboLocTinh.SelectedItem!; q = q.Where(k => k.TenTinh == t); }

            // JOIN KhachThue - PhongTro qua navigation property
            var rows = q.OrderBy(k => k.MaKhach).Select(k => new
            {
                k.MaKhach, k.HoTen, k.SoDienThoai,
                k.MaPhongNavigation.TenPhong, k.MaPhongNavigation.GiaThue,
                k.TenPhuongXa, k.TenTinh, k.DaDangKyTamTru
            }).ToList()
            .Select(k => new
            {
                k.MaKhach,
                HoTen = k.HoTen, SDT = k.SoDienThoai, Phong = k.TenPhong, GiaThue = k.GiaThue,
                QueQuan = string.Join(" - ", new[] { k.TenPhuongXa, k.TenTinh }.Where(s => !string.IsNullOrWhiteSpace(s))),
                TamTru = k.DaDangKyTamTru
            }).ToList();

            dgv.DataSource = rows;
            dgv.Columns["MaKhach"]!.Visible = false;
            dgv.Columns["HoTen"]!.HeaderText = "Họ tên"; dgv.Columns["SDT"]!.HeaderText = "SĐT";
            dgv.Columns["Phong"]!.HeaderText = "Phòng"; dgv.Columns["GiaThue"]!.HeaderText = "Giá thuê";
            dgv.Columns["GiaThue"]!.DefaultCellStyle.Format = "N0";
            dgv.Columns["QueQuan"]!.HeaderText = "Quê quán"; dgv.Columns["TamTru"]!.HeaderText = "Tạm trú";
            dgv.ClearSelection(); selectedId = null;
        }
        catch (Exception ex) { Fail("tải danh sách khách", ex); }
    }

    void BoLoc()
    {
        txtTim.Clear(); cboLocPhong.SelectedIndex = 0; cboLocTinh.SelectedIndex = 0; LoadGrid();
    }

    // ---------- chọn dòng ----------
    async void Dgv_CellClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        try
        {
            int id = (int)dgv.Rows[e.RowIndex].Cells["MaKhach"].Value;
            KhachThue? k;
            using (var db = Db.Create()) k = db.KhachThues.AsNoTracking().FirstOrDefault(x => x.MaKhach == id);
            if (k == null) return;
            selectedId = id;
            txtHoTen.Text = k.HoTen; (k.GioiTinh == "Nữ" ? rdoNu : rdoNam).Checked = true;
            dtpNgaySinh.Value = k.NgaySinh.ToDateTime(TimeOnly.MinValue);
            txtSdt.Text = k.SoDienThoai; chkTamTru.Checked = k.DaDangKyTamTru;
            ReloadRooms(k.MaPhong);

            if (apiOk)
            {
                suppressTinh = true;
                if (k.MaTinh != null) cboTinh.SelectedValue = k.MaTinh.Value;
                else cboTinh.SelectedIndex = -1;
                suppressTinh = false;
                await LoadWardsAsync();
                if (apiOk && cboXa.DataSource is List<Ward> && k.MaPhuongXa != null) cboXa.SelectedValue = k.MaPhuongXa.Value;
                else cboXa.Text = k.TenPhuongXa ?? "";
            }
            else { cboTinh.Text = k.TenTinh ?? ""; cboXa.Text = k.TenPhuongXa ?? ""; }
        }
        catch (Exception ex) { Fail("hiển thị thông tin khách", ex); }
    }

    // ---------- CRUD ----------
    bool ValidateInput(out string tenTinh, out int? maTinh, out string tenXa, out int? maXa)
    {
        tenTinh = tenXa = ""; maTinh = maXa = null;
        if (string.IsNullOrWhiteSpace(txtHoTen.Text)) return Warn("Họ tên không được để trống.", txtHoTen);
        if (dtpNgaySinh.Value.Date >= DateTime.Today) return Warn("Ngày sinh không hợp lệ.", dtpNgaySinh);
        if (!Regex.IsMatch(txtSdt.Text.Trim(), @"^0\d{9,10}$")) return Warn("Số điện thoại phải gồm 10-11 chữ số và bắt đầu bằng 0.", txtSdt);
        if (cboPhong.SelectedItem is not RoomItem) return Warn("Hãy chọn phòng.", cboPhong);

        if (cboTinh.SelectedItem is Province p && p.code != 0) { tenTinh = p.name; maTinh = p.code; }
        else tenTinh = cboTinh.Text.Trim();
        if (cboXa.SelectedItem is Ward w && w.code != 0) { tenXa = w.name; maXa = w.code; }
        else tenXa = cboXa.Text.Trim();
        if (tenTinh == "") return Warn("Hãy chọn (hoặc nhập) Tỉnh/Thành.", cboTinh);
        if (tenXa == "") return Warn("Hãy chọn (hoặc nhập) Phường/Xã.", cboXa);
        return true;
    }
    static bool Warn(string msg, Control c)
    { MessageBox.Show(msg, "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); c.Focus(); return false; }

    static void SyncRoom(QlNhaTroContext db, int maPhong)
    {
        var p = db.PhongTros.Find(maPhong);
        if (p != null) p.TinhTrang = db.KhachThues.Any(k => k.MaPhong == maPhong) ? "Đang thuê" : "Trống";
    }

    void Fill(KhachThue k, string tenTinh, int? maTinh, string tenXa, int? maXa)
    {
        k.HoTen = txtHoTen.Text.Trim(); k.GioiTinh = rdoNu.Checked ? "Nữ" : "Nam";
        k.NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value); k.SoDienThoai = txtSdt.Text.Trim();
        k.MaPhong = ((RoomItem)cboPhong.SelectedItem!).MaPhong; k.DaDangKyTamTru = chkTamTru.Checked;
        k.TenTinh = tenTinh; k.MaTinh = maTinh; k.TenPhuongXa = tenXa; k.MaPhuongXa = maXa;   // lưu cả mã và tên
    }

    void Them()
    {
        if (!ValidateInput(out var tt, out var mt, out var tx, out var mx)) return;
        try
        {
            using var db = Db.Create();
            using var tx2 = db.Database.BeginTransaction();
            var k = new KhachThue(); Fill(k, tt, mt, tx, mx);
            db.KhachThues.Add(k); db.SaveChanges();
            SyncRoom(db, k.MaPhong); db.SaveChanges();
            tx2.Commit();
            Log.Information("Thêm khách MaKhach={Id} vào phòng {Room}", k.MaKhach, k.MaPhong);
            LamMoi();
        }
        catch (Exception ex) { Fail("thêm khách", ex); }
    }

    void Sua()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn khách cần sửa."); return; }
        if (!ValidateInput(out var tt, out var mt, out var tx, out var mx)) return;
        try
        {
            using var db = Db.Create();
            using var tr = db.Database.BeginTransaction();
            var k = db.KhachThues.Find(selectedId);
            if (k == null) { MessageBox.Show("Khách này không còn tồn tại."); LamMoi(); return; }
            int oldRoom = k.MaPhong;
            Fill(k, tt, mt, tx, mx); db.SaveChanges();
            SyncRoom(db, oldRoom); SyncRoom(db, k.MaPhong); db.SaveChanges();
            tr.Commit();
            Log.Information("Sửa khách MaKhach={Id}", k.MaKhach);
            LamMoi();
        }
        catch (Exception ex) { Fail("sửa thông tin khách", ex); }
    }

    void Xoa()
    {
        if (selectedId == null) { MessageBox.Show("Hãy chọn khách cần xóa."); return; }
        if (MessageBox.Show("Bạn có chắc muốn xóa khách này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            using var db = Db.Create();
            using var tr = db.Database.BeginTransaction();
            var k = db.KhachThues.Find(selectedId);
            if (k != null)
            {
                int room = k.MaPhong;
                db.KhachThues.Remove(k); db.SaveChanges();
                SyncRoom(db, room); db.SaveChanges();     // người cuối cùng rời đi -> phòng "Trống"
            }
            tr.Commit();
            Log.Information("Xóa khách MaKhach={Id}", selectedId);
            LamMoi();
        }
        catch (Exception ex) { Fail("xóa khách", ex); }
    }

    void LamMoi()
    {
        selectedId = null;
        txtHoTen.Clear(); rdoNam.Checked = true; dtpNgaySinh.Value = DateTime.Today.AddYears(-20);
        txtSdt.Clear(); chkTamTru.Checked = false;
        if (apiOk) { cboTinh.SelectedIndex = -1; cboXa.DataSource = null; } else { cboTinh.Text = ""; cboXa.Text = ""; }
        ReloadRooms(null); LoadFilters(); LoadGrid();
    }

    static void Fail(string action, Exception ex)
    {
        Log.Error(ex, "Lỗi CSDL khi {Action}", action);
        MessageBox.Show($"Không thể {action} lúc này. Vui lòng thử lại.", "Có lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
