using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Bai3.Models;

namespace Bai3
{
    public partial class Form1 : Form
    {
        private int selectedMaPhong = 0;
        private string savedFileName = ""; // Lưu tên file ảnh hiện tại
        private string folderPath = Path.Combine(Application.StartupPath, "Images"); // Thư mục lưu ảnh

        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadComboBoxLoaiPhong();
            LoadComboBoxTinhTrang();
            LoadDataToDataGridView();
        }

        private void LoadComboBoxLoaiPhong()
        {
            try
            {
                using (var context = new SunriseHomestayDBContext())
                {
                    var listLoai = context.LoaiPhongs.ToList();

                    if (listLoai.Count == 0)
                    {
                        MessageBox.Show("Bảng LoaiPhong đang trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 1. Cấu hình cho ComboBox nhập liệu phòng
                    cboLoaiPhong.DataSource = null;
                    cboLoaiPhong.DisplayMember = "TenLoai";
                    cboLoaiPhong.ValueMember = "MaLoai";
                    cboLoaiPhong.DataSource = listLoai;

                    if (cboLoaiPhong.Items.Count > 0)
                    {
                        cboLoaiPhong.SelectedIndex = 0; // Chọn sẵn dòng đầu tiên
                    }

                    // 2. Cấu hình cho ComboBox lọc tìm kiếm (thêm dòng "Tất cả")
                    var listTimKiem = context.LoaiPhongs.ToList();
                    listTimKiem.Insert(0, new LoaiPhong { MaLoai = 0, TenLoai = "Tất cả loại phòng" });

                    cboTimKiemLoai.DataSource = null;
                    cboTimKiemLoai.DisplayMember = "TenLoai";
                    cboTimKiemLoai.ValueMember = "MaLoai";
                    cboTimKiemLoai.DataSource = listTimKiem;

                    if (cboTimKiemLoai.Items.Count > 0)
                    {
                        cboTimKiemLoai.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxTinhTrang()
        {
            // ComboBox tình trạng nhập liệu
            cboTinhTrang.Items.Clear();
            cboTinhTrang.Items.Add("Trống");
            cboTinhTrang.Items.Add("Đang ở");
            cboTinhTrang.Items.Add("Đang dọn");
            cboTinhTrang.SelectedIndex = 0;

            // ComboBox lọc tìm kiếm tình trạng
            cboTimKiemTinhTrang.Items.Clear();
            cboTimKiemTinhTrang.Items.Add("Tất cả tình trạng");
            cboTimKiemTinhTrang.Items.Add("Trống");
            cboTimKiemTinhTrang.Items.Add("Đang ở");
            cboTimKiemTinhTrang.Items.Add("Đang dọn");
            cboTimKiemTinhTrang.SelectedIndex = 0;
        }

        // Hiển thị danh sách phòng kết hợp Include lấy tên loại phòng và hình ảnh thumbnail
        private void LoadDataToDataGridView()
        {
            using (var context = new SunriseHomestayDBContext())
            {
                var query = context.Phongs
                    .Include(p => p.MaLoaiNavigation) // Eager Loading lấy thông tin loại phòng
                    .ToList();

                // Tạo DataTable để hiển thị lên DataGridView bao gồm cả cột ảnh dạng Image
                var dt = new System.Data.DataTable();
                dt.Columns.Add("MaPhong", typeof(int));
                dt.Columns.Add("Anh", typeof(System.Drawing.Image)); // Cột chứa hình ảnh thumbnail
                dt.Columns.Add("SoPhong", typeof(string));
                dt.Columns.Add("TangSo", typeof(int));
                dt.Columns.Add("TenLoai", typeof(string));
                dt.Columns.Add("GiaMoiDem", typeof(decimal));
                dt.Columns.Add("TinhTrang", typeof(string));
                dt.Columns.Add("HinhAnhFile", typeof(string)); // Cột ẩn lưu tên file ảnh

                foreach (var p in query)
                {
                    System.Drawing.Image img = null;
                    if (!string.IsNullOrEmpty(p.HinhAnh))
                    {
                        string fullPath = Path.Combine(folderPath, p.HinhAnh);
                        if (File.Exists(fullPath))
                        {
                            using (var tempImg = System.Drawing.Image.FromFile(fullPath))
                            {
                                img = new System.Drawing.Bitmap(tempImg, 60, 40); // Thu nhỏ kích thước làm thumbnail
                            }
                        }
                    }

                    dt.Rows.Add(
                        p.MaPhong,
                        img,
                        p.SoPhong,
                        p.TangSo,
                        p.MaLoaiNavigation?.TenLoai ?? "",
                        p.MaLoaiNavigation?.GiaMoiDem ?? 0,
                        p.TinhTrang,
                        p.HinhAnh
                    );
                }

                dgvPhong.DataSource = dt;

                // Định dạng hiển thị cột ảnh trên DataGridView là DataGridViewImageColumn
                if (dgvPhong.Columns["Anh"] is DataGridViewImageColumn imgCol)
                {
                    imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                }
            }
        }

        private void dgvPhong_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPhong.CurrentRow != null && dgvPhong.CurrentRow.Cells["MaPhong"].Value != null)
            {
                var cellValue = dgvPhong.CurrentRow.Cells["MaPhong"].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out selectedMaPhong))
                {
                    txtSoPhong.Text = dgvPhong.CurrentRow.Cells["SoPhong"].Value?.ToString() ?? "";

                    if (int.TryParse(dgvPhong.CurrentRow.Cells["TangSo"].Value?.ToString(), out int tang))
                    {
                        txtTangSo.Value = tang; // Nếu dùng NumericUpDown
                    }

                    cboLoaiPhong.Text = dgvPhong.CurrentRow.Cells["TenLoai"].Value?.ToString() ?? "";
                    cboTinhTrang.Text = dgvPhong.CurrentRow.Cells["TinhTrang"].Value?.ToString() ?? "";

                    savedFileName = dgvPhong.CurrentRow.Cells["HinhAnhFile"].Value?.ToString() ?? "";

                    // Hiển thị ảnh lên PictureBox
                    if (!string.IsNullOrEmpty(savedFileName))
                    {
                        string fullPath = Path.Combine(folderPath, savedFileName);
                        if (File.Exists(fullPath))
                        {
                            using (var stream = new MemoryStream(File.ReadAllBytes(fullPath)))
                            {
                                picHinhAnh.Image = System.Drawing.Image.FromStream(stream);
                            }
                        }
                        else
                        {
                            picHinhAnh.Image = null;
                        }
                    }
                    else
                    {
                        picHinhAnh.Image = null;
                    }
                }
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string sourcePath = ofd.FileName;
                    savedFileName = Guid.NewGuid().ToString() + Path.GetExtension(sourcePath);
                    string destPath = Path.Combine(folderPath, savedFileName);

                    // Copy file vào thư mục Images của ứng dụng
                    File.Copy(sourcePath, destPath, true);

                    // Hiển thị ảnh lên PictureBox vừa tạo
                    using (var stream = new MemoryStream(File.ReadAllBytes(destPath)))
                    {
                        picHinhAnh.Image = System.Drawing.Image.FromStream(stream);
                    }
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoPhong.Text))
            {
                MessageBox.Show("Số phòng không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoPhong.Focus();
                return;
            }

            // Kiểm tra xem ComboBox có đang chọn giá trị nào không
            if (cboLoaiPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn loại phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboLoaiPhong.Focus();
                return;
            }

            int maLoaiChon = 0;
            // Ép kiểu trực tiếp từ SelectedValue (vì ValueMember là "MaLoai" kiểu int)
            if (!int.TryParse(cboLoaiPhong.SelectedValue.ToString(), out maLoaiChon) || maLoaiChon == 0)
            {
                MessageBox.Show("Mã loại phòng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var context = new SunriseHomestayDBContext())
            {
                var phong = new Phong
                {
                    SoPhong = txtSoPhong.Text.Trim(),
                    TangSo = (int)txtTangSo.Value,
                    TinhTrang = cboTinhTrang.SelectedItem?.ToString() ?? "Trống",
                    HinhAnh = savedFileName,
                    MaLoai = maLoaiChon
                };

                context.Phongs.Add(phong);
                context.SaveChanges();

                MessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataToDataGridView();
                btnLamMoi_Click(sender, e);
            }
        }

        // Đã sửa lại đúng logic Sửa phòng
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaPhong == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần sửa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboLoaiPhong.SelectedValue == null || !int.TryParse(cboLoaiPhong.SelectedValue.ToString(), out int maLoaiSua))
            {
                MessageBox.Show("Vui lòng chọn loại phòng hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var context = new SunriseHomestayDBContext())
            {
                var phong = context.Phongs.Find(selectedMaPhong);
                if (phong != null)
                {
                    phong.SoPhong = txtSoPhong.Text.Trim();
                    phong.TangSo = (int)txtTangSo.Value;
                    phong.TinhTrang = cboTinhTrang.SelectedItem.ToString();
                    phong.HinhAnh = savedFileName;
                    phong.MaLoai = maLoaiSua; // Sử dụng biến đã ép kiểu an toàn

                    context.SaveChanges();
                    MessageBox.Show("Cập nhật phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToDataGridView();
                }
            }
        }

        // Đã sửa lại đúng logic Xóa phòng
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaPhong == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa phòng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (var context = new SunriseHomestayDBContext())
                {
                    var phong = context.Phongs.Find(selectedMaPhong);
                    if (phong != null)
                    {
                        context.Phongs.Remove(phong);
                        context.SaveChanges();
                        MessageBox.Show("Xóa phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataToDataGridView();
                        btnLamMoi_Click(sender, e);
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            selectedMaPhong = 0;
            txtSoPhong.Text = "";
            txtTangSo.Value = 1;
            cboLoaiPhong.SelectedIndex = 0;
            cboTinhTrang.SelectedIndex = 0;
            savedFileName = "";
            picHinhAnh.Image = null;
            cboTimKiemLoai.SelectedIndex = 0;
            cboTimKiemTinhTrang.SelectedIndex = 0;
            LoadDataToDataGridView();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (var context = new SunriseHomestayDBContext())
            {
                var query = context.Phongs
                    .Include(p => p.MaLoaiNavigation)
                    .AsQueryable();

                // Kiểm tra an toàn giá trị chọn của ComboBox tìm kiếm loại phòng
                if (cboTimKiemLoai.SelectedValue != null && int.TryParse(cboTimKiemLoai.SelectedValue.ToString(), out int maLoaiTimKiem))
                {
                    if (maLoaiTimKiem > 0)
                    {
                        query = query.Where(p => p.MaLoai == maLoaiTimKiem);
                    }
                }

                // Kiểm tra an toàn giá trị chọn của ComboBox tìm kiếm tình trạng
                if (cboTimKiemTinhTrang.SelectedItem != null)
                {
                    string tinhTrangTimKiem = cboTimKiemTinhTrang.SelectedItem.ToString();
                    if (!string.IsNullOrEmpty(tinhTrangTimKiem) && tinhTrangTimKiem != "Tất cả tình trạng")
                    {
                        query = query.Where(p => p.TinhTrang == tinhTrangTimKiem);
                    }
                }

                var resultList = query.ToList();

                var dt = new System.Data.DataTable();
                dt.Columns.Add("MaPhong", typeof(int));
                dt.Columns.Add("Anh", typeof(System.Drawing.Image));
                dt.Columns.Add("SoPhong", typeof(string));
                dt.Columns.Add("TangSo", typeof(int));
                dt.Columns.Add("TenLoai", typeof(string));
                dt.Columns.Add("GiaMoiDem", typeof(decimal));
                dt.Columns.Add("TinhTrang", typeof(string));
                dt.Columns.Add("HinhAnhFile", typeof(string));

                foreach (var p in resultList)
                {
                    System.Drawing.Image img = null;
                    if (!string.IsNullOrEmpty(p.HinhAnh))
                    {
                        string fullPath = Path.Combine(folderPath, p.HinhAnh);
                        if (File.Exists(fullPath))
                        {
                            using (var tempImg = System.Drawing.Image.FromFile(fullPath))
                            {
                                img = new System.Drawing.Bitmap(tempImg, 60, 40);
                            }
                        }
                    }

                    dt.Rows.Add(
                        p.MaPhong,
                        img,
                        p.SoPhong,
                        p.TangSo,
                        p.MaLoaiNavigation?.TenLoai ?? "",
                        p.MaLoaiNavigation?.GiaMoiDem ?? 0,
                        p.TinhTrang,
                        p.HinhAnh
                    );
                }

                dgvPhong.DataSource = dt;
            }
        }
    }
}