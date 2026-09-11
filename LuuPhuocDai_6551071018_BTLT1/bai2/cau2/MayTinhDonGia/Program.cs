using System;
namespace mayTinhDonGia
{
    class Program
    {
        static void Main(string[] args)
        {
            //Các giá trị.
            float a, b;

            // Nhập thông tin.
            Console.WriteLine("===== Nhap hai so thuc =====");
            Console.Write("Nhap a: ");
            a = Convert.ToSingle(Console.ReadLine());

            Console.Write("Nhap b: ");
            b = Convert.ToSingle(Console.ReadLine());
            Console.WriteLine("============================\n");

            // phép tính.
            // phép tính cộng.
            Console.WriteLine(a + " + " + b + " = " + (a + b).ToString("F2"));

            // phép trừ.
            Console.WriteLine(a+ " - " + b + " = "+(a - b).ToString("F2"));

            // Phép chia.
            Console.WriteLine(a + " / " + b+ " = " + (a / b).ToString("F2"));


            // Phép chia lấy phần nguyên.
            Console.WriteLine("chia lay phan nguyen: " + (int)(a / b));

            // Phép chia lấy phân dư.
            Console.WriteLine("Chia lay phan du: " + (int)(a % b));

            // Luy thua.
            Console.WriteLine(a + "^" + b + "= " + (Math.Pow(a, b)).ToString("F2"));
        }
    }
}