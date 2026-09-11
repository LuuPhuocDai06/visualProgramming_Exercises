using System;
using System.Security.Cryptography;
namespace QuanLySinhVien
{
    class Program
    {

        // struct.
        struct sinhVien
        {
            public String massv;
            public string hoTen;
            public double[] diem;
            public double diemTB;
        }
        static void Main(string[] args)
        {
            sinhVien[] sv = new sinhVien[2];

            // Nhập thông tin sinh viên.
            Console.WriteLine(" === Nhap thong tin sinh vien ===");
            for(int i = 0; i < sv.Length; i++)
            {
                double diem = 0;
                Console.WriteLine("sinh vien thu "+(i+1));
                Console.Write("Nhap ma so sinh vien: ");
                sv[i].massv = Console.ReadLine();

                Console.Write("Nhap ho va ten sinh vien: ");
                sv[i].hoTen = Console.ReadLine();

                sv[i].diem = new double[5];
                for(int j = 0; j < sv[i].diem.Length; j++)
                {
                    Console.Write($"Nhap diem mon thu {j+1}: ");
                    sv[i].diem[j] = Convert.ToDouble(Console.ReadLine());
                    diem += sv[i].diem[j];
                }
                sv[i].diemTB = diem / 5;
                Console.Write("\n");
            }

            // Xuất thông tin sinh vien.
            Console.WriteLine("\n =============== Thong tin sinh vien ===============");
            Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -7} {3, -7} {4, -7} {5, -7} {6, -7} {7, -7}",
                                            "MSSV",
                                            "Ho va ten",
                                            "Diem1",
                                            "Diem2",
                                            "Diem3",
                                            "Diem4",
                                            "Diem5",
                                            "DiemTB"));
            for(int i = 0; i < sv.Length; i++)
            {
                Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -7} {3, -7} {4, -7} {5, -7} {6, -7} {7, -7}",
                                                sv[i].massv,
                                                sv[i].hoTen,
                                                sv[i].diem[0],
                                                sv[i].diem[1],
                                                sv[i].diem[2],
                                                sv[i].diem[3],
                                                sv[i].diem[4],
                                                sv[i].diemTB));
            }

            // sắp xếp theo điểm trung bình giảm dần bằng bubble sort.
            Console.WriteLine("\n === Danh sach sau khi sap xep theo diem trung binh giam dan ===");
            for(int i = 0; i < sv.Length - 1; i++)
            {
                sinhVien temp = new sinhVien();
                for (int j = 0; j < sv.Length - 1 - i; j++)
                {
                    if (sv[j].diemTB < sv[j + 1].diemTB)
                    {
                        temp = sv[j];
                        sv[j] = sv[j + 1];
                        sv[j + 1] = temp; 
                    }
                }
            }

            // Xuat danh sach sau khi sap xep.
            Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -7} {3, -7} {4, -7} {5, -7} {6, -7} {7, -7}",
                                            "MSSV",
                                            "Ho va ten",
                                            "Diem1",
                                            "Diem2",
                                            "Diem3",
                                            "Diem4",
                                            "Diem5",
                                            "DiemTB"));
            for (int i = 0; i < sv.Length; i++)
            {
                Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -7} {3, -7} {4, -7} {5, -7} {6, -7} {7, -7}",
                                                sv[i].massv,
                                                sv[i].hoTen,
                                                sv[i].diem[0],
                                                sv[i].diem[1],
                                                sv[i].diem[2],
                                                sv[i].diem[3],
                                                sv[i].diem[4],
                                                sv[i].diemTB));
            }

            // Tìm kiếm sinh viên bằng mssv.
            Console.Write("\nNhap ma so sinh vien can tim kiem: ");
            string mssv = Console.ReadLine();

            int b = 0;
            while (true)
            {
                if (sv[b].massv.Trim().Contains(mssv))
                {
                    Console.WriteLine(" ============ Thong tin sinh vien tim kiem ============ ");
                    Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -7} {3, -7} {4, -7} {5, -7} {6, -7} {7, -7}",
                                            "MSSV",
                                            "Ho va ten",
                                            "Diem1",
                                            "Diem2",
                                            "Diem3",
                                            "Diem4",
                                            "Diem5",
                                            "DiemTB"));
                    
                    Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -7} {3, -7} {4, -7} {5, -7} {6, -7} {7, -7}",
                                                sv[b].massv,
                                                sv[b].hoTen,
                                                sv[b].diem[0],
                                                sv[b].diem[1],
                                                sv[b].diem[2],
                                                sv[b].diem[3],
                                                sv[b].diem[4],
                                                sv[b].diemTB));
                    break;
                }
                b++;
            }

        }
    }
}