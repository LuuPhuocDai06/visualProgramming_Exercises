using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TraSua.Models;

namespace TraSua;

public class DatabaseSettings { public string ConnectionString { get; set; } = ""; }

// Mục "Payment" trong appsettings.json -> lớp cấu hình có kiểu, KHÔNG hard-code trong code
public class PaymentSettings
{
    public string TenQuan { get; set; } = "";
    public string BankId { get; set; } = "";
    public string AccountNo { get; set; } = "";
    public string AccountName { get; set; } = "";
    public string Template { get; set; } = "compact2";
}

public class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();
    public PaymentSettings Payment { get; set; } = new();

    public static AppSettings Current { get; private set; } = new();

    public static void Load() =>
        Current = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false).Build().Get<AppSettings>() ?? new AppSettings();
}

public static class Db
{
    public static QlTraSuaContext Create() =>
        new(new DbContextOptionsBuilder<QlTraSuaContext>().UseSqlServer(AppSettings.Current.Database.ConnectionString).Options);
}
