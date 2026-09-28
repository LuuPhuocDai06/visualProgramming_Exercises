using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    /// <summary>
    /// CÂU 5: Form bán vé xem phim - mở Dialog riêng để chọn ghế.
    /// </summary>
    public class FormBanVe : Form
    {
        private readonly TextBox txtTenKhach = new();
        private readonly ComboBox cboPhim = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cboSuatChieu = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly TextBox txtGheDaChon = new() { ReadOnly = true };
        private readonly Button btnChonGhe = new() { Text = "Chọn ghế..." };
        private readonly Button btnDatVe = new() { Text = "Đặt vé" };
        private readonly Button btnHuy = new() { Text = "Hủy" };

        private const decimal GiaVe = 75000m;

        public FormBanVe()
        {
            Text = "FormBanVe - Bán vé xem phim";
            Width = 460;
            Height = 380;
            StartPosition = FormStartPosition.CenterScreen;

            BuildLayout();

            cboPhim.Items.AddRange(new object[] { "Avengers: Endgame", "Inside Out 3", "Dune: Part Two" });
            cboSuatChieu.Items.AddRange(new object[] { "09:00", "13:30", "18:00", "20:30" });

            btnChonGhe.Click += BtnChonGhe_Click;
            btnDatVe.Click += BtnDatVe_Click;
            btnHuy.Click += (s, e) => Close();
        }

        private void BuildLayout()
        {
            var table = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                Height = 220,
                Padding = new Padding(10)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            void AddRow(string label, Control c)
            {
                table.RowCount++;
                table.Controls.Add(new Label { Text = label, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, table.RowCount - 1);
                c.Dock = DockStyle.Fill;
                table.Controls.Add(c, 1, table.RowCount - 1);
            }

            AddRow("Tên khách:", txtTenKhach);
            AddRow("Phim:", cboPhim);
            AddRow("Suất chiếu:", cboSuatChieu);

            table.RowCount++;
            table.Controls.Add(new Label { Text = "Ghế đã chọn:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, table.RowCount - 1);
            var panelGhe = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill };
            panelGhe.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            panelGhe.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            txtGheDaChon.Dock = DockStyle.Fill;
            panelGhe.Controls.Add(txtGheDaChon, 0, 0);
            btnChonGhe.Dock = DockStyle.Fill;
            panelGhe.Controls.Add(btnChonGhe, 1, 0);
            table.Controls.Add(panelGhe, 1, table.RowCount - 1);

            Controls.Add(table);

            var panelBtn = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };
            panelBtn.Controls.Add(btnHuy);
            panelBtn.Controls.Add(btnDatVe);
            Controls.Add(panelBtn);
        }

        private void BtnChonGhe_Click(object? sender, EventArgs e)
        {
            using (var dlg = new FormChonGhe(txtGheDaChon.Text))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    txtGheDaChon.Text = dlg.GheChon;
                }
                // Cancel (Bỏ qua) -> không đổi txtGheDaChon
            }
        }

        private void BtnDatVe_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKhach.Text) ||
                cboPhim.SelectedItem == null ||
                cboSuatChieu.SelectedItem == null ||
                string.IsNullOrEmpty(txtGheDaChon.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và chọn ghế trước khi đặt vé.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                $"Đặt vé thành công!\nKhách: {txtTenKhach.Text}\nPhim: {cboPhim.SelectedItem}\n" +
                $"Suất chiếu: {cboSuatChieu.SelectedItem}\nGhế: {txtGheDaChon.Text}\nGiá vé: {GiaVe:N0}đ",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
