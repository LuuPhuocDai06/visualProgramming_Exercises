using Serilog;

namespace NhaTro;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        // Phải gọi trước khi tạo bất kỳ form nào
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        ApplicationConfiguration.Initialize();

        Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"),
                          rollingInterval: RollingInterval.Day).CreateLogger();

        Application.ThreadException += (_, e) => HandleFatal(e.Exception, "Application.ThreadException");
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            HandleFatal(e.ExceptionObject as Exception, "AppDomain.UnhandledException");

        try
        {
            Db.ConnectionString = AppSettings.Load().Database.ConnectionString;
            Application.Run(new FormKhachThue());
        }
        finally { Log.CloseAndFlush(); }
    }

    static void HandleFatal(Exception? ex, string source)
    {
        Log.Error(ex, "Lỗi chưa được bắt ({Source})", source);   // ghi đầy đủ stack trace
        MessageBox.Show("Đã xảy ra lỗi, thông tin đã được ghi lại.", "Quản lý nhà trọ",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
