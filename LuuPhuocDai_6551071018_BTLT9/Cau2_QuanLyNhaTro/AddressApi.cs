using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NhaTro;

// Tên thuộc tính viết thường để khớp JSON và dùng thẳng cho DisplayMember/ValueMember
public class Province { public int code { get; set; } public string name { get; set; } = ""; }
public class Ward     { public int code { get; set; } public string name { get; set; } = ""; }
public class ProvinceDetail
{
    public int code { get; set; }
    public string name { get; set; } = "";
    public List<Ward> wards { get; set; } = new();
}

public static class AddressApi
{
    // MỘT HttpClient dùng chung cho cả ứng dụng, Timeout 10 giây
    static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };
    static readonly JsonSerializerOptions Opt = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    public static async Task<List<Province>> GetProvincesAsync() =>
        await Http.GetFromJsonAsync<List<Province>>("https://provinces.open-api.vn/api/v2/", Opt) ?? new();

    public static async Task<ProvinceDetail> GetWardsAsync(int provinceCode) =>
        await Http.GetFromJsonAsync<ProvinceDetail>($"https://provinces.open-api.vn/api/v2/p/{provinceCode}?depth=2", Opt)
        ?? new ProvinceDetail();
}
