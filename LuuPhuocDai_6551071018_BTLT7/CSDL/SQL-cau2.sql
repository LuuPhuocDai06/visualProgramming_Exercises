CREATE DATABASE FitZoneDB;
GO
USE FitZoneDB;
GO

CREATE TABLE HoiVien (
    MaHV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh BIT, -- 1 = Nam, 0 = Nữ
    NgaySinh DATE,
    SDT VARCHAR(15),
    Email VARCHAR(100),
    HangThanhVien NVARCHAR(20), -- Basic / VIP / Premium
    NgayDangKy DATETIME DEFAULT GETDATE(),
    TrangThai BIT -- 1 = Đang hoạt động, 0 = Tạm ngưng
);
GO