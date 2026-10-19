namespace QuanLyDatTour;

static class Ui
{
    public static Label Lb(string t, int x, int y) => new() { Text = t, Left = x, Top = y + 3, AutoSize = true };
    public static T At<T>(T c, int x, int y, int w) where T : Control { c.Left = x; c.Top = y; c.Width = w; return c; }
    public static Button Btn(string t, int x, int y, Action a)
    { var b = new Button { Text = t, Left = x, Top = y, Width = 90, Height = 28 }; b.Click += (_, _) => a(); return b; }

    public static void SetupGrid(DataGridView g, Rectangle b)
    {
        g.Bounds = b; g.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        g.ReadOnly = true; g.AllowUserToAddRows = false; g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }

    public static bool Warn(string msg, Control? focus = null)
    {
        MessageBox.Show(msg, "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        focus?.Focus(); return false;
    }

    public static void Fail(string action, Exception ex)
    {
        Serilog.Log.Error(ex, "Lỗi CSDL khi {Action}", action);
        MessageBox.Show($"Không thể {action} lúc này. Vui lòng thử lại.", "Có lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
