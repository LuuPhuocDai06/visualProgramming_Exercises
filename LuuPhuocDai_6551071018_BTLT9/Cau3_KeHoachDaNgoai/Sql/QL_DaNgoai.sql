IF DB_ID('QL_DaNgoai') IS NULL CREATE DATABASE QL_DaNgoai;
GO
USE QL_DaNgoai;
GO
IF OBJECT_ID('dbo.ChuyenDi') IS NOT NULL DROP TABLE dbo.ChuyenDi;
GO
CREATE TABLE dbo.ChuyenDi(
    MaChuyen    INT IDENTITY(1,1) PRIMARY KEY,
    TenChuyen   NVARCHAR(100) NOT NULL,
    DiaDiem     NVARCHAR(100) NOT NULL,
    NgayDi      DATE          NOT NULL,
    SoNguoi     INT           NOT NULL CHECK (SoNguoi >= 1),
    NganSach    DECIMAL(18,0) NOT NULL CHECK (NganSach >= 0),
    PhuongTien  NVARCHAR(30)  NOT NULL,
    DaHoanThanh BIT           NOT NULL DEFAULT 0,
    GhiChu      NVARCHAR(200) NULL
);
GO
DECLARE @h DATE = CAST(GETDATE() AS DATE);
INSERT dbo.ChuyenDi(TenChuyen,DiaDiem,NgayDi,SoNguoi,NganSach,PhuongTien,DaHoanThanh,GhiChu) VALUES
(N'Cắm trại đồi chè',      N'Đà Lạt',  DATEADD(DAY,3,@h),  6, 3500000, N'Ô tô',     0, NULL),
(N'Trekking Fansipan',     N'Sa Pa',   DATEADD(DAY,5,@h),  4, 6000000, N'Tàu hỏa',  0, N'Mang áo ấm'),
(N'Tắm biển cuối tuần',    N'Vũng Tàu',DATEADD(DAY,7,@h),  8, 2500000, N'Xe khách', 0, NULL),
(N'Phố cổ về đêm',         N'Hội An',  DATEADD(DAY,10,@h), 3, 5000000, N'Ô tô',     0, NULL),
(N'Săn mây Tà Xùa',        N'Sơn La',  DATEADD(DAY,14,@h), 5, 4500000, N'Xe khách', 0, NULL),
(N'Lặn ngắm san hô',       N'Nha Trang',DATEADD(DAY,20,@h),4, 7000000, N'Xe khách', 0, NULL),
(N'Thăm cố đô',            N'Huế',     DATEADD(DAY,28,@h), 6, 4000000, N'Tàu hỏa',  0, NULL),
(N'Đảo ngọc Phú Quốc',     N'Phú Quốc',DATEADD(DAY,35,@h), 2, 9000000, N'Ô tô',     0, N'Đặt vé máy bay/phà trước');
GO
