using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Bai_2.Models;

namespace Bai_2
{
    public partial class Form1 : Form
    {
        private int selectedMaHV = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadComboBoxFilter();
            LoadData();
        }

        private void LoadComboBoxFilter()
        {
            // Nạp dữ liệu cứng cho ComboBox lọc tìm kiếm
            cboTimKiemHang.Items.Clear();
            cboTimKiemHang.Items.Add("Tất cả");
            cboTimKiemHang.Items.Add("Basic");
            cboTimKiemHang.Items.Add("VIP");
            cboTimKiemHang.Items.Add("Premium");
            cboTimKiemHang.SelectedIndex = 0;
        }

        private void LoadData()
        {
            using (var context = new FitZoneDBContext())
            {
                dgvHoiVien.DataSource = context.HoiViens.ToList();
            }
        }

        private void dgvHoiVien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHoiVien.CurrentRow != null && dgvHoiVien.CurrentRow.DataBoundItem is HoiVien hv)
            {
                selectedMaHV = hv.MaHv; // Lưu lại mã hội viên đang chọn
                txtHoTen.Text = hv.HoTen;
                txtSDT.Text = hv.Sdt;
                txtEmail.Text = hv.Email;

                // Xử lý RadioButton Giới tính (1 = Nam, 0 = Nữ)
                if (hv.GioiTinh == true)
                    rbNam.Checked = true;
                else
                    rbNu.Checked = true;

                // Xử lý Ngày sinh (Chuyển từ DateOnly sang DateTime để gán cho DateTimePicker)
                if (hv.NgaySinh.HasValue)
                {
                    dtpNgaySinh.Value = hv.NgaySinh.Value.ToDateTime(TimeOnly.MinValue);
                }
                else
                {
                    dtpNgaySinh.Value = DateTime.Now;
                }

                // Xử lý ComboBox Hạng thành viên
                cboHangThanhVien.Text = hv.HangThanhVien;

                // Xử lý CheckBox Trạng thái
                chkTrangThai.Checked = hv.TrangThai ?? false;
            }
        }

        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHoTen.Focus();
                return false;
            }

            // SDT chỉ chứa chữ số và đủ từ 9 đến 11 ký tự
            string sdt = txtSDT.Text.Trim();
            if (!Regex.IsMatch(sdt, @"^\d{9,11}$"))
            {
                MessageBox.Show("Số điện thoại phải chỉ chứa chữ số và có độ dài từ 9 đến 11 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus();
                return false;
            }

            // Email phải chứa ký tự '@'
            string email = txtEmail.Text.Trim();
            if (!email.Contains("@"))
            {
                MessageBox.Show("Email không hợp lệ (phải chứa ký tự '@')!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return false;
            }

            // Tuổi phải từ 15 trở lên tính từ NgaySinh đến hiện tại
            int age = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (dtpNgaySinh.Value.Date > DateTime.Now.AddYears(-age)) age--;
            if (age < 15)
            {
                MessageBox.Show("Hội viên phải từ 15 tuổi trở lên mới được đăng ký!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpNgaySinh.Focus();
                return false;
            }

            if (cboHangThanhVien.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn hạng thành viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaHV == 0)
            {
                MessageBox.Show("Vui lòng chọn hội viên cần sửa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateData()) return;

            using (var context = new FitZoneDBContext())
            {
                var hv = context.HoiViens.Find(selectedMaHV);
                if (hv != null)
                {
                    hv.HoTen = txtHoTen.Text.Trim();
                    hv.GioiTinh = rbNam.Checked ? true : false;
                    hv.NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value);
                    hv.Sdt = txtSDT.Text.Trim();
                    hv.Email = txtEmail.Text.Trim();
                    hv.HangThanhVien = cboHangThanhVien.SelectedItem.ToString();
                    hv.TrangThai = chkTrangThai.Checked;

                    context.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaHV == 0)
            {
                MessageBox.Show("Vui lòng chọn hội viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa hội viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (var context = new FitZoneDBContext())
                {
                    var hv = context.HoiViens.Find(selectedMaHV);
                    if (hv != null)
                    {
                        context.HoiViens.Remove(hv);
                        context.SaveChanges();
                        MessageBox.Show("Xóa hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        btnLamMoi_Click(sender, e);
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            rbNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Now;
            cboHangThanhVien.SelectedIndex = 0;
            chkTrangThai.Checked = true;
            txtTimKiemTen.Text = "";
            cboTimKiemHang.SelectedIndex = 0;
            LoadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiemTen.Text.Trim().ToLower();
            string selectedHang = cboTimKiemHang.SelectedItem?.ToString() ?? "Tất cả";

            using (var context = new FitZoneDBContext())
            {
                var query = context.HoiViens.AsQueryable();

                // Điều kiện 1: Tìm theo Họ tên gần đúng
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(x => x.HoTen.ToLower().Contains(keyword));
                }

                // Điều kiện 2: Lọc theo Hạng thành viên (nếu khác "Tất cả")
                if (selectedHang != "Tất cả")
                {
                    query = query.Where(x => x.HangThanhVien == selectedHang);
                }

                dgvHoiVien.DataSource = query.ToList();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            using (var context = new FitZoneDBContext())
            {
                var hv = new HoiVien
                {
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = rbNam.Checked ? true : false,
                    NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value),
                    Sdt = txtSDT.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    HangThanhVien = cboHangThanhVien.SelectedItem.ToString(),
                    NgayDangKy = DateTime.Now,
                    TrangThai = chkTrangThai.Checked
                };

                context.HoiViens.Add(hv);
                context.SaveChanges();

                MessageBox.Show("Thêm hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                btnLamMoi_Click(sender, e);
            }
        }

        private void dgvHoiVien_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}