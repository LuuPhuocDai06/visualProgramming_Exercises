using Microsoft.Win32.SafeHandles;
using System;
namespace DemSoNguyenTo
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            try
            {
                // Nhập số nguyên dương n.
                Console.Write("Nhap n: ");
                n = Convert.ToInt32(Console.ReadLine());
            } catch(FormatException e)
            {
                Console.WriteLine(e.GetBaseException);
            }
            int count = 0;


            Console.Write("So nguyen to tu 2 -> "+n+": ");
            for(int i = 2; i <= n; i++)
            {
                int check = i;
                for(int j = 2; j <= Math.Sqrt(i); j++)
                {
                    if(i % j == 0)
                    {
                        check = 1;
                    }
                }
                if(check == i)
                {
                    Console.Write(i + " ");
                    count++;
                }
            }
            Console.WriteLine("\nTong so luong so nguyen to: " + count);
        }
    }
}