using System;
namespace KiemTraSo
{
    class Program
    {
        static void Main(string[] args)
        {
            // so nguyen.
            Console.Write("Nhap so nguyen n: ");
            int n = Convert.ToInt32(Console.ReadLine());

            if(n < 0)
            {
                Console.WriteLine(n + " la so am!");
            }
            else if(n == 0)
            {
                Console.WriteLine(n + " = 0");
            }
            else
            {
                Console.WriteLine(n + " la so nguyen duong!");
            }

            if(n % 3 == 0)
            {
                Console.WriteLine(n + " chia het cho 3");
            }
            else
            {
                Console.WriteLine(n + " khong chia het cho 3");
            }

            if(n % 3 == 0 && n % 2 == 0)
            {
                Console.WriteLine(n + " chia het cho 2 va 3!");
            }
            else
            {
                Console.WriteLine(n + " khong chia het cho 2 va 3!");
            }
        }
    }
}