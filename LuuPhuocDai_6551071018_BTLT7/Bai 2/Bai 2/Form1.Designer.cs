namespace Bai_2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtSDT = new TextBox();
            txtHoTen = new TextBox();
            txtEmail = new TextBox();
            dtpNgaySinh = new DateTimePicker();
            dgvHoiVien = new DataGridView();
            grbGioiTinh = new GroupBox();
            rbNu = new RadioButton();
            rbNam = new RadioButton();
            cboHangThanhVien = new ComboBox();
            chkTrangThai = new CheckBox();
            cboTimKiemHang = new ComboBox();
            label6 = new Label();
            txtTimKiemTen = new TextBox();
            btnTimKiem = new Button();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvHoiVien).BeginInit();
            grbGioiTinh.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 67);
            label1.Name = "label1";
            label1.Size = new Size(97, 20);
            label1.TabIndex = 0;
            label1.Text = "Số điện thoại";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 24);
            label2.Name = "label2";
            label2.Size = new Size(54, 20);
            label2.TabIndex = 1;
            label2.Text = "Họ tên";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 115);
            label3.Name = "label3";
            label3.Size = new Size(46, 20);
            label3.TabIndex = 2;
            label3.Text = "Email";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 161);
            label4.Name = "label4";
            label4.Size = new Size(74, 20);
            label4.TabIndex = 3;
            label4.Text = "Ngày sinh";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(21, 201);
            label5.Name = "label5";
            label5.Size = new Size(117, 20);
            label5.TabIndex = 4;
            label5.Text = "Hạng thành viên";
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(143, 63);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(243, 27);
            txtSDT.TabIndex = 5;
            // 
            // txtHoTen
            // 
            txtHoTen.Location = new Point(143, 17);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(243, 27);
            txtHoTen.TabIndex = 6;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(143, 108);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(243, 27);
            txtEmail.TabIndex = 7;
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(143, 156);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(243, 27);
            dtpNgaySinh.TabIndex = 8;
            // 
            // dgvHoiVien
            // 
            dgvHoiVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoiVien.Location = new Point(2, 312);
            dgvHoiVien.Name = "dgvHoiVien";
            dgvHoiVien.RowHeadersWidth = 51;
            dgvHoiVien.Size = new Size(1056, 183);
            dgvHoiVien.TabIndex = 9;
            dgvHoiVien.DataError += dgvHoiVien_DataError;
            dgvHoiVien.SelectionChanged += dgvHoiVien_SelectionChanged;
            // 
            // grbGioiTinh
            // 
            grbGioiTinh.Controls.Add(rbNu);
            grbGioiTinh.Controls.Add(rbNam);
            grbGioiTinh.Location = new Point(412, 15);
            grbGioiTinh.Name = "grbGioiTinh";
            grbGioiTinh.Size = new Size(253, 72);
            grbGioiTinh.TabIndex = 10;
            grbGioiTinh.TabStop = false;
            grbGioiTinh.Text = "Giới tính";
            // 
            // rbNu
            // 
            rbNu.AutoSize = true;
            rbNu.Location = new Point(133, 26);
            rbNu.Name = "rbNu";
            rbNu.Size = new Size(50, 24);
            rbNu.TabIndex = 1;
            rbNu.TabStop = true;
            rbNu.Text = "Nữ";
            rbNu.UseVisualStyleBackColor = true;
            // 
            // rbNam
            // 
            rbNam.AutoSize = true;
            rbNam.Location = new Point(0, 26);
            rbNam.Name = "rbNam";
            rbNam.Size = new Size(62, 24);
            rbNam.TabIndex = 0;
            rbNam.TabStop = true;
            rbNam.Text = "Nam";
            rbNam.UseVisualStyleBackColor = true;
            // 
            // cboHangThanhVien
            // 
            cboHangThanhVien.FormattingEnabled = true;
            cboHangThanhVien.Items.AddRange(new object[] { "Basic", "VIP", "Premium" });
            cboHangThanhVien.Location = new Point(143, 193);
            cboHangThanhVien.Name = "cboHangThanhVien";
            cboHangThanhVien.Size = new Size(242, 28);
            cboHangThanhVien.TabIndex = 11;
            // 
            // chkTrangThai
            // 
            chkTrangThai.AutoSize = true;
            chkTrangThai.Location = new Point(21, 231);
            chkTrangThai.Name = "chkTrangThai";
            chkTrangThai.Size = new Size(140, 24);
            chkTrangThai.TabIndex = 12;
            chkTrangThai.Text = "Đang hoạt đông";
            chkTrangThai.UseVisualStyleBackColor = true;
            // 
            // cboTimKiemHang
            // 
            cboTimKiemHang.FormattingEnabled = true;
            cboTimKiemHang.Items.AddRange(new object[] { "Basic", "VIP", "Premium", "Tất cả" });
            cboTimKiemHang.Location = new Point(535, 278);
            cboTimKiemHang.Name = "cboTimKiemHang";
            cboTimKiemHang.Size = new Size(242, 28);
            cboTimKiemHang.TabIndex = 14;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(412, 286);
            label6.Name = "label6";
            label6.Size = new Size(117, 20);
            label6.TabIndex = 13;
            label6.Text = "Hạng thành viên";
            // 
            // txtTimKiemTen
            // 
            txtTimKiemTen.Location = new Point(21, 279);
            txtTimKiemTen.Name = "txtTimKiemTen";
            txtTimKiemTen.Size = new Size(364, 27);
            txtTimKiemTen.TabIndex = 15;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(795, 279);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(94, 29);
            btnTimKiem.TabIndex = 16;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(795, 24);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 17;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(795, 82);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 18;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(795, 137);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 19;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(795, 197);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 20;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1055, 491);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(btnTimKiem);
            Controls.Add(txtTimKiemTen);
            Controls.Add(cboTimKiemHang);
            Controls.Add(label6);
            Controls.Add(chkTrangThai);
            Controls.Add(cboHangThanhVien);
            Controls.Add(grbGioiTinh);
            Controls.Add(dgvHoiVien);
            Controls.Add(dtpNgaySinh);
            Controls.Add(txtEmail);
            Controls.Add(txtHoTen);
            Controls.Add(txtSDT);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvHoiVien).EndInit();
            grbGioiTinh.ResumeLayout(false);
            grbGioiTinh.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtSDT;
        private TextBox txtHoTen;
        private TextBox txtEmail;
        private DateTimePicker dtpNgaySinh;
        private DataGridView dgvHoiVien;
        private GroupBox grbGioiTinh;
        private ComboBox cboHangThanhVien;
        private RadioButton rbNu;
        private RadioButton rbNam;
        private CheckBox chkTrangThai;
        private ComboBox cboTimKiemHang;
        private Label label6;
        private TextBox txtTimKiemTen;
        private Button btnTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
    }
}
