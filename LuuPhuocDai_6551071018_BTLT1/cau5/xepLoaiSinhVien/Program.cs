using System;
namespace XepLoaiSinhVien
{
    class Program
    {
        static void Main(string[] args)
        {
            // nhap so diem.
            double diem;

            Console.Write("Nhap so diem trung binh: ");
            diem = Convert.ToDouble(Console.ReadLine());

            if(diem >= 9.0 && diem <= 10.0)
            {
                Console.WriteLine("Hoc sinh thuoc hoc luc Xuat Sac.");
            }
            else if(diem >= 8.0 && diem < 9.0)
            {
                Console.WriteLine("Hoc sinh thuoc hoc luc Gioi.");
            }
            else if(diem >= 6.5 && diem < 8.0)
            {
                Console.WriteLine("Hoc sinh thuoc hoc luc Kha.");
            }
            else if(diem >= 5.0 && diem < 6.5)
            {
                Console.WriteLine("Hoc sinh thuoc hoc luc Trung Binh.");
            }
            else if(diem < 5.0)
            {
                Console.WriteLine("Hoc sinh thuoc hoc luc Yeu.");
            }
            else
            {
                Console.WriteLine("Diem khong hop le!");
            }
        }
    }
}