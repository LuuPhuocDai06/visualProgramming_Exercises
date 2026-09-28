using System;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    // Form khởi động: cho phép chọn mở từng bài tập (Câu 1 -> Câu 6) để kiểm tra độc lập.
    public class FormLauncher : Form
    {
        public FormLauncher()
        {
            Text = "Bài tập chương 5 - Xác thực dữ liệu";
            Width = 420;
            Height = 380;
            StartPosition = FormStartPosition.CenterScreen;

            var lbl = new Label
            {
                Text = "Chọn bài tập để chạy thử:",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            Controls.Add(lbl);

            string[] names =
            {
                "Câu 1 - Form đăng ký tài khoản",
                "Câu 2 - Form đặt phòng khách sạn",
                "Câu 3 - Form nhập điểm học sinh",
                "Câu 4 - Danh bạ điện thoại",
                "Câu 5 - Bán vé xem phim (Dialog chọn ghế)",
                "Câu 6 - Quản lý ghi chú công việc (MDI)"
            };

            for (int i = 0; i < names.Length; i++)
            {
                var btn = new Button
                {
                    Text = names[i],
                    Dock = DockStyle.Top,
                    Height = 40,
                    Tag = i + 1
                };
                btn.Click += Btn_Click;
                Controls.Add(btn);
            }
            // Vì Dock=Top thêm control sau sẽ đẩy control trước xuống dưới,
            // nên đảo thứ tự hiển thị bằng cách thêm label sau cùng thật ra đã ổn với WinForms (LIFO trong Controls).
        }

        private void Btn_Click(object? sender, EventArgs e)
        {
            var btn = (Button)sender!;
            int cau = (int)btn.Tag!;
            Form f = cau switch
            {
                1 => new FormDangKy(),
                2 => new FormDatPhong(),
                3 => new FormNhapDiem(),
                4 => new FormDanhBa(),
                5 => new FormBanVe(),
                6 => new FormChinh(),
                _ => throw new InvalidOperationException()
            };
            f.Show();
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormLauncher());
        }
    }
}
