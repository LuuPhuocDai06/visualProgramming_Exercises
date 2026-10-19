using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SoChiTieu.Models;

namespace SoChiTieu;

// Lớp cấu hình có kiểu (strongly-typed)
public class DatabaseSettings { public string ConnectionString { get; set; } = ""; }

public class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();

    public static AppSettings Load()
    {
        // DOTNET_ENVIRONMENT=Development (đặt trong launchSettings.json) -> nạp thêm appsettings.Development.json để ghi đè
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        if (!string.IsNullOrEmpty(env))
            builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false);

        return builder.Build().Get<AppSettings>() ?? new AppSettings();
    }
}

public static class Db
{
    public static string ConnectionString { get; set; } = "";

    public static QlChiTieuContext Create()
    {
        var options = new DbContextOptionsBuilder<QlChiTieuContext>().UseSqlServer(ConnectionString).Options;
        return new QlChiTieuContext(options);
    }
}
