using System;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    /// <summary>
    /// CÂU 5: Dialog chọn ghế riêng, trả kết quả qua thuộc tính GheChon + DialogResult.
    /// </summary>
    public class FormChonGhe : Form
    {
        private readonly ListBox lstGhe = new();
        private readonly Label lblGheDaChon = new() { Text = "Đang chọn: (chưa chọn)" };
        private readonly Button btnXacNhan = new() { Text = "Xác nhận" };
        private readonly Button btnBoQua = new() { Text = "Bỏ qua" };

        public string GheChon { get; private set; } = "";

        public FormChonGhe(string gheHienTai)
        {
            Text = "FormChonGhe - Chọn ghế";
            Width = 320;
            Height = 420;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            BuildLayout();

            string[] hang = { "A", "B", "C" };
            foreach (string h in hang)
                for (int i = 1; i <= 5; i++)
                    lstGhe.Items.Add(h + i);

            // Chọn sẵn ghế cũ nếu có
            if (!string.IsNullOrEmpty(gheHienTai))
            {
                int idx = lstGhe.Items.IndexOf(gheHienTai);
                if (idx >= 0)
                {
                    lstGhe.SelectedIndex = idx;
                    GheChon = gheHienTai;
                    lblGheDaChon.Text = "Đang chọn: " + gheHienTai;
                }
            }

            lstGhe.SelectedIndexChanged += LstGhe_SelectedIndexChanged;
            btnXacNhan.Click += BtnXacNhan_Click;
            btnBoQua.Click += (s, e) => { DialogResult = DialogResult.Cancel; };
        }

        private void BuildLayout()
        {
            lstGhe.Dock = DockStyle.Top;
            lstGhe.Height = 280;
            Controls.Add(lstGhe);

            lblGheDaChon.Dock = DockStyle.Top;
            lblGheDaChon.Height = 30;
            Controls.Add(lblGheDaChon);

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new System.Windows.Forms.Padding(10)
            };
            panel.Controls.Add(btnBoQua);
            panel.Controls.Add(btnXacNhan);
            Controls.Add(panel);

            lblGheDaChon.BringToFront();
            lstGhe.BringToFront();
        }

        private void LstGhe_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstGhe.SelectedItem != null)
                lblGheDaChon.Text = "Đang chọn: " + lstGhe.SelectedItem;
        }

        private void BtnXacNhan_Click(object? sender, EventArgs e)
        {
            if (lstGhe.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một ghế trước khi xác nhận.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            GheChon = lstGhe.SelectedItem.ToString()!;
            DialogResult = DialogResult.OK;
        }
    }
}
