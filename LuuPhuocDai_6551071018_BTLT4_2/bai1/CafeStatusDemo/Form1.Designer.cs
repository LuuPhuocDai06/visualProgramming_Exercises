namespace CafeStatusDemo
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // MenuStrip
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuHeThong;
        private System.Windows.Forms.ToolStripMenuItem mnuDoiMauNen;
        private System.Windows.Forms.ToolStripMenuItem mnuThoat;

        // StatusStrip
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblGioHienTai;
        private System.Windows.Forms.ToolStripStatusLabel lblTenQuan;
        private System.Windows.Forms.ToolStripStatusLabel lblTrangThai;

        // Timer + ColorDialog
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColorDialog colorDialog1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDoiMauNen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuThoat = new System.Windows.Forms.ToolStripMenuItem();

            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblGioHienTai = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTenQuan = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTrangThai = new System.Windows.Forms.ToolStripStatusLabel();

            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();

            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();

            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHeThong});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(500, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";

            // 
            // mnuHeThong
            // 
            this.mnuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDoiMauNen,
            this.mnuThoat});
            this.mnuHeThong.Name = "mnuHeThong";
            this.mnuHeThong.Size = new System.Drawing.Size(70, 20);
            this.mnuHeThong.Text = "Hệ thống";

            // 
            // mnuDoiMauNen
            // 
            this.mnuDoiMauNen.Name = "mnuDoiMauNen";
            this.mnuDoiMauNen.Size = new System.Drawing.Size(160, 22);
            this.mnuDoiMauNen.Text = "Đổi màu nền";
            this.mnuDoiMauNen.Click += new System.EventHandler(this.mnuDoiMauNen_Click);

            // 
            // mnuThoat
            // 
            this.mnuThoat.Name = "mnuThoat";
            this.mnuThoat.Size = new System.Drawing.Size(160, 22);
            this.mnuThoat.Text = "Thoát";
            this.mnuThoat.Click += new System.EventHandler(this.mnuThoat_Click);

            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblGioHienTai,
            this.lblTenQuan,
            this.lblTrangThai});
            this.statusStrip1.Location = new System.Drawing.Point(0, 328);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(500, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";

            // 
            // lblGioHienTai
            // 
            this.lblGioHienTai.Name = "lblGioHienTai";
            this.lblGioHienTai.Size = new System.Drawing.Size(50, 17);
            this.lblGioHienTai.Text = "00:00:00";
            this.lblGioHienTai.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // lblTenQuan
            // 
            this.lblTenQuan.Name = "lblTenQuan";
            this.lblTenQuan.Size = new System.Drawing.Size(300, 17);
            this.lblTenQuan.Spring = true;
            this.lblTenQuan.Text = "CAFE ÁNH DƯƠNG";
            this.lblTenQuan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(80, 17);
            this.lblTrangThai.Text = "Đang mở cửa";
            this.lblTrangThai.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Bảng trạng thái quán cà phê - Ánh Dương";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}