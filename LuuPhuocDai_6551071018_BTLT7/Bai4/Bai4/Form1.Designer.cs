namespace Bai4
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
            txtTenBenhNhan = new TextBox();
            txtSDT = new TextBox();
            dtpNgayKham = new DateTimePicker();
            label4 = new Label();
            dtpGioKham = new DateTimePicker();
            label5 = new Label();
            label6 = new Label();
            cboBacSi = new ComboBox();
            cboTrangThai = new ComboBox();
            dtpTuNgay = new DateTimePicker();
            dtpDenNgay = new DateTimePicker();
            label7 = new Label();
            label8 = new Label();
            cboTimKiemBacSi = new ComboBox();
            btnTimKiem = new Button();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            cboTimKiemTrangThai = new ComboBox();
            dgvLichKham = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvLichKham).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 21);
            label1.Name = "label1";
            label1.Size = new Size(105, 20);
            label1.TabIndex = 0;
            label1.Text = "Tên bệnh nhân";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 59);
            label2.Name = "label2";
            label2.Size = new Size(97, 20);
            label2.TabIndex = 1;
            label2.Text = "Số điện thoại";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 98);
            label3.Name = "label3";
            label3.Size = new Size(84, 20);
            label3.TabIndex = 2;
            label3.Text = "Ngày khám";
            // 
            // txtTenBenhNhan
            // 
            txtTenBenhNhan.Location = new Point(135, 18);
            txtTenBenhNhan.Name = "txtTenBenhNhan";
            txtTenBenhNhan.Size = new Size(185, 27);
            txtTenBenhNhan.TabIndex = 3;
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(135, 57);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(185, 27);
            txtSDT.TabIndex = 4;
            // 
            // dtpNgayKham
            // 
            dtpNgayKham.Format = DateTimePickerFormat.Short;
            dtpNgayKham.Location = new Point(135, 96);
            dtpNgayKham.Name = "dtpNgayKham";
            dtpNgayKham.Size = new Size(185, 27);
            dtpNgayKham.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(351, 60);
            label4.Name = "label4";
            label4.Size = new Size(72, 20);
            label4.TabIndex = 6;
            label4.Text = "Giờ khám";
            // 
            // dtpGioKham
            // 
            dtpGioKham.Format = DateTimePickerFormat.Time;
            dtpGioKham.Location = new Point(351, 96);
            dtpGioKham.Name = "dtpGioKham";
            dtpGioKham.Size = new Size(105, 27);
            dtpGioKham.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(493, 60);
            label5.Name = "label5";
            label5.Size = new Size(47, 20);
            label5.TabIndex = 8;
            label5.Text = "Bác sĩ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(771, 64);
            label6.Name = "label6";
            label6.Size = new Size(75, 20);
            label6.TabIndex = 9;
            label6.Text = "Trạng thái";
            // 
            // cboBacSi
            // 
            cboBacSi.FormattingEnabled = true;
            cboBacSi.Items.AddRange(new object[] { "BS. Nguyễn Văn A - Nội tổng quát", "BS. Nguyễn Văn B - Nội tổng quát", "BS. Nguyễn Văn C - Nội tổng quát", "BS. Nguyễn Văn D - Nội tổng quát" });
            cboBacSi.Location = new Point(493, 98);
            cboBacSi.Name = "cboBacSi";
            cboBacSi.Size = new Size(244, 28);
            cboBacSi.TabIndex = 10;
            // 
            // cboTrangThai
            // 
            cboTrangThai.FormattingEnabled = true;
            cboTrangThai.Items.AddRange(new object[] { "Chờ khám", "Đã khám", "Đã hủy" });
            cboTrangThai.Location = new Point(771, 98);
            cboTrangThai.Name = "cboTrangThai";
            cboTrangThai.Size = new Size(205, 28);
            cboTrangThai.TabIndex = 11;
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpTuNgay.Location = new Point(92, 199);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(139, 27);
            dtpTuNgay.TabIndex = 12;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Location = new Point(322, 196);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(134, 27);
            dtpDenNgay.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(24, 201);
            label7.Name = "label7";
            label7.Size = new Size(62, 20);
            label7.TabIndex = 14;
            label7.Text = "Từ ngày";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(244, 198);
            label8.Name = "label8";
            label8.Size = new Size(72, 20);
            label8.TabIndex = 15;
            label8.Text = "Đến ngày";
            // 
            // cboTimKiemBacSi
            // 
            cboTimKiemBacSi.FormattingEnabled = true;
            cboTimKiemBacSi.Items.AddRange(new object[] { "BS. Nguyễn Văn A - Nội tổng quát", "BS. Nguyễn Văn B - Nội tổng quát", "BS. Nguyễn Văn C - Nội tổng quát", "BS. Nguyễn Văn D - Nội tổng quát" });
            cboTimKiemBacSi.Location = new Point(493, 198);
            cboTimKiemBacSi.Name = "cboTimKiemBacSi";
            cboTimKiemBacSi.Size = new Size(244, 28);
            cboTimKiemBacSi.TabIndex = 16;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(993, 198);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(94, 29);
            btnTimKiem.TabIndex = 17;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(658, 22);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 18;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(771, 21);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 19;
            btnSua.Text = "Xóa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(882, 22);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 20;
            btnXoa.Text = "Sửa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(993, 22);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 22;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // cboTimKiemTrangThai
            // 
            cboTimKiemTrangThai.FormattingEnabled = true;
            cboTimKiemTrangThai.Items.AddRange(new object[] { "Chờ khám", "Đã khám", "Đã hủy" });
            cboTimKiemTrangThai.Location = new Point(771, 198);
            cboTimKiemTrangThai.Name = "cboTimKiemTrangThai";
            cboTimKiemTrangThai.Size = new Size(205, 28);
            cboTimKiemTrangThai.TabIndex = 23;
            // 
            // dgvLichKham
            // 
            dgvLichKham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLichKham.Location = new Point(1, 232);
            dgvLichKham.Name = "dgvLichKham";
            dgvLichKham.RowHeadersWidth = 51;
            dgvLichKham.Size = new Size(1127, 264);
            dgvLichKham.TabIndex = 24;
            dgvLichKham.SelectionChanged += dgvLichKham_SelectionChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1127, 492);
            Controls.Add(dgvLichKham);
            Controls.Add(cboTimKiemTrangThai);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(btnTimKiem);
            Controls.Add(cboTimKiemBacSi);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(dtpDenNgay);
            Controls.Add(dtpTuNgay);
            Controls.Add(cboTrangThai);
            Controls.Add(cboBacSi);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(dtpGioKham);
            Controls.Add(label4);
            Controls.Add(dtpNgayKham);
            Controls.Add(txtSDT);
            Controls.Add(txtTenBenhNhan);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvLichKham).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtTenBenhNhan;
        private TextBox txtSDT;
        private DateTimePicker dtpNgayKham;
        private Label label4;
        private DateTimePicker dtpGioKham;
        private Label label5;
        private Label label6;
        private ComboBox cboBacSi;
        private ComboBox cboTrangThai;
        private DateTimePicker dtpTuNgay;
        private DateTimePicker dtpDenNgay;
        private Label label7;
        private Label label8;
        private ComboBox cboTimKiemBacSi;
        private Button btnTimKiem;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private ComboBox cboTimKiemTrangThai;
        private DataGridView dgvLichKham;
    }
}
