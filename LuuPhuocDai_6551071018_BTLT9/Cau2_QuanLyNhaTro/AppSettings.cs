using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NhaTro.Models;

namespace NhaTro;

public class DatabaseSettings { public string ConnectionString { get; set; } = ""; }
public class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();
    public static AppSettings Load()
    {
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var b = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false);
        if (!string.IsNullOrEmpty(env)) b.AddJsonFile($"appsettings.{env}.json", optional: true);
        return b.Build().Get<AppSettings>() ?? new AppSettings();
    }
}

public static class Db
{
    public static string ConnectionString { get; set; } = "";
    public static QlNhaTroContext Create() =>
        new(new DbContextOptionsBuilder<QlNhaTroContext>().UseSqlServer(ConnectionString).Options);
}
