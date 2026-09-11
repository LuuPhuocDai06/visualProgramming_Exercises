using System;
using System.Reflection.Metadata;
namespace EnumTrangThai
{

    enum TrangThaiDonHang{
        ChoDuyet = 0, // 0
        DaDuyet = 1, // 1
        DangGiao = 2, // 2
        HoanThanh = 3, // 3
        DaHuy = 4 // 4
    }
    class Program
    {
        static void Main(string[] args)
        {
           
            while (true)
            {
                int n = 0;
                Console.WriteLine("\n == menu ==");
                Console.WriteLine("0. Cho duyet.");
                Console.WriteLine("1. Da duyet.");
                Console.WriteLine("2. Dang giao");
                Console.WriteLine("3. Hoan Thanh");
                Console.WriteLine("4. Da huy.");
                Console.WriteLine("-1.Thoat chuong trinh.");
                Console.Write("Nhap lua chon(0-4): ");
                String input = Console.ReadLine();
                Console.WriteLine("===========");

                n = Convert.ToInt32(input);
                if (n == -1)
                {
                    Console.WriteLine("Ban da thoat chuong trinh");
                    break;
                }

                if (n < 0 || n > 4)
                {
                    Console.WriteLine("khong thoa man dieu kien! vui long lua chon lai.");
                    continue;
                }


                TrangThaiDonHang trangThai = (TrangThaiDonHang)n;

                switch (trangThai)
                {
                    case TrangThaiDonHang.ChoDuyet:
                        Console.WriteLine("Cho duyet: don hang cua ban da duoc cho duyet.");
                        break;

                    case TrangThaiDonHang.DaDuyet:
                        Console.WriteLine("Da duyet: don hang cua ban da duoc duyet.");
                        break;

                    case TrangThaiDonHang.DangGiao:
                        Console.WriteLine("Dang giao: don hang cua ban hien dang giao toi cho ban.");
                        break;

                    case TrangThaiDonHang.HoanThanh:
                        Console.WriteLine("Hoan thanh: don hang cua ban da giao toi noi.");
                        break;

                    case TrangThaiDonHang.DaHuy:
                        Console.WriteLine("Da huy: don hang cua ban da duoc huy.");
                        break;
                }
            }
        }
    }
}