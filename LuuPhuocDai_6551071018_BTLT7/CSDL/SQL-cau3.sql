CREATE DATABASE SunriseHomestayDB;
GO
USE SunriseHomestayDB;
GO

-- Bảng Loại phòng
CREATE TABLE LoaiPhong (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(100) NOT NULL,
    GiaMoiDem DECIMAL(18,2),
    MoTa NVARCHAR(255)
);
GO

-- Bảng Phòng (Có khóa ngoại trỏ tới LoaiPhong)
CREATE TABLE Phong (
    MaPhong INT IDENTITY(1,1) PRIMARY KEY,
    SoPhong VARCHAR(10) NOT NULL,
    TangSo INT,
    TinhTrang NVARCHAR(20), -- Trống / Đang ở / Đang dọn
    HinhAnh NVARCHAR(255),  -- Lưu tên file ảnh
    MaLoai INT FOREIGN KEY REFERENCES LoaiPhong(MaLoai)
);
GO
USE SunriseHomestayDB;
GO

-- Xóa dữ liệu cũ nếu có (hoặc chạy trực tiếp nếu bảng đang trống)
DELETE FROM Phong;
DELETE FROM LoaiPhong;
DBCC CHECKIDENT ('LoaiPhong', RESEED, 0);
DBCC CHECKIDENT ('Phong', RESEED, 0);

-- Thêm lại dữ liệu mẫu đúng tên cột (TenLoai)
INSERT INTO LoaiPhong (TenLoai, GiaMoiDem, MoTa) VALUES (N'Phòng Đơn', 350000, N'Phòng dành cho 1 người');
INSERT INTO LoaiPhong (TenLoai, GiaMoiDem, MoTa) VALUES (N'Phòng Đôi', 500000, N'Phòng dành cho 2 người');
INSERT INTO LoaiPhong (TenLoai, GiaMoiDem, MoTa) VALUES (N'Phòng VIP', 900000, N'Phòng cao cấp view đẹp');
GO