using System;
namespace GameDoanSo
{
    class Program
    {
        static void Main(string[] args)
        {

            // Random tu 1-100.
            Random rdm = new Random();

            // Mang de luu ket qua.
            int[] diem = new int[10];

            int soLuotChoi = 0;
            int choice_starGame;
            int diem_game = 100;
            List<int> rankDiem = new List<int>();
            do
            {
                Console.WriteLine("\n == Game doan so ==");
                Console.WriteLine("1. Bat dau choi.");
                Console.WriteLine("2. Xem luat choi.");
                Console.WriteLine("3. In tong ket gan nhat.");
                Console.WriteLine("0. Thoat tro choi.");
                Console.Write("Nhap lua chon: ");

             
                choice_starGame = Convert.ToInt32(Console.ReadLine());
                if (choice_starGame > 3 || choice_starGame < 0)
                {
                    Console.WriteLine("Lua chon khong ton tai! vui long chon lai.");
                    continue;
                }

                

                // Bat dau choi.
                if(choice_starGame == 1)
                {
                    int n = rdm.Next(1, 101);
                    int answer = 0;
                    int soLuotTraLoi = 0;

                    while (true)
                    {
                        if(soLuotTraLoi == 1 && answer != 0)
                        {
                            diem_game -= 10;
                        }
                        Console.WriteLine(n);
                        Console.WriteLine("\n == Game doan so ==");
                        Console.WriteLine("Diem so cua ban: " + diem_game);
                        Console.WriteLine("So luot(max = 7): " + soLuotTraLoi);

                        if (answer == 0)
                        {
                            Console.WriteLine("Goi y: Trong!\n");
                        }
                        else if ((answer - n) > 0)
                        {
                            Console.WriteLine("Goi y: Thap hon!\n");
                            diem_game -= 10;
                        }
                        else if ((answer - n) < 0)
                        {
                            Console.WriteLine("Goi y: Cao hon!\n");
                            diem_game -= 10;
                        }


                        Console.WriteLine("An 0 neu muon thoat game.");

                        Console.Write("Nhap dan an cua ban(1 - 100): ");

                        try
                        {
                            answer = Convert.ToInt32(Console.ReadLine());
                        } catch(FormatException ex)
                        {
                            Console.WriteLine(ex.GetBaseException());
                            continue;
                        }
                        if (answer < 0 || answer > 100)
                        {
                            Console.WriteLine("Dap an vuot qua quy dinh! vui long nhap lai.");
                            continue;
                        }

                        if ((answer - n) == 0)
                        {
                            Console.WriteLine("Goi y: Chinh xac!\n");
                            if (diem.Length == 10)
                            {
                                soLuotChoi = 0;
                                diem[soLuotChoi++] = diem_game;
                                rankDiem.Add(diem[soLuotChoi]);
                                break;
                            }
                            diem[soLuotChoi++] = diem_game;
                            rankDiem.Add(diem[soLuotChoi]);
                            break;
                        }


                        if (answer == 0)
                        {
                            break;
                        }
                        if(soLuotTraLoi == 7)
                        {
                            Console.WriteLine("Da du 7 luot choi.");
                            if (diem.Length == 10)
                            {
                                soLuotChoi = 0;
                                diem[soLuotChoi++] = diem_game;
                                rankDiem.Add(diem[soLuotChoi]);
                                break;
                            }
                            diem[soLuotChoi++] = diem_game;
                            rankDiem.Add(diem[soLuotChoi]);
                            break;
                        }

                        soLuotTraLoi++;
                    }

                    string choice_nextGame;
                    while (true)
                    {
                        Console.Write("Ban co muon tiep tuc game moi khong(y/n)?: ");
                        choice_nextGame = Console.ReadLine();
                        if((choice_nextGame.ToLower().Equals("y")) || choice_nextGame.ToLower().Equals("n")){
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Lua chon khong hop le!");
                            continue;
                        }
                    }

                    if (choice_nextGame.ToLower().Equals("y"))
                    {
                        continue;
                    }
                    else
                    {
                        choice_starGame = 0;
                    }
                }

                // xem luat choi.
                if(choice_starGame == 2)
                {
                    Console.WriteLine("\n == Luat choi Game doan so ==");
                    Console.WriteLine("1. Doan so tu 1 - 100.");
                    Console.WriteLine("2. Neu qua 7 luot doan ma khong trung thi het van game.");
                    Console.WriteLine("3. Trong 7 van neu khong doan trung thi tru 10 diem (so diem max = 100).");
                    continue;
                }    

                if(diem_game == 0)
                {
                    Console.WriteLine("So diem cua ban het!!!");
                    Console.WriteLine("Ket thuc tro choi!");
                    diem_game = 100; // reset lai diem.
                    continue;
                }

                // thong ke.
                if(choice_starGame == 3)
                {
                    int diemTb = 0;
                    for(int i = 0; i < rankDiem.Count; i++)
                    {
                        diemTb += rankDiem[i];
                    }
                    
                    Console.WriteLine("\n == Thong ke gan nhat (10 lan choi) == ");
                    Console.WriteLine("So van choi: " + soLuotChoi);
                    Console.WriteLine("Diem cao nhat: " + rankDiem.Max());
                    Console.WriteLine("Diem thap nhat: " + rankDiem.Min());
                    Console.WriteLine("Diem trung binh: "+(diemTb / rankDiem.Count));
                    continue;
                }

            } while (choice_starGame != 0);
        }
    }
}