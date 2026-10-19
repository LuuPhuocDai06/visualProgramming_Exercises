using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuanLyDatTour.Models;

namespace QuanLyDatTour;

public class DatabaseSettings { public string ConnectionString { get; set; } = ""; }

public class ExchangeRateSettings
{
    public string PrimaryUrl { get; set; } = "https://cdn.jsdelivr.net/npm/@fawazahmed0/currency-api@latest/v1/currencies/{code}.json";
    public string FallbackUrl { get; set; } = "https://latest.currency-api.pages.dev/v1/currencies/{code}.json";
    public int TimeoutSeconds { get; set; } = 8;
    public int CacheMinutes { get; set; } = 10;
}

public class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();
    public ExchangeRateSettings ExchangeRate { get; set; } = new();
    public static AppSettings Current { get; private set; } = new();

    public static void Load() =>
        Current = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false).Build().Get<AppSettings>() ?? new AppSettings();
}

public static class Db
{
    public static QlDatTourContext Create() =>
        new(new DbContextOptionsBuilder<QlDatTourContext>().UseSqlServer(AppSettings.Current.Database.ConnectionString).Options);
}
