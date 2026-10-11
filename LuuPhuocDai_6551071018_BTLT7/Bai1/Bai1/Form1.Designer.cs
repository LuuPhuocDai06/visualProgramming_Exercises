namespace Bai1
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
            txtMaTL = new TextBox();
            txtTenTheLoai = new TextBox();
            txtMoTa = new TextBox();
            btnThem = new Button();
            btnLamMoi = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            dgvTheLoai = new DataGridView();
            btnTimKiem = new Button();
            txtTimKiem = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvTheLoai).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 23);
            label1.Name = "label1";
            label1.Size = new Size(84, 20);
            label1.TabIndex = 0;
            label1.Text = "Mã thể loại";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 76);
            label2.Name = "label2";
            label2.Size = new Size(86, 20);
            label2.TabIndex = 1;
            label2.Text = "Tên thể loại";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 123);
            label3.Name = "label3";
            label3.Size = new Size(48, 20);
            label3.TabIndex = 2;
            label3.Text = "Mô tả";
            // 
            // txtMaTL
            // 
            txtMaTL.Location = new Point(186, 16);
            txtMaTL.Name = "txtMaTL";
            txtMaTL.ReadOnly = true;
            txtMaTL.Size = new Size(196, 27);
            txtMaTL.TabIndex = 3;
            // 
            // txtTenTheLoai
            // 
            txtTenTheLoai.Location = new Point(186, 69);
            txtTenTheLoai.Name = "txtTenTheLoai";
            txtTenTheLoai.Size = new Size(196, 27);
            txtTenTheLoai.TabIndex = 4;
            // 
            // txtMoTa
            // 
            txtMoTa.Location = new Point(186, 120);
            txtMoTa.Multiline = true;
            txtMoTa.Name = "txtMoTa";
            txtMoTa.Size = new Size(248, 78);
            txtMoTa.TabIndex = 5;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(490, 14);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(826, 14);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 7;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(601, 14);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 8;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(710, 14);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 7;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // dgvTheLoai
            // 
            dgvTheLoai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTheLoai.Location = new Point(0, 278);
            dgvTheLoai.Name = "dgvTheLoai";
            dgvTheLoai.RowHeadersWidth = 51;
            dgvTheLoai.Size = new Size(930, 197);
            dgvTheLoai.TabIndex = 9;
            dgvTheLoai.SelectionChanged += dgvTheLoai_SelectionChanged;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(304, 229);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(94, 29);
            btnTimKiem.TabIndex = 10;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(23, 229);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(251, 27);
            txtTimKiem.TabIndex = 11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(932, 478);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTimKiem);
            Controls.Add(dgvTheLoai);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnThem);
            Controls.Add(txtMoTa);
            Controls.Add(txtTenTheLoai);
            Controls.Add(txtMaTL);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTheLoai).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtMaTL;
        private TextBox txtTenTheLoai;
        private TextBox txtMoTa;
        private Button btnThem;
        private Button btnLamMoi;
        private Button btnSua;
        private Button btnXoa;
        private DataGridView dgvTheLoai;
        private Button btnTimKiem;
        private TextBox txtTimKiem;
    }
}
