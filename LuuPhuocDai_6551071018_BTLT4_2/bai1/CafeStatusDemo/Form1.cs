using System;
using System.Drawing;
using System.Windows.Forms;

namespace CafeStatusDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện Tick của Timer: cập nhật giờ và trạng thái mở/đóng cửa
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Cập nhật giờ hiện tại
            lblGioHienTai.Text = DateTime.Now.ToString("HH:mm:ss");

            // Kiểm tra khung giờ mở cửa (6h - 22h)
            int gio = DateTime.Now.Hour;
            if (gio >= 6 && gio < 22)
            {
                lblTrangThai.Text = "Đang mở cửa";
                lblTrangThai.ForeColor = Color.Green;
            }
            else
            {
                lblTrangThai.Text = "Đã đóng cửa";
                lblTrangThai.ForeColor = Color.Red;
            }
        }

        // Xử lý Click menu "Đổi màu nền"
        private void mnuDoiMauNen_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = colorDialog1.Color;
            }
        }

        // Xử lý Click menu "Thoát"
        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}