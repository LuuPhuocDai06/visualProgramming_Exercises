namespace GymRegisterDemo
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.ComboBox cboGoiTap;
        private System.Windows.Forms.NumericUpDown numSoBuoiTuan;
        private System.Windows.Forms.Button btnDangKy;

        private System.Windows.Forms.ToolTip toolTip1;

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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();

            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.cboGoiTap = new System.Windows.Forms.ComboBox();
            this.numSoBuoiTuan = new System.Windows.Forms.NumericUpDown();
            this.btnDangKy = new System.Windows.Forms.Button();

            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);

            ((System.ComponentModel.ISupportInitialize)(this.numSoBuoiTuan)).BeginInit();
            this.SuspendLayout();

            // label1 - Họ tên
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Text = "Họ tên:";

            // label2 - SĐT
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 70);
            this.label2.Text = "Số điện thoại:";

            // label3 - Email
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 110);
            this.label3.Text = "Email:";

            // label4 - Ngày sinh
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 150);
            this.label4.Text = "Ngày sinh:";

            // label5 - Gói tập
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 190);
            this.label5.Text = "Gói tập:";

            // label6 - Số buổi/tuần
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 230);
            this.label6.Text = "Số buổi/tuần:";

            // txtHoTen
            this.txtHoTen.Location = new System.Drawing.Point(150, 27);
            this.txtHoTen.Size = new System.Drawing.Size(220, 20);
            this.txtHoTen.Name = "txtHoTen";

            // txtSDT
            this.txtSDT.Location = new System.Drawing.Point(150, 67);
            this.txtSDT.Size = new System.Drawing.Size(220, 20);
            this.txtSDT.Name = "txtSDT";

            // txtEmail
            this.txtEmail.Location = new System.Drawing.Point(150, 107);
            this.txtEmail.Size = new System.Drawing.Size(220, 20);
            this.txtEmail.Name = "txtEmail";

            // dtpNgaySinh
            this.dtpNgaySinh.Location = new System.Drawing.Point(150, 147);
            this.dtpNgaySinh.Size = new System.Drawing.Size(220, 20);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // cboGoiTap
            this.cboGoiTap.Location = new System.Drawing.Point(150, 187);
            this.cboGoiTap.Size = new System.Drawing.Size(220, 21);
            this.cboGoiTap.Name = "cboGoiTap";
            this.cboGoiTap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGoiTap.Items.AddRange(new object[] { "Basic", "VIP", "Premium" });

            // numSoBuoiTuan
            this.numSoBuoiTuan.Location = new System.Drawing.Point(150, 227);
            this.numSoBuoiTuan.Size = new System.Drawing.Size(80, 20);
            this.numSoBuoiTuan.Name = "numSoBuoiTuan";
            this.numSoBuoiTuan.Minimum = 1;
            this.numSoBuoiTuan.Maximum = 7;
            this.numSoBuoiTuan.Value = 1;

            // btnDangKy
            this.btnDangKy.Location = new System.Drawing.Point(150, 270);
            this.btnDangKy.Size = new System.Drawing.Size(120, 35);
            this.btnDangKy.Name = "btnDangKy";
            this.btnDangKy.Text = "Đăng ký";
            this.btnDangKy.UseVisualStyleBackColor = true;
            this.btnDangKy.Click += new System.EventHandler(this.btnDangKy_Click);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(420, 340);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtHoTen);
            this.Controls.Add(this.txtSDT);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.dtpNgaySinh);
            this.Controls.Add(this.cboGoiTap);
            this.Controls.Add(this.numSoBuoiTuan);
            this.Controls.Add(this.btnDangKy);
            this.Name = "Form1";
            this.Text = "Đăng ký hội viên - Gym FitLife";

            ((System.ComponentModel.ISupportInitialize)(this.numSoBuoiTuan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}