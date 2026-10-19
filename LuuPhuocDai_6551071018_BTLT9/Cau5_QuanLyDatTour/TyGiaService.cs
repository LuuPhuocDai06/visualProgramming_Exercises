using Serilog;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;

namespace QuanLyDatTour;

public record TyGiaResult(decimal Rate, string Source, bool FromCache);

public class TyGiaException : Exception
{
    public TyGiaException(string message, Exception? inner) : base(message, inner) { }
}

public class TyGiaService
{
    public static TyGiaService Instance { get; private set; } = null!;
    public static void Init(ExchangeRateSettings s) => Instance = new TyGiaService(s);

    readonly ExchangeRateSettings cfg;
    readonly HttpClient http;                       // dùng chung
    readonly ConcurrentDictionary<string, (DateTime At, decimal Rate, string Source)> cache = new();

    TyGiaService(ExchangeRateSettings s)
    {
        cfg = s; http = new HttpClient { Timeout = TimeSpan.FromSeconds(s.TimeoutSeconds) };
    }

    /// <summary>Tỷ giá: 1 đơn vị ngoại tệ = ? VND. Thử địa chỉ chính, lỗi thì thử địa chỉ dự phòng.</summary>
    public async Task<TyGiaResult> LayTyGiaAsync(string ngoaiTe)
    {
        var code = ngoaiTe.Trim().ToLowerInvariant();
        if (code == "vnd") return new TyGiaResult(1m, "VND", false);

        if (cache.TryGetValue(code, out var c) && DateTime.UtcNow - c.At < TimeSpan.FromMinutes(cfg.CacheMinutes))
        {
            Log.Information("Tỷ giá {Code}→VND lấy từ bộ nhớ đệm ({Source})", code, c.Source);
            return new TyGiaResult(c.Rate, c.Source, true);
        }

        var urls = new[] { cfg.PrimaryUrl, cfg.FallbackUrl };
        Exception? last = null;
        for (int i = 0; i < urls.Length; i++)
        {
            var url = urls[i].Replace("{code}", code);
            try
            {
                var rate = await FetchAsync(url, code);
                var host = new Uri(url).Host;
                cache[code] = (DateTime.UtcNow, rate, host);
                Log.Information("Tỷ giá {Code}→VND = {Rate} lấy từ địa chỉ {Which}: {Host}",
                    code, rate, i == 0 ? "chính" : "dự phòng", host);
                return new TyGiaResult(rate, host, false);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException
                                          or KeyNotFoundException or InvalidOperationException or UriFormatException)
            {
                last = ex;
                if (i == 0) Log.Warning("Địa chỉ chính lỗi, chuyển sang địa chỉ dự phòng. Lý do: {Reason}", ex.Message);
                else Log.Warning("Địa chỉ dự phòng cũng lỗi. Lý do: {Reason}", ex.Message);
            }
        }
        throw new TyGiaException("Không lấy được tỷ giá từ cả hai địa chỉ.", last);
    }

    async Task<decimal> FetchAsync(string url, string code)
    {
        // Kết quả dạng {"date":"…","thb":{"vnd":778.1,...}}
        using var doc = JsonDocument.Parse(await http.GetStringAsync(url));
        var rate = doc.RootElement.GetProperty(code).GetProperty("vnd").GetDecimal();
        if (rate <= 0) throw new InvalidOperationException("Tỷ giá không hợp lệ");
        return rate;
    }
}
