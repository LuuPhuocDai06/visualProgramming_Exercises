using System;
using System.Windows.Forms;

namespace FormBanHangDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Chỉ cho phép nhập chữ số (0-9) và phím Backspace trong txtSoLuong, txtDonGia
        private void SoOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép ký tự số (0-9) hoặc phím Backspace, chặn tất cả ký tự khác
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Hủy ký tự vừa gõ, không cho xuất hiện trong TextBox
            }
        }

        // Xử lý Click cho btnThem: thêm dòng kết quả vào ListBox
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text) ||
                string.IsNullOrWhiteSpace(txtSoLuong.Text) ||
                string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã SP, Số lượng và Đơn giá",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dong = txtMaSP.Text + " | " + txtSoLuong.Text + " | " + txtDonGia.Text;
            lstKetQua.Items.Add(dong);
        }

        // Xử lý Click cho btnXoaTrang: xóa trắng 3 TextBox và focus về txtMaSP
        private void btnXoaTrang_Click(object sender, EventArgs e)
        {
            txtMaSP.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtMaSP.Focus();
        }

        // Bắt phím tắt trên toàn Form (nhờ KeyPreview = true)
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    // Nhấn F2 từ bất kỳ đâu -> kích hoạt btnThem
                    btnThem_Click(sender, e);
                    e.Handled = true;
                    break;

                case Keys.F5:
                    // Nhấn F5 -> xóa trắng và focus về txtMaSP
                    btnXoaTrang_Click(sender, e);
                    e.Handled = true;
                    break;

                case Keys.Escape:
                    // Nhấn Escape -> hỏi xác nhận trước khi đóng form
                    DialogResult ketQua = MessageBox.Show("Bạn có muốn thoát?", "Xác nhận thoát",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ketQua == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    e.Handled = true;
                    break;
            }
        }
    }
}