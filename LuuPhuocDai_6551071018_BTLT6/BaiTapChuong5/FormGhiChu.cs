using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    /// <summary>
    /// CÂU 6: Form con MDI - một ghi chú công việc.
    /// Gồm: Validating/Validated, phím tắt Ctrl+S / Escape, giới hạn ký tự,
    /// double-click tiêu đề để phóng to/thu nhỏ, hover đổi màu nút Lưu.
    /// </summary>
    public class FormGhiChu : Form
    {
        private readonly Label lblTieuDeForm = new() { Text = "GHI CHÚ MỚI", Dock = DockStyle.Top, Height = 30, BackColor = Color.SteelBlue, ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        private readonly TextBox txtTieuDe = new();
        private readonly TextBox txtNoiDung = new() { Multiline = true, ScrollBars = ScrollBars.Vertical, Height = 150 };
        private readonly ComboBox cboMucDoUuTien = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button btnLuuGhiChu = new() { Text = "Lưu ghi chú" };
        private readonly ErrorProvider errorProvider1 = new();

        private const int GioiHanKyTu = 500;
        private static readonly Color MauNutBinhThuong = SystemColors.Control;
        private static readonly Color MauNutHover = Color.LightSkyBlue;

        private bool _daThayDoi = false;

        public FormGhiChu()
        {
            Width = 380;
            Height = 400;
            Text = "Ghi chú mới";
            KeyPreview = true;

            BuildLayout();

            cboMucDoUuTien.Items.AddRange(new object[] { "Thấp", "Trung bình", "Cao" });
            cboMucDoUuTien.SelectedIndex = 0;

            txtTieuDe.Validating += TxtTieuDe_Validating;
            txtTieuDe.Validated += TxtTieuDe_Validated;

            txtNoiDung.KeyPress += TxtNoiDung_KeyPress;
            txtTieuDe.TextChanged += (s, e) => _daThayDoi = true;
            txtNoiDung.TextChanged += (s, e) => _daThayDoi = true;

            lblTieuDeForm.MouseDoubleClick += LblTieuDeForm_MouseDoubleClick;
            btnLuuGhiChu.MouseEnter += (s, e) => btnLuuGhiChu.BackColor = MauNutHover;
            btnLuuGhiChu.MouseLeave += (s, e) => btnLuuGhiChu.BackColor = MauNutBinhThuong;

            btnLuuGhiChu.Click += BtnLuuGhiChu_Click;
            this.KeyDown += FormGhiChu_KeyDown;
        }

        private void BuildLayout()
        {
            Controls.Add(lblTieuDeForm);

            var table = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                Height = 90,
                Padding = new Padding(8)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            table.Controls.Add(new Label { Text = "Tiêu đề:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            txtTieuDe.Dock = DockStyle.Fill;
            table.Controls.Add(txtTieuDe, 1, 0);
            table.Controls.Add(new Label { Text = "Mức độ ưu tiên:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            cboMucDoUuTien.Dock = DockStyle.Fill;
            table.Controls.Add(cboMucDoUuTien, 1, 1);
            Controls.Add(table);

            var lblNoiDung = new Label { Text = "Nội dung (tối đa 500 ký tự):", Dock = DockStyle.Top, Height = 22, Padding = new Padding(8, 4, 0, 0) };
            Controls.Add(lblNoiDung);

            txtNoiDung.Dock = DockStyle.Top;
            Controls.Add(txtNoiDung);

            btnLuuGhiChu.Dock = DockStyle.Bottom;
            btnLuuGhiChu.Height = 40;
            Controls.Add(btnLuuGhiChu);

            // Sắp xếp lại thứ tự Dock cho đúng: tiêu đề trên cùng, rồi table, rồi label nội dung, rồi textbox nội dung
            lblTieuDeForm.BringToFront();
        }

        private void TxtTieuDe_Validating(object? sender, CancelEventArgs e)
        {
            string s = txtTieuDe.Text.Trim();
            if (string.IsNullOrEmpty(s) || s.Length > 50)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTieuDe, "Tiêu đề không được để trống và tối đa 50 ký tự.");
                txtTieuDe.BackColor = Color.MistyRose;
                return;
            }
            errorProvider1.SetError(txtTieuDe, "");
        }

        private void TxtTieuDe_Validated(object? sender, EventArgs e)
        {
            txtTieuDe.BackColor = Color.White;
            errorProvider1.SetError(txtTieuDe, "");
        }

        private void TxtNoiDung_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Cho phép luôn các phím điều khiển (Backspace, ...) để không khóa việc xóa nội dung
            bool laPhimDieuKhien = char.IsControl(e.KeyChar);
            if (!laPhimDieuKhien && txtNoiDung.Text.Length >= GioiHanKyTu)
            {
                e.Handled = true; // chặn ký tự thêm khi đã đủ 500 ký tự
            }
        }

        private void LblTieuDeForm_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            WindowState = WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }

        private void FormGhiChu_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
                btnLuuGhiChu.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (_daThayDoi)
                {
                    var ketQua = MessageBox.Show(
                        "Nội dung đã thay đổi. Bạn có muốn đóng ghi chú này không?",
                        "Xác nhận đóng",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (ketQua == DialogResult.Yes)
                        Close();
                }
                else
                {
                    Close();
                }
            }
        }

        private void BtnLuuGhiChu_Click(object? sender, EventArgs e)
        {
            if (!ValidateChildren())
                return; // còn lỗi Validating (vd: tiêu đề trống) -> dừng lại

            Text = txtTieuDe.Text;
            _daThayDoi = false;
            MessageBox.Show("Đã lưu ghi chú", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
