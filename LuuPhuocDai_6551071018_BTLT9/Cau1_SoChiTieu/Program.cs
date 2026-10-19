using Serilog;

namespace SoChiTieu;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Log ghi theo ngày vào thư mục Logs cạnh file .exe -> Logs/log-yyyyMMdd.txt
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"),
                          rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Db.ConnectionString = AppSettings.Load().Database.ConnectionString;
            Log.Information("Ứng dụng khởi động");
            Application.Run(new FormChiTieu());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Không thể khởi động ứng dụng");
            MessageBox.Show("Không thể khởi động ứng dụng. Vui lòng kiểm tra file appsettings.json.",
                "Sổ chi tiêu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { Log.CloseAndFlush(); }
    }
}
