using System;
namespace MaTranSoHoc
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ma trận 3x3.
            int[,] matrix = new int[3, 3];

            // Nhập giá trị cho các ma trận.
            Console.WriteLine("Nhap cac phan tu cho ma tran 3x3.");
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    Console.Write($"matrix[{i + 1}][{j + 1}] = ");
                    matrix[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }

            // Xuất ma trận 3x3.
            Console.WriteLine("\nMa tran vuong 3x3.");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.Write("\n");
            }

            // tổng các phần tử.
            int tong = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tong += matrix[i, j];
                }
            }
            Console.WriteLine($"\nTong cac phan tu trong ma tran 3x3: {tong}");

            // Tổng đường chéo chính và đường chéo phủ.
            int mainDiagonal = 0;
            int secondaryDiagonal = 0;
            int row = 3;
            for(int i = 0; i < row; i++)
            {
                mainDiagonal += matrix[i, i];
                secondaryDiagonal += matrix[i, row - 1 - i];
            }
            Console.WriteLine($"\nTong duong cheo chinh: {mainDiagonal}\nTong duong cheo phu: {secondaryDiagonal}");


            // Ma tran chuyển vị.
            Console.WriteLine("\nMa tran khi chuyen vi.");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"{matrix[j, i]} ");
                }
                Console.Write("\n");
            }
        }
    }
}