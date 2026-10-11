CREATE DATABASE AnKhangClinicDB;
GO

USE AnKhangClinicDB;
GO

-- 1. Bảng Bác sĩ
CREATE TABLE BacSi (
    MaBS INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    ChuyenKhoa NVARCHAR(100),
    SDT VARCHAR(15)
);
GO

-- 2. Bảng Lịch khám
CREATE TABLE LichKham (
    MaLich INT IDENTITY(1,1) PRIMARY KEY,
    TenBenhNhan NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    NgayKham DATE,
    GioKham VARCHAR(10), -- Hoặc kiểu TIME
    MaBS INT FOREIGN KEY REFERENCES BacSi(MaBS),
    TrangThai NVARCHAR(20) -- Chờ khám / Đã khám / Đã hủy
);
GO

-- Thêm dữ liệu mẫu Bác sĩ
INSERT INTO BacSi (HoTen, ChuyenKhoa, SDT) VALUES (N'Nguyễn Văn A', N'Nội tổng quát', '0901112223');
INSERT INTO BacSi (HoTen, ChuyenKhoa, SDT) VALUES (N'Trần Thị B', N'Nhi khoa', '0903334445');
INSERT INTO BacSi (HoTen, ChuyenKhoa, SDT) VALUES (N'Lê Văn C', N'Tim mạch', '0905556667');
GO

-- Thêm dữ liệu mẫu Lịch khám
INSERT INTO LichKham (TenBenhNhan, SDT, NgayKham, GioKham, MaBS, TrangThai) 
VALUES (N'Nguyễn Văn X', '0972566789', '2026-10-15', '09:00', 1, N'Chờ khám');

INSERT INTO LichKham (TenBenhNhan, SDT, NgayKham, GioKham, MaBS, TrangThai) 
VALUES (N'Trần Thị Y', '0972566790', '2026-10-16', '13:30', 2, N'Đã khám');
GO