IF DB_ID('QL_DatTour') IS NULL CREATE DATABASE QL_DatTour;
GO
USE QL_DatTour;
GO
IF OBJECT_ID('dbo.DatTour')   IS NOT NULL DROP TABLE dbo.DatTour;
IF OBJECT_ID('dbo.KhachHang') IS NOT NULL DROP TABLE dbo.KhachHang;
IF OBJECT_ID('dbo.Tour')      IS NOT NULL DROP TABLE dbo.Tour;
GO
CREATE TABLE dbo.Tour(
    MaTour        INT IDENTITY(1,1) PRIMARY KEY,
    TenTour       NVARCHAR(100) NOT NULL,
    DiemDen       NVARCHAR(50)  NOT NULL,
    NgoaiTe       CHAR(3)       NOT NULL CHECK (NgoaiTe IN ('THB','KRW','JPY','USD')),
    GiaNgoaiTe    DECIMAL(18,2) NOT NULL CHECK (GiaNgoaiTe > 0),
    NgayKhoiHanh  DATE          NOT NULL,
    SoChoToiDa    INT           NOT NULL CHECK (SoChoToiDa > 0)
);
CREATE TABLE dbo.KhachHang(
    MaKH        INT IDENTITY(1,1) PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15)   NOT NULL,
    Email       NVARCHAR(100) NULL
);
CREATE TABLE dbo.DatTour(
    MaDat       INT IDENTITY(1,1) PRIMARY KEY,
    MaKH        INT NOT NULL REFERENCES dbo.KhachHang(MaKH),
    MaTour      INT NOT NULL REFERENCES dbo.Tour(MaTour),
    SoNguoi     INT NOT NULL CHECK (SoNguoi > 0),
    NgayDat     DATETIME      NOT NULL DEFAULT GETDATE(),
    TyGia       DECIMAL(18,4) NOT NULL,       -- tỷ giá tại thời điểm đặt (1 ngoại tệ = ? VND)
    TongTienVND DECIMAL(18,0) NOT NULL
);
GO
DECLARE @h DATE = CAST(GETDATE() AS DATE);
INSERT dbo.Tour(TenTour,DiemDen,NgoaiTe,GiaNgoaiTe,NgayKhoiHanh,SoChoToiDa) VALUES
(N'Bangkok - Pattaya 5N4Đ',   N'Thái Lan',  'THB',   9500, DATEADD(DAY,30,@h), 20),
(N'Seoul - Nami 5N4Đ',        N'Hàn Quốc',  'KRW', 850000, DATEADD(DAY,45,@h), 15),
(N'Tokyo - Phú Sĩ 6N5Đ',      N'Nhật Bản',  'JPY', 120000, DATEADD(DAY,60,@h), 12),
(N'Phuket - Phi Phi 4N3Đ',    N'Thái Lan',  'THB',  8200, DATEADD(DAY,40,@h), 18),
(N'Maldives nghỉ dưỡng 4N3Đ', N'Maldives',  'USD',   950, DATEADD(DAY,75,@h), 10);
INSERT dbo.KhachHang(HoTen,SoDienThoai,Email) VALUES
(N'Trần Thị Mai',   '0987654321', N'mai.tran@example.com'),
(N'Lê Văn An',      '0912345678', N'an.le@example.com'),
(N'Phạm Minh Hùng', '0976543210', NULL),
(N'Hoàng Thu Thảo', '0933221100', N'thao.hoang@example.com');
INSERT dbo.DatTour(MaKH,MaTour,SoNguoi,NgayDat,TyGia,TongTienVND) VALUES
(1,1,3,DATEADD(DAY,-5,GETDATE()),  778.0000, 22173000),
(2,2,2,DATEADD(DAY,-3,GETDATE()),   18.5000, 31450000),
(3,3,1,DATEADD(DAY,-1,GETDATE()),  170.0000, 20400000);
GO
