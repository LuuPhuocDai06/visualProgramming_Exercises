using System;
namespace ChuyenDoiNhietDo
{
    class Program
    {
        static void Main(string[] args)
        {
            // các giá trị.
            double celsius; // độ c.

            // Nhập giá trị.
            Console.Write("Nhap celsius(°C): ");
            celsius = Convert.ToDouble(Console.ReadLine());

            // chuyển đổi giá trị sang fahrenheit và Kelvin.
            Console.WriteLine("\nCelsius: " + celsius + "°C");
            Console.WriteLine("\nFehrenheit: " + (celsius * (9 / 5) + 32)+ "°F");
            decimal k = (decimal)celsius + 273.15m;
            Console.WriteLine("\nKelvin: " + k.ToString("F2")+"K");

        }
    }
}