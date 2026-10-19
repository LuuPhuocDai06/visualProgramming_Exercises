namespace TraSua;

public class FormMain : Form
{
    readonly FormBanHang banHang = new();
    readonly FormThucDon thucDon = new();
    readonly FormHoaDon hoaDon = new();

    public FormMain()
    {
        Text = AppSettings.Current.Payment.TenQuan is { Length: > 0 } t ? $"Bán hàng - {t}" : "Bán hàng quán trà sữa";
        ClientSize = new Size(900, 560); MinimumSize = new Size(900, 560);
        StartPosition = FormStartPosition.CenterScreen;

        var tabs = new TabControl { Dock = DockStyle.Fill };
        AddTab(tabs, "Bán hàng", banHang); AddTab(tabs, "Thực đơn", thucDon); AddTab(tabs, "Hóa đơn", hoaDon);
        tabs.SelectedIndexChanged += (_, _) => { banHang.RefreshMenu(); thucDon.Reload(); hoaDon.Reload(); };
        Controls.Add(tabs);
    }

    static void AddTab(TabControl tabs, string title, Form f)
    {
        var page = new TabPage(title);
        f.TopLevel = false; f.FormBorderStyle = FormBorderStyle.None; f.Dock = DockStyle.Fill;
        page.Controls.Add(f); tabs.TabPages.Add(page); f.Show();
    }
}
