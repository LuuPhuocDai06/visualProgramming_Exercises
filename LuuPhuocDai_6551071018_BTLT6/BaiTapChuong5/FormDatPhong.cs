using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    /// <summary>
    /// CÂU 2: Form đặt phòng khách sạn - phản hồi tức thì khi rời field (Validating/Validated).
    /// </summary>
    public class FormDatPhong : Form
    {
        private readonly TextBox txtHoTen = new();
        private readonly TextBox txtCCCD = new();
        private readonly TextBox txtNgayNhan = new();
        private readonly TextBox txtNgayTra = new();
        private readonly TextBox txtSoNguoiLon = new();
        private readonly TextBox txtSoTreEm = new();
        private readonly Button btnDatPhong = new() { Text = "Đặt phòng" };
        private readonly ErrorProvider errorProvider1 = new();

        public FormDatPhong()
        {
            Text = "FormDatPhong - Đặt phòng khách sạn";
            Width = 460;
            Height = 380;
            StartPosition = FormStartPosition.CenterScreen;

            BuildLayout();
            HookValidation();

            btnDatPhong.Click += BtnDatPhong_Click;
        }

        private void BuildLayout()
        {
            var table = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                Height = 240,
                Padding = new Padding(10)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            void AddRow(string label, TextBox tb, string? placeholder = null)
            {
                table.RowCount++;
                table.Controls.Add(new Label { Text = label, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, table.RowCount - 1);
                tb.Dock = DockStyle.Fill;
                if (placeholder != null) tb.PlaceholderText = placeholder;
                table.Controls.Add(tb, 1, table.RowCount - 1);
            }

            AddRow("Họ tên khách:", txtHoTen);
            AddRow("Số CCCD:", txtCCCD);
            AddRow("Ngày nhận phòng:", txtNgayNhan, "dd/MM/yyyy");
            AddRow("Ngày trả phòng:", txtNgayTra, "dd/MM/yyyy");
            AddRow("Số người lớn (1-4):", txtSoNguoiLon);
            AddRow("Số trẻ em (0-3):", txtSoTreEm);

            Controls.Add(table);

            var panelBtn = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };
            panelBtn.Controls.Add(btnDatPhong);
            Controls.Add(panelBtn);
        }

        private void HookValidation()
        {
            txtHoTen.Validating += TxtHoTen_Validating;
            txtCCCD.Validating += TxtCCCD_Validating;
            txtNgayNhan.Validating += TxtNgayNhan_Validating;
            txtNgayTra.Validating += TxtNgayTra_Validating;
            txtSoNguoiLon.Validating += TxtSoNguoiLon_Validating;
            txtSoTreEm.Validating += TxtSoTreEm_Validating;

            foreach (var tb in new[] { txtHoTen, txtCCCD, txtNgayNhan, txtNgayTra, txtSoNguoiLon, txtSoTreEm })
                tb.Validated += Tb_Validated;
        }

        private void Tb_Validated(object? sender, EventArgs e)
        {
            // Khi field đã hợp lệ, tô nền xanh lá nhạt để xác nhận
            ((Control)sender!).BackColor = Color.Honeydew;
        }

        private void BaoLoi(TextBox tb, CancelEventArgs e, string thongBao)
        {
            e.Cancel = true;
            errorProvider1.SetError(tb, thongBao);
            tb.BackColor = Color.MistyRose;
        }

        private void HetLoi(TextBox tb)
        {
            errorProvider1.SetError(tb, "");
        }

        private void TxtHoTen_Validating(object? sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                BaoLoi(txtHoTen, e, "Họ tên không được để trống.");
                return;
            }
            HetLoi(txtHoTen);
        }

        private void TxtCCCD_Validating(object? sender, CancelEventArgs e)
        {
            string s = txtCCCD.Text.Trim();
            bool ok = s.Length == 12;
            foreach (char c in s) ok &= char.IsDigit(c);
            if (!ok)
            {
                BaoLoi(txtCCCD, e, "Số CCCD phải gồm đúng 12 chữ số.");
                return;
            }
            HetLoi(txtCCCD);
        }

        private void TxtNgayNhan_Validating(object? sender, CancelEventArgs e)
        {
            if (!DateTime.TryParseExact(txtNgayNhan.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime ngayNhan) || ngayNhan.Date < DateTime.Today)
            {
                BaoLoi(txtNgayNhan, e, "Ngày nhận phòng không hợp lệ (dạng dd/MM/yyyy) và phải từ hôm nay trở đi.");
                return;
            }
            HetLoi(txtNgayNhan);
        }

        private void TxtNgayTra_Validating(object? sender, CancelEventArgs e)
        {
            bool nhanOk = DateTime.TryParseExact(txtNgayNhan.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime ngayNhan);
            if (!DateTime.TryParseExact(txtNgayTra.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime ngayTra) || !nhanOk || ngayTra.Date <= ngayNhan.Date)
            {
                BaoLoi(txtNgayTra, e, "Ngày trả phòng phải hợp lệ và lớn hơn ngày nhận phòng.");
                return;
            }
            HetLoi(txtNgayTra);
        }

        private void TxtSoNguoiLon_Validating(object? sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtSoNguoiLon.Text.Trim(), out int n) || n < 1 || n > 4)
            {
                BaoLoi(txtSoNguoiLon, e, "Số người lớn phải là số nguyên từ 1 đến 4.");
                return;
            }
            HetLoi(txtSoNguoiLon);
        }

        private void TxtSoTreEm_Validating(object? sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtSoTreEm.Text.Trim(), out int n) || n < 0 || n > 3)
            {
                BaoLoi(txtSoTreEm, e, "Số trẻ em phải là số nguyên từ 0 đến 3.");
                return;
            }
            HetLoi(txtSoTreEm);
        }

        private void BtnDatPhong_Click(object? sender, EventArgs e)
        {
            // Bấm nút sẽ tự kích hoạt Validating cho control đang có focus (nếu CausesValidation = true, mặc định là true)
            if (!ValidateChildren())
                return;

            DateTime ngayNhan = DateTime.ParseExact(txtNgayNhan.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime ngayTra = DateTime.ParseExact(txtNgayTra.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int soDem = (ngayTra - ngayNhan).Days;

            MessageBox.Show(
                $"Đặt phòng thành công!\nKhách: {txtHoTen.Text}\nSố đêm: {soDem}\n" +
                $"Người lớn: {txtSoNguoiLon.Text} - Trẻ em: {txtSoTreEm.Text}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
