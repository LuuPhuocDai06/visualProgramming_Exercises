using System;
namespace RefOfParams
{
    class Program
    {
        // Hàm hoán vị.
        static void hoanVi(ref int a, ref int b)
        {
            int c = 0;
            c = a;
            a = b;
            b = c;
        }

        // Hàm tachSo.
        static void tachSo(double n, out long phanNguyen, out double phanThapPhan)
        {
            phanNguyen = (long)n;
            phanThapPhan = n % 1;
        }

        // Hàm tổng.
        static void tinhTong(out int tong, params int[] soNguyen)
        {
            tong = 0;
            foreach(int x in soNguyen)
            {
                tong += x;
            }
        }



        // Hàm main.
        static void Main(string[] args)
        {
            // Hoán đổi a và b.
            int a = 10, b = 20;
            Console.WriteLine($"Chua hoan vi a = {a}, b = {b}.");
            hoanVi(ref a, ref b);
            Console.WriteLine($"Da hoan vi a = {a}, b = {b}.");

            // Tách phần nguyên với phần thập phân của số thực.
            double n = 10.15;
            long phanNuyen;
            double phanThapPhan;
            Console.WriteLine($"\nkhi chua tach n = {n}");
            tachSo(n, out phanNuyen, out phanThapPhan);
            Console.WriteLine($"Sau khi tach, phan nguyen = {phanNuyen}, phan thap phan = {phanThapPhan.ToString("F2")}.");


            // Tính tổng dãy số a, b, c...
            int tong;
            tinhTong(out tong, 1, 2, 3, 4, 5, 6, 7);
            Console.WriteLine($"\nTong tu 1 -> 7 la: {tong}");
        }
    }
}