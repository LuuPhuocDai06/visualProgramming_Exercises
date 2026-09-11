using System;
namespace QuanLyMang
{
    class Program
    {
        static void Main(string[] args)
        {
            // Khai báo mảng 1 chiều.
            int[] n = new int[10];
            Console.WriteLine("Nhap cac phan tu cho mang.");
            for(int i = 0; i < n.Length; i++)
            {
                Console.Write($"arr[{i}] = ");
                n[i] = Convert.ToInt32(Console.ReadLine());
            }

            // in các phần tử trong mảng.
            Console.WriteLine("\nCac phan tu trong mang gom: ");
            foreach(int x in n)
            {
                Console.Write(x + " ");
            }

            // tong trung bình các phần tử trong mảng là.
            int tongTB = 0;
            for(int i = 0; i < n.Length; i++)
            {
                tongTB += n[i];
            }
            Console.Write("\n\nTong trung binh = "+tongTB);

            // Giá trị lớn nhất và nhỏ nhất.
            int max = n[0], indexMax = 0;
            int min = n[0], indexMin = 0;

            for(int i = 1; i < n.Length; i++)
            {
                if(max < n[i])
                {
                    max = n[i];
                    indexMax = i;
                }
            }
            for (int i = 1; i < n.Length; i++)
            {
                if (min > n[i])
                {
                    min = n[i];
                    indexMin = i;
                }
            }
            Console.WriteLine($"\n\nMax = {max} tai vi tri arr[{indexMax}]" +
                              $"\nMin =  {min} tai vi tri arr[{indexMin}]");

            // Số lượng chẵn và lẻ.
            int even = 0, odd = 0;
            for(int i = 0; i < n.Length; i++)
            {
                if (n[i] % 2 == 0)
                {
                    even++;
                }
                else
                {
                    odd++;
                }
            }
            Console.WriteLine($"\nSo chan = {even} \nSo le = {odd}");


            // Mảng sau khi đảo ngược.
            Console.WriteLine("\nMang sau khi dao nguoc.");
            for(int i = n.Length -1; i >= 0; i--)
            {
                Console.Write(n[i] + " ");
            }
        }
    }
}