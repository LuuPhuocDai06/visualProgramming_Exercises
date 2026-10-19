using DaNgoai.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace DaNgoai;


public class NudEx : NumericUpDown
{
    public void RefreshText() => UpdateEditText();
}

public class FormChuyenDi : Form
{

    // Giá trị lưu trong CSDL luôn là tiếng Việt; chỉ phần hiển thị được dịch
    static readonly string[] PtKeys = { "Xe máy", "Ô tô", "Xe khách", "Tàu hỏa" };
    sealed record PtItem(string Key) { public override string ToString() => PtText(Key); }
    static string PtText(string v) { var s = Loc.Get("pt_" + v); return s.StartsWith("pt_") ? v : s; }

    readonly List<(Control c, string key)> tr = new();   // control nào hiện chuỗi nào -> đổi ngôn ngữ chỉ cần chạy lại vòng lặp

    readonly Label lblTitle = new() { Font = new Font("Segoe UI", 14F, FontStyle.Bold), AutoSize = true };
    readonly ComboBox cboLang = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly GroupBox gbInfo = new(), gbWeather = new();
    readonly TextBox txtTen = new() { MaxLength = 100 }, txtDiaDiem = new() { MaxLength = 100 };
    readonly DateTimePicker dtpNgay = new();
    readonly NudEx nudNguoi = new() { Minimum = 1, Maximum = 1000, Value = 1 };
    readonly NudEx nudNganSach = new() { Minimum = 0, Maximum = 10_000_000_000, Increment = 100_000, ThousandsSeparator = true };

    readonly ComboBox cboPt = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly CheckBox chkXong = new() { AutoSize = true };
    // thời tiết
    readonly Button btnWeather = new();
    readonly ProgressBar prg = new() { Style = ProgressBarStyle.Marquee, MarqueeAnimationSpeed = 30, Visible = false };
    readonly Label lblIcon = new() { Font = new Font("Segoe UI Emoji", 44F), TextAlign = ContentAlignment.MiddleCenter };
    readonly Label lblDesc = new() { Font = new Font("Segoe UI", 16F), AutoSize = true };
    readonly Label lblTemp = new() { AutoSize = true }, lblProb = new() { AutoSize = true };
    readonly Panel pnlAdvice = new() { BackColor = Color.FromArgb(255, 204, 128), Visible = false };
    readonly Label lblAdvice = new() { AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
    Action? renderWeather;
    // tìm kiếm + lưới
    readonly TextBox txtTim = new();
    readonly CheckBox chkChuaHT = new() { AutoSize = true };
    readonly DateTimePicker dtpTu = new() { ShowCheckBox = true, Checked = false };
    readonly DateTimePicker dtpDen = new() { ShowCheckBox = true, Checked = false };
    readonly DataGridView dgv = new();

    int? selectedId; bool initLang = true;

    public FormChuyenDi()
    {
        ClientSize = new Size(960, 640); MinimumSize = new Size(960, 640);
        StartPosition = FormStartPosition.CenterScreen;

        lblTitle.Location = new Point(10, 8);
        var lblLang = Tx(new Label { AutoSize = true, Location = new Point(740, 14) }, "lang");
        cboLang.SetBounds(820, 10, 125, 24); cboLang.Items.AddRange(new object[] { "Tiếng Việt", "English" });
        cboLang.SelectedIndexChanged += (_, _) =>
        {
            if (initLang) return;
            Loc.Lang = cboLang.SelectedIndex == 1 ? "en" : "vi";
            new UserSettings { Language = Loc.Lang }.Save();      // nhớ lựa chọn cho lần mở sau
            ApplyLanguage(); Reload(); renderWeather?.Invoke();
        };

        // ----- thông tin chuyến đi
        gbInfo.SetBounds(10, 45, 470, 280); Tx(gbInfo, "grpInfo");
        cboPt.Items.AddRange(PtKeys.Select(k => (object)new PtItem(k)).ToArray()); cboPt.SelectedIndex = 0;
        gbInfo.Controls.AddRange(new Control[] {
            Row("tenChuyen", 25, txtTen), Row("diaDiem", 57, txtDiaDiem), Row("ngayDi", 89, dtpNgay),
            Row("soNguoi", 121, nudNguoi), Row("nganSach", 153, nudNganSach), Row("phuongTien", 185, cboPt) });
        chkXong.Location = new Point(120, 217); Tx(chkXong, "daHoanThanh"); gbInfo.Controls.Add(chkXong);
        int bx = 15;
        foreach (var (key, act) in new (string, Action)[] { ("them", Them), ("sua", Sua), ("xoa", Xoa), ("lamMoi", LamMoi) })
        {
            var b = Tx(new Button { Bounds = new Rectangle(bx, 242, 105, 28) }, key); b.Click += (_, _) => act();
            gbInfo.Controls.Add(b); bx += 112;
        }

        // ----- thời tiết
        btnWeather.SetBounds(495, 48, 150, 28); Tx(btnWeather, "xemThoiTiet");
        btnWeather.Click += BtnWeather_Click;
        prg.SetBounds(655, 54, 290, 16);
        gbWeather.SetBounds(490, 85, 455, 240); Tx(gbWeather, "grpWeather");
        lblIcon.SetBounds(15, 30, 120, 95); lblDesc.Location = new Point(145, 32);
        lblTemp.Location = new Point(147, 78); lblProb.Location = new Point(147, 102);
        pnlAdvice.SetBounds(15, 160, 425, 45); pnlAdvice.Controls.Add(lblAdvice);
        gbWeather.Controls.AddRange(new Control[] { lblIcon, lblDesc, lblTemp, lblProb, pnlAdvice });

        // ----- tìm kiếm
        txtTim.SetBounds(10, 335, 200, 24);
        chkChuaHT.Location = new Point(222, 338); Tx(chkChuaHT, "chiChuaHT");
        var lblTu = Tx(new Label { AutoSize = true, Location = new Point(480, 339) }, "tu");
        dtpTu.SetBounds(520, 335, 130, 24);
        var lblDen = Tx(new Label { AutoSize = true, Location = new Point(660, 339) }, "den");
        dtpDen.SetBounds(700, 335, 130, 24);
        var btnTim = Tx(new Button { Bounds = new Rectangle(845, 334, 100, 27) }, "tim"); btnTim.Click += (_, _) => Reload();

        dgv.SetBounds(10, 370, 935, 260);
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv.ReadOnly = true; dgv.AllowUserToAddRows = false; dgv.MultiSelect = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.CellClick += Dgv_CellClick;

        Controls.AddRange(new Control[] { lblTitle, lblLang, cboLang, gbInfo, btnWeather, prg, gbWeather,
            txtTim, chkChuaHT, lblTu, dtpTu, lblDen, dtpDen, btnTim, dgv });

        // áp dụng ngôn ngữ đã lưu từ lần trước
        Loc.Lang = UserSettings.Load().Language == "en" ? "en" : "vi";
        cboLang.SelectedIndex = Loc.Lang == "en" ? 1 : 0; initLang = false;
        ApplyLanguage();
        Load += (_, _) => { Reload(); ShowIdle(); };
    }

    // ---------- tiện ích dựng UI ----------
    T Tx<T>(T c, string key) where T : Control { tr.Add((c, key)); return c; }
    Control Row(string key, int y, Control input)
    {
        var p = new Panel { Bounds = new Rectangle(0, y, 465, 30) };
        var l = Tx(new Label { AutoSize = true, Location = new Point(15, 4) }, key);
        input.SetBounds(120, 1, 330, 24);
        p.Controls.AddRange(new[] { l, input });
        return p;
    }

    // ---------- đa ngôn ngữ ----------
    void ApplyLanguage()
    {
        var ci = Loc.Culture;
        CultureInfo.CurrentCulture = ci; CultureInfo.CurrentUICulture = ci;
        CultureInfo.DefaultThreadCurrentCulture = ci; CultureInfo.DefaultThreadCurrentUICulture = ci;

        Text = Loc.Get("title"); lblTitle.Text = Loc.Get("title");
        foreach (var (c, key) in tr) c.Text = Loc.Get(key);
        txtTim.PlaceholderText = Loc.Get("timHint");

        foreach (var d in new[] { dtpNgay, dtpTu, dtpDen })      // định dạng ngày theo CultureInfo
        { d.Format = DateTimePickerFormat.Custom; d.CustomFormat = ci.DateTimeFormat.ShortDatePattern; }
        nudNganSach.RefreshText(); nudNguoi.RefreshText();

        int sel = Math.Max(0, cboPt.SelectedIndex);               // dịch lại danh sách phương tiện
        cboPt.Items.Clear(); cboPt.Items.AddRange(PtKeys.Select(k => (object)new PtItem(k)).ToArray());
        cboPt.SelectedIndex = sel;
    }

    // ---------- lưới ----------
    void Reload()
    {
        try
        {
            using var db = Db.Create();
            var q = db.ChuyenDis.AsNoTracking().AsQueryable();
            var kw = txtTim.Text.Trim();
            if (kw != "") q = q.Where(x => x.TenChuyen.Contains(kw) || x.DiaDiem.Contains(kw));
            if (chkChuaHT.Checked) q = q.Where(x => !x.DaHoanThanh);
            if (dtpTu.Checked) { var tu = DateOnly.FromDateTime(dtpTu.Value); q = q.Where(x => x.NgayDi >= tu); }
            if (dtpDen.Checked) { var den = DateOnly.FromDateTime(dtpDen.Value); q = q.Where(x => x.NgayDi <= den); }

            var list = q.OrderBy(x => x.NgayDi).ThenBy(x => x.MaChuyen).ToList();
            dgv.DataSource = list.Select(x => new
            {
                x.MaChuyen, x.TenChuyen, x.DiaDiem,
                NgayDi = x.NgayDi.ToDateTime(TimeOnly.MinValue),
                x.SoNguoi, x.NganSach, PhuongTien = PtText(x.PhuongTien), x.DaHoanThanh
            }).ToList();

            foreach (DataGridViewColumn col in dgv.Columns) col.HeaderText = Loc.Get("col_" + col.Name);
            dgv.Columns["NgayDi"]!.DefaultCellStyle.Format = Loc.Culture.DateTimeFormat.ShortDatePattern;
            dgv.Columns["NgayDi"]!.DefaultCellStyle.FormatProvider = Loc.Culture;
            dgv.Columns["NganSach"]!.DefaultCellStyle.Format = "N0";
            dgv.Columns["NganSach"]!.DefaultCellStyle.FormatProvider = Loc.Culture;
            dgv.Columns["MaChuyen"]!.FillWeight = 35; dgv.Columns["SoNguoi"]!.FillWeight = 55;
            dgv.ClearSelection(); selectedId = null;
        }
        catch { MessageBox.Show(Loc.Get("dbError"), Loc.Get("error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    void Dgv_CellClick(object? s, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        try
        {
            int id = (int)dgv.Rows[e.RowIndex].Cells["MaChuyen"].Value;
            using var db = Db.Create();
            var x = db.ChuyenDis.AsNoTracking().FirstOrDefault(c => c.MaChuyen == id);
            if (x == null) return;
            selectedId = id;
            txtTen.Text = x.TenChuyen; txtDiaDiem.Text = x.DiaDiem;
            dtpNgay.Value = x.NgayDi.ToDateTime(TimeOnly.MinValue);
            nudNguoi.Value = Math.Clamp(x.SoNguoi, (int)nudNguoi.Minimum, (int)nudNguoi.Maximum);
            nudNganSach.Value = Math.Min(nudNganSach.Maximum, x.NganSach);
            var i = Array.IndexOf(PtKeys, x.PhuongTien); cboPt.SelectedIndex = i < 0 ? 0 : i;
            chkXong.Checked = x.DaHoanThanh;
        }
        catch { MessageBox.Show(Loc.Get("dbError"), Loc.Get("error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    // ---------- CRUD ----------
    bool Valid(bool adding)
    {
        string? err = null;
        if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtDiaDiem.Text)) err = "vName";
        else if (adding && dtpNgay.Value.Date < DateTime.Today) err = "vDate";
        else if (nudNguoi.Value < 1) err = "vPeople";
        else if (nudNganSach.Value < 0) err = "vBudget";
        if (err == null) return true;
        MessageBox.Show(Loc.Get(err), Loc.Get("invalid"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    void Apply(ChuyenDi c)
    {
        c.TenChuyen = txtTen.Text.Trim(); c.DiaDiem = txtDiaDiem.Text.Trim();
        c.NgayDi = DateOnly.FromDateTime(dtpNgay.Value); c.SoNguoi = (int)nudNguoi.Value;
        c.NganSach = nudNganSach.Value; c.PhuongTien = ((PtItem)cboPt.SelectedItem!).Key; c.DaHoanThanh = chkXong.Checked;
    }

    void Them()
    {
        if (!Valid(true)) return;
        try { using var db = Db.Create(); var c = new ChuyenDi(); Apply(c); db.ChuyenDis.Add(c); db.SaveChanges(); LamMoi(); }
        catch { MessageBox.Show(Loc.Get("dbError"), Loc.Get("error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    void Sua()
    {
        if (selectedId == null) { MessageBox.Show(Loc.Get("pickTrip"), Loc.Get("info")); return; }
        if (!Valid(false)) return;
        try
        {
            using var db = Db.Create(); var c = db.ChuyenDis.Find(selectedId);
            if (c != null) { Apply(c); db.SaveChanges(); }
            LamMoi();
        }
        catch { MessageBox.Show(Loc.Get("dbError"), Loc.Get("error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    void Xoa()
    {
        if (selectedId == null) { MessageBox.Show(Loc.Get("pickTrip"), Loc.Get("info")); return; }
        if (MessageBox.Show(Loc.Get("confirmDelete"), Loc.Get("confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            using var db = Db.Create(); var c = db.ChuyenDis.Find(selectedId);
            if (c != null) { db.ChuyenDis.Remove(c); db.SaveChanges(); }
            LamMoi();
        }
        catch { MessageBox.Show(Loc.Get("dbError"), Loc.Get("error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    void LamMoi()
    {
        selectedId = null; txtTen.Clear(); txtDiaDiem.Clear(); dtpNgay.Value = DateTime.Today;
        nudNguoi.Value = 1; nudNganSach.Value = 0; cboPt.SelectedIndex = 0; chkXong.Checked = false;
        txtTim.Clear(); chkChuaHT.Checked = false; dtpTu.Checked = false; dtpDen.Checked = false;
        Reload();
    }

    // ---------- thời tiết ----------
    void SetBusy(bool busy) { btnWeather.Enabled = !busy; prg.Visible = busy; }

    void ShowIdle() => renderWeather = () =>
    {
        gbWeather.Text = Loc.Get("grpWeather"); gbWeather.BackColor = SystemColors.Control;
        lblIcon.Text = "🌤"; lblDesc.Text = ""; lblTemp.Text = ""; lblProb.Text = ""; pnlAdvice.Visible = false;
    };

    void ShowNoData() { renderWeather = () =>
    {
        gbWeather.Text = Loc.Get("grpWeather"); gbWeather.BackColor = SystemColors.Control;
        lblIcon.Text = "❓"; lblDesc.Text = Loc.Get("noData"); lblTemp.Text = Loc.Get("noDataDetail");
        lblProb.Text = ""; pnlAdvice.Visible = false;
    }; renderWeather(); }

    void ShowForecast(DayForecast f, string place) { renderWeather = () =>
    {
        gbWeather.Text = Loc.Get("grpWeatherFor", f.Date.ToString("d", Loc.Culture), place);
        lblIcon.Text = WeatherService.IconOf(f.Code);
        lblDesc.Text = Loc.Get(WeatherService.KeyOf(f.Code));
        lblTemp.Text = Loc.Get("temp", Math.Round(f.TMin), Math.Round(f.TMax));
        lblProb.Text = f.RainProb is int p ? Loc.Get("prob", p) : Loc.Get("probNA");
        bool rainy = f.RainProb >= 60;                              // xác suất mưa >= 60% -> cảnh báo nền cam
        gbWeather.BackColor = rainy ? Color.FromArgb(255, 243, 224) : SystemColors.Control;
        pnlAdvice.Visible = rainy; lblAdvice.Text = "  ⚠  " + Loc.Get("advice");
    }; renderWeather(); }

    async void BtnWeather_Click(object? sender, EventArgs e)
    {
        var place = txtDiaDiem.Text.Trim();
        if (place == "") { MessageBox.Show(Loc.Get("pickTrip"), Loc.Get("info")); return; }
        var day = DateOnly.FromDateTime(dtpNgay.Value);
        int diff = (dtpNgay.Value.Date - DateTime.Today).Days;
        if (diff < 0 || diff > 16) { ShowNoData(); return; }        // quá 16 ngày -> không gọi API

        SetBusy(true);
        try
        {
            var geo = await WeatherService.GeocodeAsync(place);
            if (geo == null) { MessageBox.Show(Loc.Get("notFound", place), Loc.Get("info"), MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            var fc = await WeatherService.GetForecastAsync(geo.Value.lat, geo.Value.lon, day);
            if (fc == null) ShowNoData(); else ShowForecast(fc, place);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        { MessageBox.Show(Loc.Get("netError"), Loc.Get("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        catch (Exception ex) when (ex is JsonException or KeyNotFoundException or InvalidOperationException)
        { MessageBox.Show(Loc.Get("genericError"), Loc.Get("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        finally { SetBusy(false); }
    }
}
