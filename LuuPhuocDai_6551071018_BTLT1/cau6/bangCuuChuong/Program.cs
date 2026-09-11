using System;
namespace BangCuuChuong
{
    class Program
    {
        static void Main(string[] args)
        {
            int n;
            do
            {
                Console.Write("Nhap n: ");
                n = Convert.ToInt32(Console.ReadLine());
                if (n > 9 || n < 1)
                {
                    Console.WriteLine("So khong hop le! vui long nhap lai.");
                }
            } while (n > 9 || n < 1);


            Console.WriteLine("Bang cuu chuong " + n);
            for (int i = 1; i <= 9; i++)
            {
                // Console.WriteLine(n + " * " + i + " = " + (n * i));
                Console.WriteLine($"{n} * {i} = {n * i}");
            }
        }
    }
}