namespace Bai3
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
            txtSoPhong = new TextBox();
            txtTangSo = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            cboLoaiPhong = new ComboBox();
            cboTinhTrang = new ComboBox();
            picHinhAnh = new PictureBox();
            btnChonAnh = new Button();
            btnXoa = new Button();
            btnThem = new Button();
            btnSua = new Button();
            btnLamMoi = new Button();
            button6 = new Button();
            cboTimKiemTinhTrang = new ComboBox();
            cboTimKiemLoai = new ComboBox();
            dgvPhong = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)txtTangSo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHinhAnh).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPhong).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 19);
            label1.Name = "label1";
            label1.Size = new Size(73, 20);
            label1.TabIndex = 0;
            label1.Text = "Số phòng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(155, 19);
            label2.Name = "label2";
            label2.Size = new Size(61, 20);
            label2.TabIndex = 1;
            label2.Text = "Tầng số";
            // 
            // txtSoPhong
            // 
            txtSoPhong.Location = new Point(10, 44);
            txtSoPhong.Name = "txtSoPhong";
            txtSoPhong.Size = new Size(116, 27);
            txtSoPhong.TabIndex = 2;
            // 
            // txtTangSo
            // 
            txtTangSo.Location = new Point(155, 45);
            txtTangSo.Name = "txtTangSo";
            txtTangSo.Size = new Size(116, 27);
            txtTangSo.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(299, 19);
            label3.Name = "label3";
            label3.Size = new Size(84, 20);
            label3.TabIndex = 4;
            label3.Text = "Loại phòng";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(445, 19);
            label4.Name = "label4";
            label4.Size = new Size(76, 20);
            label4.TabIndex = 5;
            label4.Text = "Tình trạng";
            // 
            // cboLoaiPhong
            // 
            cboLoaiPhong.FormattingEnabled = true;
            cboLoaiPhong.Items.AddRange(new object[] { "Phòng đơn", "Phòng đôi", "Phòng VIP" });
            cboLoaiPhong.Location = new Point(299, 45);
            cboLoaiPhong.Name = "cboLoaiPhong";
            cboLoaiPhong.Size = new Size(116, 28);
            cboLoaiPhong.TabIndex = 6;
            // 
            // cboTinhTrang
            // 
            cboTinhTrang.FormattingEnabled = true;
            cboTinhTrang.Items.AddRange(new object[] { "Trống", "Đang ở", "Đang dọn" });
            cboTinhTrang.Location = new Point(445, 45);
            cboTinhTrang.Name = "cboTinhTrang";
            cboTinhTrang.Size = new Size(116, 28);
            cboTinhTrang.TabIndex = 7;
            // 
            // picHinhAnh
            // 
            picHinhAnh.Location = new Point(593, 13);
            picHinhAnh.Name = "picHinhAnh";
            picHinhAnh.Size = new Size(94, 75);
            picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
            picHinhAnh.TabIndex = 8;
            picHinhAnh.TabStop = false;
            // 
            // btnChonAnh
            // 
            btnChonAnh.Location = new Point(593, 94);
            btnChonAnh.Name = "btnChonAnh";
            btnChonAnh.Size = new Size(94, 29);
            btnChonAnh.TabIndex = 9;
            btnChonAnh.Text = "Chọn ảnh";
            btnChonAnh.UseVisualStyleBackColor = true;
            btnChonAnh.Click += btnChonAnh_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(841, 42);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 10;
            btnXoa.Text = "Sửa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(741, 42);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 11;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(941, 43);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 12;
            btnSua.Text = "Xóa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(1041, 44);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 13;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // button6
            // 
            button6.Location = new Point(481, 175);
            button6.Name = "button6";
            button6.Size = new Size(94, 29);
            button6.TabIndex = 14;
            button6.Text = "Tìm kiếm";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // cboTimKiemTinhTrang
            // 
            cboTimKiemTinhTrang.FormattingEnabled = true;
            cboTimKiemTinhTrang.Items.AddRange(new object[] { "Phòng đơn", "Phòng đôi", "Phòng VIP" });
            cboTimKiemTinhTrang.Location = new Point(243, 175);
            cboTimKiemTinhTrang.Name = "cboTimKiemTinhTrang";
            cboTimKiemTinhTrang.Size = new Size(207, 28);
            cboTimKiemTinhTrang.TabIndex = 15;
            // 
            // cboTimKiemLoai
            // 
            cboTimKiemLoai.FormattingEnabled = true;
            cboTimKiemLoai.Items.AddRange(new object[] { "Phòng đơn", "Phòng đôi", "Phòng VIP" });
            cboTimKiemLoai.Location = new Point(10, 175);
            cboTimKiemLoai.Name = "cboTimKiemLoai";
            cboTimKiemLoai.Size = new Size(207, 28);
            cboTimKiemLoai.TabIndex = 16;
            // 
            // dgvPhong
            // 
            dgvPhong.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPhong.Location = new Point(1, 224);
            dgvPhong.Name = "dgvPhong";
            dgvPhong.RowHeadersWidth = 51;
            dgvPhong.Size = new Size(1142, 273);
            dgvPhong.TabIndex = 17;
            dgvPhong.SelectionChanged += dgvPhong_SelectionChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1144, 495);
            Controls.Add(dgvPhong);
            Controls.Add(cboTimKiemLoai);
            Controls.Add(cboTimKiemTinhTrang);
            Controls.Add(button6);
            Controls.Add(btnLamMoi);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(btnXoa);
            Controls.Add(btnChonAnh);
            Controls.Add(picHinhAnh);
            Controls.Add(cboTinhTrang);
            Controls.Add(cboLoaiPhong);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txtTangSo);
            Controls.Add(txtSoPhong);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)txtTangSo).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHinhAnh).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPhong).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtSoPhong;
        private NumericUpDown txtTangSo;
        private Label label3;
        private Label label4;
        private ComboBox cboLoaiPhong;
        private ComboBox cboTinhTrang;
        private PictureBox picHinhAnh;
        private Button btnChonAnh;
        private Button btnXoa;
        private Button btnThem;
        private Button btnSua;
        private Button btnLamMoi;
        private Button button6;
        private ComboBox cboTimKiemTinhTrang;
        private ComboBox cboTimKiemLoai;
        private DataGridView dgvPhong;
    }
}
