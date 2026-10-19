using Serilog;

namespace TraSua;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();
        try
        {
            AppSettings.Load();
            Application.Run(new FormMain());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Không thể khởi động");
            MessageBox.Show("Không thể khởi động ứng dụng. Hãy kiểm tra file appsettings.json.");
        }
        finally { Log.CloseAndFlush(); }
    }
}
