using System;
namespace StructDiemSo
{
    class Program
    {

        struct HocSinh
        {
            public string maSo;
            public string hoten;
            public double diemVan;
            public  double diemToan;
            public double diemAnh;
            public String xepLoai;
        }
        static void Main(string[] args)
        {
            HocSinh[] student = new HocSinh[5];

            // Nhap thông tin cho từng học sinh.
            Console.WriteLine(" ===== Nhap thong tin =====");
            for(int i = 0; i < student.Length; i++)
            {
                Console.WriteLine($"Hoc sinh thu {i+1}.");
                Console.Write("nhap ma so: ");
                student[i].maSo = Console.ReadLine();

                Console.Write("Nhap ho ten: ");
                student[i].hoten = Console.ReadLine();

                Console.Write("Nhap diem van: ");
                student[i].diemVan = Convert.ToDouble(Console.ReadLine());

                Console.Write("Nhap diem toan: ");
                student[i].diemToan = Convert.ToDouble(Console.ReadLine());

                Console.Write("Nhap diem anh: ");
                student[i].diemAnh = Convert.ToDouble(Console.ReadLine());
                Console.Write("\n");
            }
            Console.WriteLine(" =========================");

            // Xuat thong tin cua moi hoc sinh kem theo diem.
            Console.WriteLine("\n========== Bang thong tin ==========");
            Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10}",
                                            "Ma so",
                                            "Ho va ten",
                                            "Diem van",
                                            "Diem toan",
                                            "Diem anh",
                                            "Diem tb"));
            for(int i = 0; i < student.Length; i++)
            {
                Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10:F2}",
                                                student[i].maSo,
                                                student[i].hoten,
                                                student[i].diemVan,
                                                student[i].diemToan,
                                                student[i].diemAnh,
                                                ((student[i].diemAnh + student[i].diemToan + student[i].diemVan) / 3)));
            }



            // Tìm học sinh có điểm trung bình cao nhất.
            double max = 0;
            int index_max = 0;
            for (int i = 0; i < student.Length; i++)
            {
               if(max < ((student[i].diemAnh + student[i].diemToan + student[i].diemVan) / 3))
                {
                    max = (student[i].diemAnh + student[i].diemToan + student[i].diemVan) / 3;
                    index_max = i;
                }
            }

            Console.WriteLine("\n========== hoc sinh co diem trung binh cao nhat ==========");
            Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10}",
                                            "Ma so",
                                            "Ho va ten",
                                            "Diem van",
                                            "Diem toan",
                                            "Diem anh",
                                            "Diem tb"));
            Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10:F2}",
                                                student[index_max].maSo,
                                                student[index_max].hoten,
                                                student[index_max].diemVan,
                                                student[index_max].diemToan,
                                                student[index_max].diemAnh,
                                                ((student[index_max].diemAnh + student[index_max].diemToan + student[index_max].diemVan) / 3)));


            // in tất cả học sinh  kèm theo xếp loại.
            for(int i = 0; i < student.Length; i++)
            {
                double dtb = (student[i].diemAnh + student[i].diemToan + student[i].diemVan) / 3;
                if (dtb >= 8.5 && dtb <= 10.0)
                {
                    student[i].xepLoai = "Gioi";
                }
                else if(dtb >= 6.5 && dtb < 8.5)
                {
                    student[i].xepLoai = "Kha";
                }
                else if(dtb >= 5.0 && dtb < 6.5)
                {
                    student[i].xepLoai = "Trung binh";
                }
                else
                {
                    student[i].xepLoai = "Yeu";
                }
            }

            Console.WriteLine("\n========== Bang thong tin ==========");
            Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10} {6, -12}",
                                            "Ma so",
                                            "Ho va ten",
                                            "Diem van",
                                            "Diem toan",
                                            "Diem anh",
                                            "Diem tb",
                                            "Xep loai"));
            for (int i = 0; i < student.Length; i++)
            {
                Console.WriteLine(string.Format("{0, -7} {1, -20} {2, -10} {3, -10} {4, -10} {5, -10:F2} {6, -12}",
                                                student[i].maSo,
                                                student[i].hoten,
                                                student[i].diemVan,
                                                student[i].diemToan,
                                                student[i].diemAnh,
                                                ((student[i].diemAnh + student[i].diemToan + student[i].diemVan) / 3),
                                                student[i].xepLoai));
            }
        }
    }
}