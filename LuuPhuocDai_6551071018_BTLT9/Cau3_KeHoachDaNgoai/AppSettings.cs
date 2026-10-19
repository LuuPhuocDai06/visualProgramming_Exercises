using DaNgoai.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DaNgoai;

public class DatabaseSettings { public string ConnectionString { get; set; } = ""; }
public class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();
    public static AppSettings Load() =>
        new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false).Build().Get<AppSettings>() ?? new AppSettings();
}

public static class Db
{
    public static string ConnectionString { get; set; } = "";
    public static QlDaNgoaiContext Create() =>
        new(new DbContextOptionsBuilder<QlDaNgoaiContext>().UseSqlServer(ConnectionString).Options);
}

// usersettings.json trong %LocalAppData%\KeHoachDaNgoai\
public class UserSettings
{
    public string Language { get; set; } = "vi";

    static string FilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KeHoachDaNgoai", "usersettings.json");

    public static UserSettings Load()
    {
        try { if (File.Exists(FilePath)) return JsonSerializer.Deserialize<UserSettings>(File.ReadAllText(FilePath)) ?? new(); }
        catch { /* file hỏng -> dùng mặc định */ }
        return new UserSettings();
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch { /* không lưu được cũng không làm app chết */ }
    }
}
