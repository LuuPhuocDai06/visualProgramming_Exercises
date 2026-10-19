using Serilog;

namespace QuanLyDatTour;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        ApplicationConfiguration.Initialize();

        Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Application.ThreadException += (_, e) => HandleFatal(e.Exception, "Application.ThreadException");
        AppDomain.CurrentDomain.UnhandledException += (_, e) => HandleFatal(e.ExceptionObject as Exception, "AppDomain.UnhandledException");

        try
        {
            AppSettings.Load();
            TyGiaService.Init(AppSettings.Current.ExchangeRate);
            Application.Run(new FormMain());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Không thể khởi động ứng dụng");
            MessageBox.Show("Không thể khởi động ứng dụng. Hãy kiểm tra file appsettings.json cạnh file .exe.",
                "Quản lý đặt tour", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { Log.CloseAndFlush(); }
    }

    static void HandleFatal(Exception? ex, string source)
    {
        Log.Error(ex, "Lỗi chưa được bắt ({Source})", source);
        MessageBox.Show("Đã xảy ra lỗi, thông tin đã được ghi lại.", "Quản lý đặt tour", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
