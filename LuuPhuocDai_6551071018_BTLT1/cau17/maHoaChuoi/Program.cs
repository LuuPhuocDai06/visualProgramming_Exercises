using System;
using System.Xml.Serialization;
namespace MaHoaChuoi
{
    // enum.
    enum KieuMaHoa
    {
        Caesar,
        Reverse,
        DoiChu
    }
    class Program
    {

        // Hàm Caeser.
        static string Caeser(string chuoiKytu)
        {
            char[] result = new char[chuoiKytu.Length];
            for(int i= 0;i < chuoiKytu.Length; i++)
            {
                char c = chuoiKytu[i];
                if(c >= 'a' && c <= 'z')
                {
                    result[i] = (char)('a' + (c - 'a' + 3) % 26);
                }
                else if(c >= 'A' && c <= 'Z')
                {
                    result[i] = (char)('A' + (c - 'A' + 3) % 26);
                }
                else
                {
                    result[i] = c;
                }
            }
            return new string(result);
        }


        // hàm reveser.
        static string reveser(string chuoiKyTu)
        {
            char[] result = new char[chuoiKyTu.Length];

            for(int i = 0; i < chuoiKyTu.Length; i++)
            {
                char c = chuoiKyTu[chuoiKyTu.Length - 1 - i];
            }
            return new string(result);
        }


        // Hàm DoiChu(đổi chữ thường in hoa, in hoa sang thường).
        static string doiChu(string chuoiKyTu)
        {
            char[] result = new char[chuoiKyTu.Length];
            int i = 0;
            foreach(char x in chuoiKyTu)
            {
                if (x >= 'a' && x <= 'z')
                {
                    result[i] = (char)(x - 32);
                }
                else if(x >= 'A' && x <= 'Z')
                {
                    result[i] = (char)(x + 32);
                }
                else
                {
                    result[i] = x;
                }
                i++;
            }
            return new string(result);
        }

        // hàm main.
        static void Main(string[] args) {
            do
            {
                try
                {
                    Console.Write("\n\nNhap chuoi ky tu: ");
                    string chuoiKyTu = Console.ReadLine();

                    if (string.IsNullOrEmpty(chuoiKyTu))
                    {
                        throw new Exception("Chuoi khong dc rong!");
      
                    }

                    Console.WriteLine("== Cac chuc nang ==");
                    Console.WriteLine("0. Caeser.");
                    Console.WriteLine("1. Reveser.");
                    Console.WriteLine("2. Doi chu(in hoa - chu thuong).");
                    Console.Write("Chon chuc nang: ");
                    KieuMaHoa choice = (KieuMaHoa)int.Parse(Console.ReadLine());

                    string ketQua = "";

                    switch (choice)
                    {
                        case KieuMaHoa.Caesar:
                            ketQua = Caeser(chuoiKyTu);
                            break;

                        case KieuMaHoa.Reverse:
                            ketQua = reveser(chuoiKyTu);
                            break;

                        case KieuMaHoa.DoiChu:
                            ketQua = doiChu(chuoiKyTu);
                            break;

                        default:
                            Console.WriteLine("Lụa chon khong hop le! vui long chon lai.");
                            break;
                    }
                    Console.WriteLine("Chuoi ky tu: " + ketQua);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetBaseException);
                }

                Console.Write("Nhan y/Y de tiep va nhan n/N de thoat: ");
                string x = Console.ReadLine();

                if (x.Contains("n") || x.Contains("N"))
                {
                    break;
                }

            } while (true);
        }
    }
}