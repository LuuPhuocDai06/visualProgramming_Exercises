using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    /// <summary>
    /// CÂU 1: Form đăng ký tài khoản ứng dụng giao đồ ăn.
    /// Kiểm tra hợp lệ toàn bộ khi bấm Đăng ký, dùng ErrorProvider để báo lỗi từng field.
    /// </summary>
    public class FormDangKy : Form
    {
        private readonly TextBox txtHoTen = new();
        private readonly TextBox txtSDT = new();
        private readonly TextBox txtEmail = new();
        private readonly TextBox txtMatKhau = new() { PasswordChar = '*' };
        private readonly TextBox txtXacNhanMK = new() { PasswordChar = '*' };
        private readonly Button btnDangKy = new() { Text = "Đăng ký" };
        private readonly Button btnHuy = new() { Text = "Hủy" };
        private readonly ErrorProvider errorProvider1 = new();

        public FormDangKy()
        {
            Text = "FormDangKy - Đăng ký tài khoản";
            Width = 420;
            Height = 320;
            StartPosition = FormStartPosition.CenterScreen;

            BuildLayout();

            btnDangKy.Click += BtnDangKy_Click;
            btnHuy.Click += BtnHuy_Click;
        }

        private void BuildLayout()
        {
            var table = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                Height = 200,
                Padding = new Padding(10)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            void AddRow(string label, TextBox tb)
            {
                table.RowCount++;
                table.Controls.Add(new Label { Text = label, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, table.RowCount - 1);
                tb.Dock = DockStyle.Fill;
                table.Controls.Add(tb, 1, table.RowCount - 1);
            }

            AddRow("Họ tên:", txtHoTen);
            AddRow("Số điện thoại:", txtSDT);
            AddRow("Email:", txtEmail);
            AddRow("Mật khẩu:", txtMatKhau);
            AddRow("Xác nhận mật khẩu:", txtXacNhanMK);

            Controls.Add(table);

            var panelBtn = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };
            panelBtn.Controls.Add(btnHuy);
            panelBtn.Controls.Add(btnDangKy);
            Controls.Add(panelBtn);
        }

        /// <summary>
        /// Kiểm tra hợp lệ toàn bộ form. Trả về true nếu tất cả field hợp lệ.
        /// Dùng errorProvider1.SetError để hiển thị/ xóa lỗi cho từng control.
        /// </summary>
        private bool KiemTraHopLe()
        {
            bool hopLe = true;

            // 1. Họ tên: không rỗng, tối thiểu 3 ký tự
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || txtHoTen.Text.Trim().Length < 3)
            {
                errorProvider1.SetError(txtHoTen, "Họ tên không được để trống và phải có ít nhất 3 ký tự.");
                hopLe = false;
            }
            else
            {
                errorProvider1.SetError(txtHoTen, "");
            }

            // 2. SĐT: đúng 10 chữ số, bắt đầu bằng '0'
            string sdt = txtSDT.Text.Trim();
            bool sdtHopLe = sdt.Length == 10 && sdt[0] == '0' && IsAllDigits(sdt);
            if (!sdtHopLe)
            {
                errorProvider1.SetError(txtSDT, "Số điện thoại phải gồm đúng 10 chữ số và bắt đầu bằng số 0.");
                hopLe = false;
            }
            else
            {
                errorProvider1.SetError(txtSDT, "");
            }

            // 3. Email: chứa '@' và '.' phía sau '@'
            string email = txtEmail.Text.Trim();
            int viTriA = email.IndexOf('@');
            bool emailHopLe = viTriA > 0 && email.IndexOf('.', viTriA) > viTriA + 1;
            if (!emailHopLe)
            {
                errorProvider1.SetError(txtEmail, "Email không hợp lệ (phải có dạng ten@miendomain.gì).");
                hopLe = false;
            }
            else
            {
                errorProvider1.SetError(txtEmail, "");
            }

            // 4. Mật khẩu: tối thiểu 6 ký tự
            if (txtMatKhau.Text.Length < 6)
            {
                errorProvider1.SetError(txtMatKhau, "Mật khẩu phải có ít nhất 6 ký tự.");
                hopLe = false;
            }
            else
            {
                errorProvider1.SetError(txtMatKhau, "");
            }

            // 5. Xác nhận mật khẩu: khớp với mật khẩu
            if (txtXacNhanMK.Text != txtMatKhau.Text)
            {
                errorProvider1.SetError(txtXacNhanMK, "Xác nhận mật khẩu không khớp với mật khẩu.");
                hopLe = false;
            }
            else
            {
                errorProvider1.SetError(txtXacNhanMK, "");
            }

            return hopLe;
        }

        private static bool IsAllDigits(string s)
        {
            foreach (char c in s)
                if (!char.IsDigit(c)) return false;
            return true;
        }

        private void BtnDangKy_Click(object? sender, EventArgs e)
        {
            if (!KiemTraHopLe())
                return; // còn lỗi, dừng lại

            MessageBox.Show(
                "Đăng ký thành công! Chào mừng " + txtHoTen.Text,
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            btnHuy.CausesValidation = false;
            errorProvider1.Clear();
            Close();
        }
    }
}
