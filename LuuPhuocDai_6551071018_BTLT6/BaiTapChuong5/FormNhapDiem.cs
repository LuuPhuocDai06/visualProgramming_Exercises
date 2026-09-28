using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    /// <summary>
    /// CÂU 3: Form nhập điểm học sinh - tối ưu luồng nhập liệu bằng bàn phím (Enter chuyển field).
    /// </summary>
    public class FormNhapDiem : Form
    {
        private readonly TextBox txtMaHS = new() { TabIndex = 0 };
        private readonly TextBox txtHoTen = new() { TabIndex = 1 };
        private readonly TextBox txtToan = new() { TabIndex = 2 };
        private readonly TextBox txtVan = new() { TabIndex = 3 };
        private readonly TextBox txtAnh = new() { TabIndex = 4 };
        private readonly Button btnLuu = new() { Text = "Lưu", TabIndex = 5 };
        private readonly Button btnXoaTrang = new() { Text = "Xóa trắng", TabIndex = 6 };
        private readonly ListBox lstKetQua = new();
        private readonly ErrorProvider errorProvider1 = new();

        public FormNhapDiem()
        {
            Text = "FormNhapDiem - Nhập điểm học sinh";
            Width = 500;
            Height = 500;
            StartPosition = FormStartPosition.CenterScreen;

            BuildLayout();
            DangKyEnterChuyenField();

            txtToan.Enter += TxtDiem_Enter;
            txtVan.Enter += TxtDiem_Enter;
            txtAnh.Enter += TxtDiem_Enter;

            btnLuu.Click += BtnLuu_Click;
            btnXoaTrang.Click += (s, e) => XoaTrangForm();
        }

        private void BuildLayout()
        {
            var table = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                Height = 190,
                Padding = new Padding(10)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            void AddRow(string label, TextBox tb)
            {
                table.RowCount++;
                var lbl = new Label { Text = label, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, TabStop = false };
                table.Controls.Add(lbl, 0, table.RowCount - 1);
                tb.Dock = DockStyle.Fill;
                table.Controls.Add(tb, 1, table.RowCount - 1);
            }

            AddRow("Mã học sinh:", txtMaHS);
            AddRow("Họ tên:", txtHoTen);
            AddRow("Điểm Toán:", txtToan);
            AddRow("Điểm Văn:", txtVan);
            AddRow("Điểm Anh:", txtAnh);

            Controls.Add(table);

            var panelBtn = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 45,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10, 0, 10, 0)
            };
            panelBtn.Controls.Add(btnXoaTrang);
            panelBtn.Controls.Add(btnLuu);
            Controls.Add(panelBtn);

            var lbl2 = new Label { Text = "Danh sách đã lưu:", Dock = DockStyle.Top, Height = 24, Padding = new Padding(10, 4, 0, 0) };
            Controls.Add(lbl2);

            lstKetQua.Dock = DockStyle.Fill;
            Controls.Add(lstKetQua);

            // Đưa table lên trên cùng do thứ tự add ảnh hưởng Dock
            table.BringToFront();
            panelBtn.BringToFront();
            lbl2.BringToFront();
        }

        /// <summary>
        /// Duyệt tất cả TextBox trên form, đăng ký KeyPress: Enter -> chuyển sang control kế tiếp theo TabIndex.
        /// Riêng txtAnh khi Enter -> thực hiện Lưu.
        /// </summary>
        private void DangKyEnterChuyenField()
        {
            foreach (Control c in Controls)
                DangKyDeQuy(c);
        }

        private void DangKyDeQuy(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox tb)
                {
                    tb.KeyPress += TextBox_KeyPress;
                }
                if (c.HasChildren)
                    DangKyDeQuy(c);
            }
        }

        private void TextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                var tb = (TextBox)sender!;
                if (tb == txtAnh)
                {
                    btnLuu.PerformClick();
                }
                else
                {
                    SelectNextControl((Control)sender!, true, true, true, true);
                }
            }
        }

        private void TxtDiem_Enter(object? sender, EventArgs e)
        {
            // Khi field điểm nhận focus, bôi xanh toàn bộ nội dung cũ để gõ đè
            ((TextBox)sender!).SelectAll();
        }

        private void BtnLuu_Click(object? sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool hopLe = true;

            if (string.IsNullOrWhiteSpace(txtMaHS.Text))
            {
                errorProvider1.SetError(txtMaHS, "Mã học sinh không được để trống.");
                hopLe = false;
            }
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errorProvider1.SetError(txtHoTen, "Họ tên không được để trống.");
                hopLe = false;
            }

            hopLe &= KiemTraDiem(txtToan, "Toán");
            hopLe &= KiemTraDiem(txtVan, "Văn");
            hopLe &= KiemTraDiem(txtAnh, "Anh");

            if (!hopLe) return;

            lstKetQua.Items.Add(
                $"[{txtMaHS.Text}] | {txtHoTen.Text} | T:{txtToan.Text} V:{txtVan.Text} A:{txtAnh.Text}");

            XoaTrangForm();
            txtMaHS.Focus();
        }

        private bool KiemTraDiem(TextBox tb, string tenMon)
        {
            if (!decimal.TryParse(tb.Text.Trim(), out decimal diem) || diem < 0.0m || diem > 10.0m)
            {
                errorProvider1.SetError(tb, $"Điểm {tenMon} phải là số từ 0.0 đến 10.0.");
                return false;
            }
            errorProvider1.SetError(tb, "");
            return true;
        }

        private void XoaTrangForm()
        {
            txtMaHS.Clear();
            txtHoTen.Clear();
            txtToan.Clear();
            txtVan.Clear();
            txtAnh.Clear();
            errorProvider1.Clear();
        }
    }
}
