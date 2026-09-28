using System;
using System.Windows.Forms;

namespace BaiTapChuong5
{
    /// <summary>
    /// CÂU 6: Form chính MDI Container quản lý nhiều FormGhiChu (MDI Child).
    /// </summary>
    public class FormChinh : Form
    {
        private readonly MenuStrip menuStrip1 = new();
        private readonly StatusStrip statusStrip1 = new();
        private readonly ToolStripStatusLabel lblSoGhiChu = new() { Text = "Số ghi chú đang mở: 0" };

        public FormChinh()
        {
            Text = "FormChinh - Quản lý ghi chú công việc (MDI)";
            Width = 900;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            IsMdiContainer = true;

            BuildMenu();
            BuildStatusBar();
        }

        private void BuildMenu()
        {
            var mnuTep = new ToolStripMenuItem("Tệp");
            var mnuMoGhiChuMoi = new ToolStripMenuItem("Mở ghi chú mới");
            var mnuSapXepCuaSo = new ToolStripMenuItem("Sắp xếp cửa sổ");
            var mnuThoat = new ToolStripMenuItem("Thoát");

            mnuMoGhiChuMoi.Click += MnuMoGhiChuMoi_Click;
            mnuSapXepCuaSo.Click += (s, e) => LayoutMdi(MdiLayout.Cascade);
            mnuThoat.Click += (s, e) => Close();

            mnuTep.DropDownItems.Add(mnuMoGhiChuMoi);
            mnuTep.DropDownItems.Add(mnuSapXepCuaSo);
            mnuTep.DropDownItems.Add(new ToolStripSeparator());
            mnuTep.DropDownItems.Add(mnuThoat);

            var mnuCuaSo = new ToolStripMenuItem("Cửa sổ");
            var mnuXepTang = new ToolStripMenuItem("Xếp tầng");
            var mnuXepNgang = new ToolStripMenuItem("Xếp ngang");
            var mnuXepDoc = new ToolStripMenuItem("Xếp dọc");

            mnuXepTang.Click += (s, e) => LayoutMdi(MdiLayout.Cascade);
            mnuXepNgang.Click += (s, e) => LayoutMdi(MdiLayout.TileHorizontal);
            mnuXepDoc.Click += (s, e) => LayoutMdi(MdiLayout.TileVertical);

            mnuCuaSo.DropDownItems.Add(mnuXepTang);
            mnuCuaSo.DropDownItems.Add(mnuXepNgang);
            mnuCuaSo.DropDownItems.Add(mnuXepDoc);

            menuStrip1.Items.Add(mnuTep);
            menuStrip1.Items.Add(mnuCuaSo);

            MainMenuStrip = menuStrip1;
            Controls.Add(menuStrip1);
        }

        private void BuildStatusBar()
        {
            statusStrip1.Items.Add(lblSoGhiChu);
            Controls.Add(statusStrip1);
        }

        private void MnuMoGhiChuMoi_Click(object? sender, EventArgs e)
        {
            var f = new FormGhiChu
            {
                MdiParent = this
            };
            f.FormClosed += (s, e2) => CapNhatSoGhiChu();
            f.Show();
            CapNhatSoGhiChu();
        }

        private void CapNhatSoGhiChu()
        {
            lblSoGhiChu.Text = $"Số ghi chú đang mở: {MdiChildren.Length}";
        }
    }
}
