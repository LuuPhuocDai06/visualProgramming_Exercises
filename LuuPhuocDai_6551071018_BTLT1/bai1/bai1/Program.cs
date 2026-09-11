using System;
namespace bai1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Khai báo biến.
            String name;
            int age;
            double height;
            float weight;
            bool daTotNghiep;

            // Nhập giá trị.
            Console.WriteLine("\n === Nhập thông tin === ");
            Console.Write("Nhap ho ten: ");
            name = Console.ReadLine();

            Console.Write("Nhap so tuoi: ");
            age = Convert.ToInt32(Console.ReadLine());


            Console.Write("Nhap so chieu cao(cm): ");
            height = Convert.ToDouble(Console.ReadLine());

            Console.Write("Nhap so can nang(kg): ");
            weight = (float)Convert.ToDouble(Console.ReadLine());

            Console.Write("Ban da tot nghiep chua(nhap 1 la da tot nghiep, nhap 0 khi chua tot nghiep): ");
            int check = Convert.ToInt32(Console.ReadLine());
            if (check == 1)
            {
                daTotNghiep = true;
            }
            else
            {
                daTotNghiep = false;
            }
            Console.WriteLine("==================\n");

            // Giá trị trả về
            Console.WriteLine("\n === Xuất thông tin === ");
            Console.WriteLine("Ho ten: " + name);
            Console.WriteLine("Tuoi: " + age);
            Console.WriteLine("Chieu cao: " + height + "cm");
            Console.WriteLine("Can nang: " + weight + "kg");
            if (daTotNghiep)
            {
                Console.WriteLine("Ban da tot nghiep!");
            }
            else
            {
                Console.WriteLine("Ban chua tot nghiep!");
            }
            Console.WriteLine("==================\n");
        }
    }
}