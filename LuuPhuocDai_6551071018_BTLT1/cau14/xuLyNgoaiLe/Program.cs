using System;
using System.ComponentModel.Design;
namespace XuLyNgoaiLe
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = 0, b = 0;
            while (true)
            {
                Console.WriteLine("\n=== Nhap a va b de tinh toan ===");
                try
                {
                    Console.Write("Nhap a: ");
                    a = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Nhap b: ");
                    b = Convert.ToDouble(Console.ReadLine());
                } catch(FormatException e)
                {
                    Console.WriteLine(e.GetBaseException);
                }

                Console.WriteLine("=== chon phep tinh ===");
                Console.WriteLine("1. Cong(+).");
                Console.WriteLine("2. Tru(-).");
                Console.WriteLine("3. Nhan(+).");
                Console.WriteLine("4. Chia(+).");
                Console.WriteLine("0. thoat.");
                int choice = 0;

                try
                {
                    Console.Write("Nhap lua chon: ");
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.GetBaseException);
                }

                if(choice == 0)
                {
                    break;
                }

                switch (choice)
                {
                    case 1:
                        try
                        {
                            Console.WriteLine($"{a} + {b} = {a + b}");
                        } catch(OverflowException e)
                        {
                            Console.WriteLine(e.GetBaseException);
                        }
                        break;

                    case 2:
                        Console.WriteLine($"{a} - {b} = {a - b}");
                        break;

                    case 3:
                        Console.WriteLine($"{a} * {b} = {a * b}");
                        break;

                    case 4:
                        try
                        {
                            Console.WriteLine($"{a} / {b} = {a / b}");
                        }
                        catch (DivideByZeroException e)
                        {
                            Console.WriteLine(e.GetBaseException);
                        }
                        break;

                    default:
                        Console.WriteLine("Lua chon khong hop le!!!");
                        break;
                }
                Console.Write("ban co muon tiep tuc khong nhan y/Y de tiep, nhan n/N de thoat: ");
                string exit = Console.ReadLine();

                if(exit.ToLower().Equals("n"))
                {
                    Console.WriteLine("Ban da thoat chuong trinh");
                    break;
                }
            }
        }
    }
}