namespace QuanLyDatTour;

public class FormMain : Form
{
    readonly TourTab tour = new();
    readonly KhachTab khach = new();
    readonly DatTourTab dat = new();

    public FormMain()
    {
        // Phiên bản lấy từ <Version> trong .csproj, hiện trên thanh tiêu đề
        var ver = typeof(FormMain).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";
        Text = $"Quản lý đặt tour - Bình An v{ver}";
        ClientSize = new Size(1000, 640); MinimumSize = new Size(1000, 640);
        StartPosition = FormStartPosition.CenterScreen;

        var tabs = new TabControl { Dock = DockStyle.Fill };
        foreach (var (title, ctl) in new (string, Control)[] { ("Tour", tour), ("Khách hàng", khach), ("Đặt tour", dat) })
        {
            var p = new TabPage(title); ctl.Dock = DockStyle.Fill; p.Controls.Add(ctl); tabs.TabPages.Add(p);
        }
        tabs.SelectedIndexChanged += (_, _) =>
        {
            switch (tabs.SelectedIndex)
            {
                case 0: tour.Reload(); break;
                case 1: khach.Reload(); break;
                case 2: dat.ReloadAll(); break;
            }
        };
        Controls.Add(tabs);
    }
}
