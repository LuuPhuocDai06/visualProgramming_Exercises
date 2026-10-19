IF DB_ID('QL_ChiTieu') IS NULL CREATE DATABASE QL_ChiTieu;
GO
USE QL_ChiTieu;
GO
IF OBJECT_ID('dbo.ChiTieu') IS NOT NULL DROP TABLE dbo.ChiTieu;
GO
CREATE TABLE dbo.ChiTieu(
    MaCT    INT IDENTITY(1,1) PRIMARY KEY,
    NgayChi DATE          NOT NULL,
    NoiDung NVARCHAR(100) NOT NULL,
    LoaiChi NVARCHAR(30)  NOT NULL,
    SoTien  DECIMAL(18,0) NOT NULL CHECK (SoTien > 0),
    GhiChu  NVARCHAR(200) NULL
);
GO
DECLARE @h DATE = CAST(GETDATE() AS DATE);
INSERT dbo.ChiTieu(NgayChi, NoiDung, LoaiChi, SoTien, GhiChu) VALUES
(DATEADD(DAY,-0,@h), N'Cà phê với bạn',      N'Ăn uống', 45000,  NULL),
(DATEADD(DAY,-1,@h), N'Cơm trưa',            N'Ăn uống', 35000,  NULL),
(DATEADD(DAY,-2,@h), N'Xăng xe',             N'Đi lại',  60000,  NULL),
(DATEADD(DAY,-3,@h), N'Photo giáo trình',    N'Học tập', 25000,  N'Môn Lập trình C#'),
(DATEADD(DAY,-4,@h), N'Xem phim',            N'Giải trí',90000,  NULL),
(DATEADD(DAY,-5,@h), N'Trà sữa',             N'Ăn uống', 40000,  NULL),
(DATEADD(DAY,-6,@h), N'Mua sách tham khảo',  N'Học tập', 150000, NULL),
(DATEADD(DAY,-7,@h), N'Vé xe buýt tháng',    N'Đi lại',  200000, NULL),
(DATEADD(DAY,-8,@h), N'Ăn sáng',             N'Ăn uống', 25000,  NULL),
(DATEADD(DAY,-9,@h), N'Chơi game',           N'Giải trí',50000,  NULL),
(DATEADD(DAY,-10,@h),N'Quà sinh nhật bạn',   N'Khác',    120000, NULL),
(DATEADD(DAY,-11,@h),N'Cơm tối',             N'Ăn uống', 40000,  NULL);
GO
