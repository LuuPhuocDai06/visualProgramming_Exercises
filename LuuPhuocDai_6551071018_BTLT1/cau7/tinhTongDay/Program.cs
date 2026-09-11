using System;
namespace TinhTongDay
{
    class Program
    {
        static void Main(string[] args)
        {
            // nhập dữ liệu.
            int even = 0; // số chẵn.
            int odd = 0; // số lẻ.
            int factorial = 1; // giai thừa.

            Console.Write("Nhap n: ");
            int n = Convert.ToInt32(Console.ReadLine());

            for(int i = 1; i <= n; i++)
            {
                if(i % 2 == 0)
                {
                    even += i;
                }
                else if(i % 2 == 1)
                {
                    odd += i;
                }
                   factorial *= i;
                
            }
            Console.WriteLine("Tong so chan trong day la: " + even);
            Console.WriteLine("Tong so le trong day la: " + odd);
            Console.WriteLine($"{n}! = {factorial}");
            
        }
    }
}