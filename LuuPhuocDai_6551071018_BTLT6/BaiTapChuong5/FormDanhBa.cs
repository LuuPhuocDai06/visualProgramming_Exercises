using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    /// <summary>
    /// CÂU 4: Quản lý danh bạ điện thoại - xác nhận các thao tác nguy hiểm (xóa, thoát chưa lưu).
    /// </summary>
    public class FormDanhBa : Form
    {
        private readonly ListBox lstLienHe = new();
        private readonly TextBox txtTen = new();
        private readonly TextBox txtSDT = new();
        private readonly Button btnThem = new() { Text = "Thêm" };
        private readonly Button btnSua = new() { Text = "Sửa" };
        private readonly Button btnXoa = new() { Text = "Xóa" };
        private readonly Button btnThoat = new() { Text = "Thoát" };

        private int _indexDangSua = -1; // -1 nghĩa là đang ở chế độ Thêm mới

        public FormDanhBa()
        {
            Text = "FormDanhBa - Danh bạ điện thoại";
            Width = 480;
            Height = 480;
            StartPosition = FormStartPosition.CenterScreen;

            BuildLayout();

            btnThem.Click += BtnThem_Click;
            btnXoa.Click += BtnXoa_Click;
            btnSua.Click += BtnSua_Click;
            btnThoat.Click += (s, e) => Close();
            FormClosing += FormDanhBa_FormClosing;
        }

        private void BuildLayout()
        {
            var top = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                Height = 90,
                Padding = new Padding(10)
            };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            top.Controls.Add(new Label { Text = "Họ tên:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            txtTen.Dock = DockStyle.Fill;
            top.Controls.Add(txtTen, 1, 0);
            top.Controls.Add(new Label { Text = "Số điện thoại:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            txtSDT.Dock = DockStyle.Fill;
            top.Controls.Add(txtSDT, 1, 1);
            Controls.Add(top);

            var panelBtn = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 45,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10, 0, 10, 0)
            };
            panelBtn.Controls.Add(btnThem);
            panelBtn.Controls.Add(btnSua);
            panelBtn.Controls.Add(btnXoa);
            panelBtn.Controls.Add(btnThoat);
            Controls.Add(panelBtn);

            lstLienHe.Dock = DockStyle.Fill;
            Controls.Add(lstLienHe);

            top.BringToFront();
            panelBtn.BringToFront();
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ họ tên và số điện thoại.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dong = $"{txtTen.Text} - {txtSDT.Text}";

            if (_indexDangSua >= 0)
            {
                // Đang ở chế độ sửa: cập nhật lại dòng đang chọn
                lstLienHe.Items[_indexDangSua] = dong;
                _indexDangSua = -1;
                btnThem.Text = "Thêm";
                MessageBox.Show("Cập nhật thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lstLienHe.Items.Add(dong);
                MessageBox.Show("Thêm thành công", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtTen.Clear();
            txtSDT.Clear();
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (lstLienHe.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn một liên hệ để xóa", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ten = lstLienHe.SelectedItem!.ToString()!;
            var ketQua = MessageBox.Show(
                $"Bạn có chắc muốn xóa liên hệ {ten}? Thao tác này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                lstLienHe.Items.RemoveAt(lstLienHe.SelectedIndex);
                MessageBox.Show("Xóa thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // No -> không làm gì, item vẫn còn nguyên
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (lstLienHe.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn một liên hệ để sửa", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dong = lstLienHe.SelectedItem!.ToString()!;
            int viTri = dong.IndexOf(" - ");
            if (viTri > 0)
            {
                txtTen.Text = dong[..viTri];
                txtSDT.Text = dong[(viTri + 3)..];
            }
            _indexDangSua = lstLienHe.SelectedIndex;
            btnThem.Text = "Cập nhật (bấm Thêm)";
        }

        private void FormDanhBa_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTen.Text) || !string.IsNullOrEmpty(txtSDT.Text))
            {
                var ketQua = MessageBox.Show(
                    "Bạn có dữ liệu chưa được lưu. Bạn muốn thoát không?",
                    "Cảnh báo",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (ketQua == DialogResult.Yes)
                {
                    // thoát bình thường
                }
                else if (ketQua == DialogResult.No)
                {
                    txtTen.Clear();
                    txtSDT.Clear();
                    // vẫn thoát sau khi xóa
                }
                else // Cancel
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
