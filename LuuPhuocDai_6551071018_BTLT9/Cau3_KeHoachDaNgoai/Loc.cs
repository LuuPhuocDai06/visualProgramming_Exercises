using System.Globalization;

namespace DaNgoai;

// Bộ từ điển Việt / Anh. Đổi Loc.Lang là toàn bộ giao diện lấy chuỗi mới qua Loc.Get(key).
public static class Loc
{
    public static string Lang { get; set; } = "vi";
    static readonly CultureInfo Vi = new("vi-VN"), En = new("en-US");
    public static CultureInfo Culture => Lang == "vi" ? Vi : En;

    static readonly Dictionary<string, (string vi, string en)> T = new()
    {
        ["title"] = ("Kế hoạch dã ngoại cuối tuần", "Weekend Picnic Planner"),
        ["lang"] = ("Ngôn ngữ", "Language"),
        ["grpInfo"] = ("Thông tin chuyến đi", "Trip information"),
        ["tenChuyen"] = ("Tên chuyến", "Trip name"), ["diaDiem"] = ("Địa điểm", "Destination"),
        ["ngayDi"] = ("Ngày đi", "Date"), ["soNguoi"] = ("Số người", "People"),
        ["nganSach"] = ("Ngân sách", "Budget"), ["phuongTien"] = ("Phương tiện", "Transport"),
        ["daHoanThanh"] = ("Đã hoàn thành", "Completed"),
        ["them"] = ("Thêm", "Add"), ["sua"] = ("Sửa", "Edit"), ["xoa"] = ("Xóa", "Delete"), ["lamMoi"] = ("Làm mới", "Refresh"),
        ["xemThoiTiet"] = ("Xem thời tiết", "Check weather"),
        ["grpWeather"] = ("Dự báo thời tiết", "Weather forecast"),
        ["grpWeatherFor"] = ("Dự báo thời tiết ngày {0} - {1}", "Forecast for {0} - {1}"),
        ["timHint"] = ("Tìm tên chuyến hoặc địa điểm...", "Search trip or destination..."),
        ["chiChuaHT"] = ("Chỉ hiện chuyến chưa hoàn thành", "Only show unfinished trips"),
        ["tu"] = ("Từ", "From"), ["den"] = ("Đến", "To"), ["tim"] = ("Tìm", "Search"),
        ["col_MaChuyen"] = ("Mã", "ID"), ["col_TenChuyen"] = ("Tên chuyến", "Trip name"),
        ["col_DiaDiem"] = ("Địa điểm", "Destination"), ["col_NgayDi"] = ("Ngày đi", "Date"),
        ["col_SoNguoi"] = ("Số người", "People"), ["col_NganSach"] = ("Ngân sách", "Budget"),
        ["col_PhuongTien"] = ("Phương tiện", "Transport"), ["col_DaHoanThanh"] = ("Hoàn thành", "Done"),
        ["pt_Xe máy"] = ("Xe máy", "Motorbike"), ["pt_Ô tô"] = ("Ô tô", "Car"),
        ["pt_Xe khách"] = ("Xe khách", "Coach"), ["pt_Tàu hỏa"] = ("Tàu hỏa", "Train"),
        // thời tiết
        ["w_clear"] = ("Trời quang", "Clear sky"), ["w_cloud"] = ("Có mây", "Partly cloudy"),
        ["w_fog"] = ("Sương mù", "Fog"), ["w_rain"] = ("Mưa", "Rain"), ["w_storm"] = ("Dông", "Thunderstorm"),
        ["w_snow"] = ("Tuyết", "Snow"), ["w_other"] = ("Không xác định", "Unknown"),
        ["temp"] = ("Nhiệt độ: {0}°C - {1}°C", "Temperature: {0}°C - {1}°C"),
        ["prob"] = ("Khả năng mưa: {0}%", "Chance of rain: {0}%"),
        ["probNA"] = ("Khả năng mưa: không có dữ liệu", "Chance of rain: n/a"),
        ["advice"] = ("Nên mang áo mưa hoặc cân nhắc dời lịch", "Bring a raincoat or consider rescheduling"),
        ["noData"] = ("Chưa có dữ liệu dự báo", "No forecast data yet"),
        ["noDataDetail"] = ("Open-Meteo chỉ dự báo tối đa 16 ngày kể từ hôm nay.", "Open-Meteo only forecasts up to 16 days ahead."),
        ["notFound"] = ("Không tìm thấy địa danh \"{0}\". Hãy thử tên không dấu, ví dụ \"Da Lat\".",
                        "Place \"{0}\" was not found. Try the name without diacritics, e.g. \"Da Lat\"."),
        ["netError"] = ("Không kết nối được máy chủ thời tiết. Vui lòng kiểm tra mạng.",
                        "Cannot reach the weather service. Please check your network."),
        ["genericError"] = ("Có lỗi xảy ra. Vui lòng thử lại.", "Something went wrong. Please try again."),
        ["dbError"] = ("Không thể thao tác với cơ sở dữ liệu lúc này.", "Cannot access the database right now."),
        ["pickTrip"] = ("Hãy chọn một chuyến đi trong danh sách.", "Please select a trip in the list."),
        // kiểm tra hợp lệ
        ["vName"] = ("Tên chuyến và địa điểm không được để trống.", "Trip name and destination are required."),
        ["vDate"] = ("Ngày đi không được nhỏ hơn hôm nay.", "The trip date cannot be before today."),
        ["vPeople"] = ("Số người phải ≥ 1.", "Number of people must be ≥ 1."),
        ["vBudget"] = ("Ngân sách phải ≥ 0.", "Budget must be ≥ 0."),
        ["confirmDelete"] = ("Bạn có chắc muốn xóa chuyến đi này?", "Delete this trip?"),
        ["invalid"] = ("Dữ liệu chưa hợp lệ", "Invalid data"), ["confirm"] = ("Xác nhận", "Confirm"),
        ["error"] = ("Có lỗi", "Error"), ["info"] = ("Thông báo", "Notice"),
    };

    public static string Get(string key) =>
        T.TryGetValue(key, out var v) ? (Lang == "vi" ? v.vi : v.en) : key;
    public static string Get(string key, params object[] args) => string.Format(Culture, Get(key), args);
}
