using System.Globalization;
using System.Text;
using System.Text.Json;

namespace DaNgoai;

public record DayForecast(DateOnly Date, int Code, double TMin, double TMax, int? RainProb);

public static class WeatherService
{
    // HttpClient dùng chung, Timeout 8 giây
    static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(8) };
    const string GeoUrl = "https://geocoding-api.open-meteo.com/v1/search";
    const string ForecastUrl = "https://api.open-meteo.com/v1/forecast";

    /// <summary>Bước 1: tên địa danh -> (lat, lon). Trả null nếu không tìm thấy.</summary>
    public static async Task<(double lat, double lon)?> GeocodeAsync(string name)
    {
        var r = await GeoOnce(name);
        if (r == null)
        {
            var plain = RemoveDiacritics(name);                       // thử lại tên không dấu: "Đà Lạt" -> "Da Lat"
            if (!string.Equals(plain, name, StringComparison.Ordinal)) r = await GeoOnce(plain);
        }
        return r;
    }

    static async Task<(double, double)?> GeoOnce(string name)
    {
        var url = $"{GeoUrl}?name={Uri.EscapeDataString(name)}&count=1&language={Loc.Lang}";
        using var doc = JsonDocument.Parse(await Http.GetStringAsync(url));
        if (!doc.RootElement.TryGetProperty("results", out var arr) || arr.GetArrayLength() == 0) return null;
        var f = arr[0];
        return (f.GetProperty("latitude").GetDouble(), f.GetProperty("longitude").GetDouble());
    }

    /// <summary>Bước 2: lấy dự báo 16 ngày, tìm đúng chỉ số của ngày đi. Trả null nếu ngày đi ngoài phạm vi.</summary>
    public static async Task<DayForecast?> GetForecastAsync(double lat, double lon, DateOnly day)
    {
        var inv = CultureInfo.InvariantCulture;
        var url = $"{ForecastUrl}?latitude={lat.ToString(inv)}&longitude={lon.ToString(inv)}" +
                  "&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max" +
                  "&timezone=auto&forecast_days=16";
        using var doc = JsonDocument.Parse(await Http.GetStringAsync(url));
        var daily = doc.RootElement.GetProperty("daily");
        var times = daily.GetProperty("time");

        int idx = -1; var key = day.ToString("yyyy-MM-dd");
        for (int i = 0; i < times.GetArrayLength(); i++)
            if (times[i].GetString() == key) { idx = i; break; }
        if (idx < 0) return null;

        double Num(string p) => daily.GetProperty(p)[idx].GetDouble();
        var probEl = daily.GetProperty("precipitation_probability_max")[idx];
        int? prob = probEl.ValueKind == JsonValueKind.Number ? (int)Math.Round(probEl.GetDouble()) : null;
        return new DayForecast(day, (int)Num("weather_code"), Num("temperature_2m_min"), Num("temperature_2m_max"), prob);
    }

    public static string KeyOf(int c) =>
        c == 0 ? "w_clear" :
        c >= 1 && c <= 3 ? "w_cloud" :
        c == 45 || c == 48 ? "w_fog" :
        (c >= 51 && c <= 67) || (c >= 80 && c <= 82) ? "w_rain" :
        c >= 95 && c <= 99 ? "w_storm" :
        (c >= 71 && c <= 77) || c == 85 || c == 86 ? "w_snow" : "w_other";

    public static string IconOf(int c) => KeyOf(c) switch
    {
        "w_clear" => "☀", "w_cloud" => "⛅", "w_fog" => "🌫", "w_rain" => "🌧",
        "w_storm" => "⛈", "w_snow" => "❄", _ => "❓"
    };

    static string RemoveDiacritics(string s)
    {
        var sb = new StringBuilder();
        foreach (var ch in s.Replace('đ', 'd').Replace('Đ', 'D').Normalize(NormalizationForm.FormD))
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
