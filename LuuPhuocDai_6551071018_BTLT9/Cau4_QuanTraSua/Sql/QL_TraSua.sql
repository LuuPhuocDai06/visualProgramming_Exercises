IF DB_ID('QL_TraSua') IS NULL CREATE DATABASE QL_TraSua;
GO
USE QL_TraSua;
GO
IF OBJECT_ID('dbo.ChiTietHoaDon') IS NOT NULL DROP TABLE dbo.ChiTietHoaDon;
IF OBJECT_ID('dbo.HoaDon')        IS NOT NULL DROP TABLE dbo.HoaDon;
IF OBJECT_ID('dbo.MonUong')       IS NOT NULL DROP TABLE dbo.MonUong;
IF OBJECT_ID('dbo.DanhMucMon')    IS NOT NULL DROP TABLE dbo.DanhMucMon;
GO
CREATE TABLE dbo.DanhMucMon(
    MaDM  INT IDENTITY(1,1) PRIMARY KEY,
    TenDM NVARCHAR(50) NOT NULL
);
CREATE TABLE dbo.MonUong(
    MaMon   INT IDENTITY(1,1) PRIMARY KEY,
    TenMon  NVARCHAR(100) NOT NULL,
    GiaBan  DECIMAL(18,0) NOT NULL CHECK (GiaBan >= 0),
    DangBan BIT NOT NULL DEFAULT 1,
    MaDM    INT NOT NULL REFERENCES dbo.DanhMucMon(MaDM)
);
CREATE TABLE dbo.HoaDon(
    MaHD        INT IDENTITY(1,1) PRIMARY KEY,
    NgayLap     DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien    DECIMAL(18,0) NOT NULL,
    HinhThucTT  NVARCHAR(20) NOT NULL CHECK (HinhThucTT IN (N'Tiền mặt', N'Chuyển khoản')),
    DaThanhToan BIT NOT NULL DEFAULT 0
);
CREATE TABLE dbo.ChiTietHoaDon(
    MaHD   INT NOT NULL REFERENCES dbo.HoaDon(MaHD),
    MaMon  INT NOT NULL REFERENCES dbo.MonUong(MaMon),
    SoLy   INT NOT NULL CHECK (SoLy > 0),
    DonGia DECIMAL(18,0) NOT NULL,
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaHD, MaMon)
);
GO
INSERT dbo.DanhMucMon(TenDM) VALUES (N'Trà sữa'),(N'Trà trái cây'),(N'Topping'),(N'Đồ ăn vặt');
INSERT dbo.MonUong(TenMon,GiaBan,DangBan,MaDM) VALUES
(N'Trà sữa trân châu', 35000,1,1),(N'Trà sữa matcha',40000,1,1),
(N'Hồng trà sữa',      32000,1,1),(N'Trà sữa khoai môn',38000,1,1),
(N'Trà đào cam sả',    42000,1,2),(N'Trà vải hoa hồng',40000,1,2),(N'Trà dâu tằm',38000,1,2),
(N'Trân châu trắng',    5000,1,3),(N'Thạch dừa',5000,1,3),(N'Pudding trứng',8000,1,3),
(N'Khoai tây chiên',   25000,1,4),(N'Gà popcorn',30000,1,4),(N'Bánh tráng trộn',20000,0,4);
GO
