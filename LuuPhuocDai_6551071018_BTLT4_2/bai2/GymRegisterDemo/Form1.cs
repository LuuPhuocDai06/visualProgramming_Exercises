using System;
using System.Windows.Forms;

namespace GymRegisterDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Thiết lập thuộc tính của ToolTip1 trong code (constructor)
            toolTip1.AutoPopDelay = 5000;   // Chú thích tự ẩn sau 5 giây
            toolTip1.InitialDelay = 500;    // Chờ 0.5s trước khi hiện
            toolTip1.ReshowDelay = 100;     // Độ trễ khi hiện lại chú thích cho control khác
            toolTip1.ShowAlways = true;     // Luôn hiện dù Form đang không active

            // Gán nội dung ToolTip cho từng control
            toolTip1.SetToolTip(txtHoTen, "Nhập họ và tên đầy đủ của hội viên");
            toolTip1.SetToolTip(txtSDT, "Nhập đúng 10 chữ số, không chứa khoảng trắng hay ký tự đặc biệt");
            toolTip1.SetToolTip(txtEmail, "Email dùng để nhận thông báo lịch tập và khuyến mãi");
            toolTip1.SetToolTip(dtpNgaySinh, "Chọn ngày sinh của hội viên");
            toolTip1.SetToolTip(cboGoiTap, "Gói VIP và Premium có kèm huấn luyện viên riêng");
            toolTip1.SetToolTip(numSoBuoiTuan, "Chọn số buổi tập trong tuần (từ 1 đến 7)");
        }

        // Xử lý Click cho btnDangKy
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên và Số điện thoại",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string goiTap = cboGoiTap.SelectedItem != null ? cboGoiTap.SelectedItem.ToString() : "(chưa chọn)";

            string thongTin =
                "Họ tên: " + txtHoTen.Text + Environment.NewLine +
                "SĐT: " + txtSDT.Text + Environment.NewLine +
                "Gói tập: " + goiTap + Environment.NewLine +
                "Số buổi/tuần: " + numSoBuoiTuan.Value;

            MessageBox.Show(thongTin, "Thông tin đăng ký hội viên",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}