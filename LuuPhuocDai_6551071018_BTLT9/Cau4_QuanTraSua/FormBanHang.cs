using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel;
using System.Globalization;
using TraSua.Models;

namespace TraSua;

public class CartLine
{
    public int MaMon { get; set; }
    public string TenMon { get; set; } = "";
    public int SoLy { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien => SoLy * DonGia;
}

public class FormBanHang : Form
{
    static readonly CultureInfo Vn = new("vi-VN");
    // MỘT HttpClient dùng chung
    static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };

    sealed record MonItem(int MaMon, string TenMon, decimal GiaBan)
    { public override string ToString() => $"{TenMon} - {GiaBan.ToString("N0", Vn)}"; }

    readonly ComboBox cboDM = new() { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "TenDM", ValueMember = "MaDM" };
    readonly ListBox lstMon = new();
    readonly NumericUpDown nudLy = new() { Minimum = 1, Maximum = 100, Value = 1 };
    readonly DataGridView dgvCart = new();
    readonly BindingList<CartLine> cart = new();
    readonly Label lblTong = new() { Font = new Font("Segoe UI", 13F, FontStyle.Bold), AutoSize = false, TextAlign = ContentAlignment.MiddleCenter };
    readonly Button btnQr = new() { Text = "Thanh toán chuyển khoản" };
    readonly Button btnCash = new() { Text = "Tiền mặt" };
    readonly Button btnDaNhan = new() { Text = "Đã nhận tiền", Enabled = false, BackColor = Color.ForestGreen, ForeColor = Color.White };
    readonly PictureBox pic = new() { SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
    readonly Label lblQrStatus = new() { AutoSize = false, ForeColor = Color.Firebrick };
    readonly Label lblSoTien = new() { AutoSize = false }, lblNoiDung = new() { AutoSize = false }, lblTk = new() { AutoSize = false };

    MemoryStream? qrStream;     // phải giữ stream sống suốt thời gian ảnh còn dùng
    int? qrInvoiceId;           // mã HD dự kiến in trong nội dung chuyển khoản

    decimal Total => cart.Sum(c => c.ThanhTien);

    public FormBanHang()
    {
        // ---- cột trái: chọn món
        var gbMon = new GroupBox { Text = "Chọn món", Bounds = new Rectangle(8, 5, 230, 500) };
        var btnAdd = new Button { Text = "Thêm vào đơn", Bounds = new Rectangle(15, 455, 200, 30) };
        btnAdd.Click += (_, _) => ThemVaoDon();
        lstMon.Bounds = new Rectangle(15, 80, 200, 300); lstMon.DoubleClick += (_, _) => ThemVaoDon();
        gbMon.Controls.AddRange(new Control[] {
            new Label { Text = "Danh mục:", Left = 15, Top = 25, AutoSize = true }, btnAdd });
        cboDM.SetBounds(15, 47, 200, 24); cboDM.SelectedIndexChanged += (_, _) => LoadMon();
        nudLy.SetBounds(100, 395, 115, 24);
        gbMon.Controls.AddRange(new Control[] { cboDM, lstMon, new Label { Text = "Số ly:", Left = 15, Top = 398, AutoSize = true }, nudLy });

        // ---- giữa: đơn hiện tại
        var gbDon = new GroupBox { Text = "Đơn hiện tại", Bounds = new Rectangle(245, 5, 360, 500) };
        dgvCart.SetBounds(10, 22, 340, 385);
        dgvCart.ReadOnly = true; dgvCart.AllowUserToAddRows = false; dgvCart.MultiSelect = false;
        dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCart.DataSource = cart;
        var btnXoaDong = new Button { Text = "Xóa dòng", Bounds = new Rectangle(10, 415, 340, 28) };
        btnXoaDong.Click += (_, _) => XoaDong();
        lblTong.SetBounds(10, 450, 340, 36);
        gbDon.Controls.AddRange(new Control[] { dgvCart, btnXoaDong, lblTong });

        // ---- phải: thanh toán
        var gbTT = new GroupBox { Text = "Thanh toán", Bounds = new Rectangle(612, 5, 270, 500) };
        btnQr.SetBounds(10, 22, 250, 30); btnCash.SetBounds(10, 58, 250, 28);
        btnQr.Click += BtnQr_Click; btnCash.Click += (_, _) => SaveInvoice("Tiền mặt");
        pic.SetBounds(35, 95, 200, 200);
        lblQrStatus.SetBounds(10, 298, 250, 32);
        lblSoTien.SetBounds(10, 332, 250, 20); lblNoiDung.SetBounds(10, 352, 250, 20); lblTk.SetBounds(10, 374, 250, 58);
        btnDaNhan.SetBounds(10, 450, 250, 36); btnDaNhan.Click += (_, _) => SaveInvoice("Chuyển khoản");
        gbTT.Controls.AddRange(new Control[] { btnQr, btnCash, pic, lblQrStatus, lblSoTien, lblNoiDung, lblTk, btnDaNhan });

        Controls.AddRange(new Control[] { gbMon, gbDon, gbTT });
        Load += (_, _) => { LoadCategories(); FormatCart(); UpdateTotal(); };
    }

    // ---------- nạp dữ liệu ----------
    void LoadCategories()
    {
        try
        {
            using var db = Db.Create();
            cboDM.DataSource = db.DanhMucMons.AsNoTracking().OrderBy(d => d.MaDM).ToList();
            LoadMon();
        }
        catch (Exception ex) { Fail("tải danh mục", ex); }
    }

    public void RefreshMenu() { if (cboDM.DataSource != null) LoadMon(); }

    void LoadMon()
    {
        if (cboDM.SelectedValue is not int dm) { lstMon.Items.Clear(); return; }
        try
        {
            using var db = Db.Create();
            var items = db.MonUongs.AsNoTracking().Where(m => m.MaDM == dm && m.DangBan).OrderBy(m => m.TenMon).ToList()
                .Select(m => new MonItem(m.MaMon, m.TenMon, m.GiaBan)).ToArray();
            lstMon.Items.Clear(); lstMon.Items.AddRange(items);
        }
        catch (Exception ex) { Fail("tải danh sách món", ex); }
    }

    void FormatCart()
    {
        dgvCart.Columns["MaMon"]!.Visible = false;
        dgvCart.Columns["TenMon"]!.HeaderText = "Tên món";
        dgvCart.Columns["SoLy"]!.HeaderText = "SL"; dgvCart.Columns["SoLy"]!.FillWeight = 25;
        dgvCart.Columns["DonGia"]!.HeaderText = "Đơn giá";
        dgvCart.Columns["ThanhTien"]!.HeaderText = "Thành tiền";
        foreach (var n in new[] { "DonGia", "ThanhTien" })
        { dgvCart.Columns[n]!.DefaultCellStyle.Format = "N0"; dgvCart.Columns[n]!.DefaultCellStyle.FormatProvider = Vn; }
    }

    // ---------- giỏ hàng ----------
    void ThemVaoDon()
    {
        if (lstMon.SelectedItem is not MonItem m) { MessageBox.Show("Hãy chọn một món."); return; }
        int ly = (int)nudLy.Value;
        var line = cart.FirstOrDefault(c => c.MaMon == m.MaMon);
        if (line != null) { line.SoLy += ly; cart.ResetBindings(); }              // cộng dồn nếu món đã có
        else cart.Add(new CartLine { MaMon = m.MaMon, TenMon = m.TenMon, SoLy = ly, DonGia = m.GiaBan });
        nudLy.Value = 1; UpdateTotal();
    }

    void XoaDong()
    {
        if (dgvCart.CurrentRow?.DataBoundItem is CartLine l) { cart.Remove(l); UpdateTotal(); }
    }

    void UpdateTotal()
    {
        lblTong.Text = $"Tổng tiền: {Total.ToString("N0", Vn)} đ";
        ResetQr();                                  // giỏ đổi -> mã QR cũ không còn đúng số tiền
    }

    void ResetQr()
    {
        pic.Image?.Dispose(); pic.Image = null; qrStream?.Dispose(); qrStream = null; qrInvoiceId = null;
        lblQrStatus.Text = ""; lblSoTien.Text = ""; lblNoiDung.Text = ""; lblTk.Text = "";
        btnDaNhan.Enabled = false;
    }

    // ---------- VietQR ----------
    static string Mask(string acc) => acc.Length <= 3 ? "***" : new string('*', acc.Length - 3) + acc[^3..];

    async void BtnQr_Click(object? s, EventArgs e)
    {
        if (cart.Count == 0) { MessageBox.Show("Đơn hàng đang trống."); return; }
        var cfg = AppSettings.Current.Payment;
        decimal total = Total;
        int nextId;
        try { using var db = Db.Create(); nextId = (db.HoaDons.Max(h => (int?)h.MaHD) ?? 0) + 1; }
        catch (Exception ex) { Fail("tạo mã hóa đơn", ex); return; }

        var noiDung = $"TRASUA HD{nextId}";
        qrInvoiceId = nextId;
        // luôn hiện thông tin dạng chữ để khách chuyển tay nếu QR không tải được
        lblSoTien.Text = $"Số tiền: {total.ToString("N0", Vn)} đ";
        lblNoiDung.Text = $"Nội dung: {noiDung}";
        lblTk.Text = $"Ngân hàng: {cfg.BankId}\nSTK: {cfg.AccountNo}\nChủ TK: {cfg.AccountName}";
        lblQrStatus.ForeColor = SystemColors.GrayText; lblQrStatus.Text = "Đang tạo mã QR…";
        pic.Image?.Dispose(); pic.Image = null;
        btnQr.Enabled = false; btnDaNhan.Enabled = true;      // vẫn cho xác nhận nếu khách chuyển tay

        try
        {
            var url = $"https://img.vietqr.io/image/{Uri.EscapeDataString(cfg.BankId)}-{Uri.EscapeDataString(cfg.AccountNo)}-{Uri.EscapeDataString(cfg.Template)}.png" +
                      $"?amount={((long)total).ToString(CultureInfo.InvariantCulture)}" +
                      $"&addInfo={Uri.EscapeDataString(noiDung)}&accountName={Uri.EscapeDataString(cfg.AccountName)}";
            var bytes = await Http.GetByteArrayAsync(url);
            var ms = new MemoryStream(bytes);
            var img = Image.FromStream(ms);
            qrStream?.Dispose(); qrStream = ms; pic.Image = img;
            lblQrStatus.Text = "";
            Log.Information("Tạo QR cho HD {MaHD}, số tiền {SoTien}, TK {TK}", nextId, total, Mask(cfg.AccountNo));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or ArgumentException)
        {
            Log.Error(ex, "Tải ảnh QR thất bại cho HD {MaHD}", nextId);
            lblQrStatus.ForeColor = Color.Firebrick;
            lblQrStatus.Text = "Không tải được mã QR. Khách chuyển khoản theo thông tin bên dưới, hoặc chọn \"Tiền mặt\".";
        }
        finally { btnQr.Enabled = true; }
    }

    // ---------- lưu hóa đơn (1 lần SaveChanges cho HoaDon + ChiTietHoaDon) ----------
    void SaveInvoice(string hinhThuc)
    {
        if (cart.Count == 0) { MessageBox.Show("Đơn hàng đang trống."); return; }
        try
        {
            using var db = Db.Create();
            var hd = new HoaDon { NgayLap = DateTime.Now, TongTien = Total, HinhThucTT = hinhThuc, DaThanhToan = true };
            foreach (var l in cart)
                hd.ChiTietHoaDons.Add(new ChiTietHoaDon { MaMon = l.MaMon, SoLy = l.SoLy, DonGia = l.DonGia });
            db.HoaDons.Add(hd);
            db.SaveChanges();

            Log.Information("Lưu hóa đơn MaHD={MaHD}, {HinhThuc}, tổng {Tong}", hd.MaHD, hinhThuc, hd.TongTien);
            string extra = "";
            if (hinhThuc == "Chuyển khoản" && qrInvoiceId != null && qrInvoiceId != hd.MaHD)
            {
                Log.Warning("Mã HD thực tế {Real} khác mã trong nội dung chuyển khoản {Shown}", hd.MaHD, qrInvoiceId);
                extra = $"\n(Lưu ý: nội dung chuyển khoản ghi HD{qrInvoiceId}, mã hóa đơn thực là HD{hd.MaHD})";
            }
            cart.Clear(); UpdateTotal();
            MessageBox.Show($"Đã lưu hóa đơn HD{hd.MaHD}.{extra}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { Fail("lưu hóa đơn", ex); }
    }

    static void Fail(string action, Exception ex)
    {
        Log.Error(ex, "Lỗi CSDL khi {Action}", action);
        MessageBox.Show($"Không thể {action} lúc này. Vui lòng thử lại.", "Có lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

