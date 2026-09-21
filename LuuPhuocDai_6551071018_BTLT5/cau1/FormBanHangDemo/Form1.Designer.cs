namespace FormBanHangDemo
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.TextBox txtMaSP;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.TextBox txtDonGia;

        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnXoaTrang;

        private System.Windows.Forms.ListBox lstKetQua;

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

            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();

            this.txtMaSP = new System.Windows.Forms.TextBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.txtDonGia = new System.Windows.Forms.TextBox();

            this.btnThem = new System.Windows.Forms.Button();
            this.btnXoaTrang = new System.Windows.Forms.Button();

            this.lstKetQua = new System.Windows.Forms.ListBox();

            this.SuspendLayout();

            // label1 - Mã sản phẩm
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Text = "Mã sản phẩm:";

            // label2 - Số lượng
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 70);
            this.label2.Text = "Số lượng:";

            // label3 - Đơn giá
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 110);
            this.label3.Text = "Đơn giá:";

            // txtMaSP
            this.txtMaSP.Location = new System.Drawing.Point(150, 27);
            this.txtMaSP.Size = new System.Drawing.Size(200, 20);
            this.txtMaSP.Name = "txtMaSP";

            // txtSoLuong
            this.txtSoLuong.Location = new System.Drawing.Point(150, 67);
            this.txtSoLuong.Size = new System.Drawing.Size(200, 20);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoOnly_KeyPress);

            // txtDonGia
            this.txtDonGia.Location = new System.Drawing.Point(150, 107);
            this.txtDonGia.Size = new System.Drawing.Size(200, 20);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoOnly_KeyPress);

            // btnThem
            this.btnThem.Location = new System.Drawing.Point(150, 150);
            this.btnThem.Size = new System.Drawing.Size(95, 30);
            this.btnThem.Name = "btnThem";
            this.btnThem.Text = "Thêm (F2)";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            // btnXoaTrang
            this.btnXoaTrang.Location = new System.Drawing.Point(255, 150);
            this.btnXoaTrang.Size = new System.Drawing.Size(95, 30);
            this.btnXoaTrang.Name = "btnXoaTrang";
            this.btnXoaTrang.Text = "Xóa trắng (F5)";
            this.btnXoaTrang.UseVisualStyleBackColor = true;
            this.btnXoaTrang.Click += new System.EventHandler(this.btnXoaTrang_Click);

            // lstKetQua
            this.lstKetQua.Location = new System.Drawing.Point(30, 200);
            this.lstKetQua.Size = new System.Drawing.Size(400, 160);
            this.lstKetQua.Name = "lstKetQua";

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(460, 390);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMaSP);
            this.Controls.Add(this.txtSoLuong);
            this.Controls.Add(this.txtDonGia);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.btnXoaTrang);
            this.Controls.Add(this.lstKetQua);
            this.Name = "Form1";
            this.Text = "Form nhập liệu siêu thị - FormBanHang";
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}