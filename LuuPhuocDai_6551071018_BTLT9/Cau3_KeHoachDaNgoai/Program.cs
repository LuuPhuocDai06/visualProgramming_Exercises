namespace DaNgoai;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            Db.ConnectionString = AppSettings.Load().Database.ConnectionString;
            Application.Run(new FormChuyenDi());
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể khởi động ứng dụng / Cannot start the application.\n" + ex.Message);
        }
    }
}
