using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Bai4.Models; // Đổi lại đúng namespace thư mục Models của bạn

namespace Bai4
{
    public partial class Form1 : Form
    {
        private int selectedMaLich = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dtpNgayKham.Format = DateTimePickerFormat.Short;
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Format = DateTimePickerFormat.Short;

            dtpGioKham.Format = DateTimePickerFormat.Time;
            dtpGioKham.ShowUpDown = true;

            LoadComboBoxBacSi();
            LoadComboBoxTrangThai();
            LoadDataToDataGridView();
        }

        private void LoadComboBoxBacSi()
        {
            try
            {
                using (var context = new AnKhangClinicDBContext())
                {
                    var listBS = context.BacSis.ToList();

                    var displayList = listBS.Select(bs => new
                    {
                        bs.MaBs,
                        HienThi = $"BS. {bs.HoTen} - {bs.ChuyenKhoa}"
                    }).ToList();

                    cboBacSi.DataSource = displayList;
                    cboBacSi.DisplayMember = "HienThi";
                    cboBacSi.ValueMember = "MaBs";

                    var listTimKiem = listBS.Select(bs => new
                    {
                        bs.MaBs,
                        HienThi = $"BS. {bs.HoTen} - {bs.ChuyenKhoa}"
                    }).ToList();

                    listTimKiem.Insert(0, new { MaBs = 0, HienThi = "Tất cả bác sĩ" });

                    cboTimKiemBacSi.DataSource = listTimKiem;
                    cboTimKiemBacSi.DisplayMember = "HienThi";
                    cboTimKiemBacSi.ValueMember = "MaBs";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bác sĩ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Chờ khám");
            cboTrangThai.Items.Add("Đã khám");
            cboTrangThai.Items.Add("Đã hủy");
            cboTrangThai.SelectedIndex = 0;

            cboTimKiemTrangThai.Items.Clear();
            cboTimKiemTrangThai.Items.Add("Tất cả trạng thái");
            cboTimKiemTrangThai.Items.Add("Chờ khám");
            cboTimKiemTrangThai.Items.Add("Đã khám");
            cboTimKiemTrangThai.Items.Add("Đã hủy");
            cboTimKiemTrangThai.SelectedIndex = 0;
        }

        private void LoadDataToDataGridView()
        {
            using (var context = new AnKhangClinicDBContext())
            {
                var query = context.LichKhams
                    .Include(lk => lk.MaBsNavigation)
                    .ToList();

                var dt = new DataTable();
                dt.Columns.Add("MaLich", typeof(int));
                dt.Columns.Add("TenBenhNhan", typeof(string));
                dt.Columns.Add("SDT", typeof(string));
                dt.Columns.Add("NgayKham", typeof(string));
                dt.Columns.Add("GioKham", typeof(string));
                dt.Columns.Add("HoTenBS", typeof(string));
                dt.Columns.Add("ChuyenKhoa", typeof(string));
                dt.Columns.Add("TrangThai", typeof(string));
                dt.Columns.Add("MaBS", typeof(int));

                foreach (var item in query)
                {
                    dt.Rows.Add(
                        item.MaLich,
                        item.TenBenhNhan,
                        item.Sdt,
                        item.NgayKham.HasValue ? item.NgayKham.Value.ToString("dd/MM/yyyy") : "",
                        item.GioKham ?? "",
                        item.MaBsNavigation?.HoTen ?? "",
                        item.MaBsNavigation?.ChuyenKhoa ?? "",
                        item.TrangThai,
                        item.MaBs
                    );
                }

                dgvLichKham.DataSource = dt;

                if (dgvLichKham.Columns["MaBS"] != null)
                {
                    dgvLichKham.Columns["MaBS"].Visible = false;
                }
            }
        }

        private void dgvLichKham_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLichKham.CurrentRow != null && dgvLichKham.CurrentRow.Cells["MaLich"].Value != null)
            {
                var cellValue = dgvLichKham.CurrentRow.Cells["MaLich"].Value;
                if (int.TryParse(cellValue.ToString(), out selectedMaLich))
                {
                    txtTenBenhNhan.Text = dgvLichKham.CurrentRow.Cells["TenBenhNhan"].Value?.ToString() ?? "";
                    txtSDT.Text = dgvLichKham.CurrentRow.Cells["SDT"].Value?.ToString() ?? "";

                    if (DateTime.TryParse(dgvLichKham.CurrentRow.Cells["NgayKham"].Value?.ToString(), out DateTime ngay))
                    {
                        dtpNgayKham.Value = ngay;
                    }

                    string gioStr = dgvLichKham.CurrentRow.Cells["GioKham"].Value?.ToString() ?? "00:00";
                    if (DateTime.TryParse(gioStr, out DateTime gio))
                    {
                        dtpGioKham.Value = gio;
                    }

                    if (int.TryParse(dgvLichKham.CurrentRow.Cells["MaBS"].Value?.ToString(), out int maBS))
                    {
                        cboBacSi.SelectedValue = maBS;
                    }

                    cboTrangThai.Text = dgvLichKham.CurrentRow.Cells["TrangThai"].Value?.ToString() ?? "";
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenBenhNhan.Text))
            {
                MessageBox.Show("Tên bệnh nhân không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenBenhNhan.Focus();
                return;
            }

            if (cboBacSi.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bác sĩ khám!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboBacSi.Focus();
                return;
            }

            if (dtpNgayKham.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Không thể đặt lịch khám vào ngày trong quá khứ!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpNgayKham.Focus();
                return;
            }

            int maBS = Convert.ToInt32(cboBacSi.SelectedValue);

            using (var context = new AnKhangClinicDBContext())
            {
                var lich = new LichKham
                {
                    TenBenhNhan = txtTenBenhNhan.Text.Trim(),
                    Sdt = txtSDT.Text.Trim(),
                    NgayKham = DateOnly.FromDateTime(dtpNgayKham.Value), // Chuyển đổi DateTime sang DateOnly
                    GioKham = dtpGioKham.Value.ToString("HH:mm"),
                    MaBs = maBS,
                    TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Chờ khám"
                };

                context.LichKhams.Add(lich);
                context.SaveChanges();

                MessageBox.Show("Thêm lịch khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataToDataGridView();
                btnLamMoi_Click(sender, e);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaLich == 0)
            {
                MessageBox.Show("Vui lòng chọn lịch khám cần sửa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenBenhNhan.Text))
            {
                MessageBox.Show("Tên bệnh nhân không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new AnKhangClinicDBContext())
            {
                var lich = context.LichKhams.Find(selectedMaLich);
                if (lich != null)
                {
                    lich.TenBenhNhan = txtTenBenhNhan.Text.Trim();
                    lich.Sdt = txtSDT.Text.Trim();
                    lich.NgayKham = DateOnly.FromDateTime(dtpNgayKham.Value); // Chuyển đổi DateTime sang DateOnly
                    lich.GioKham = dtpGioKham.Value.ToString("HH:mm");
                    lich.MaBs = Convert.ToInt32(cboBacSi.SelectedValue);
                    lich.TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Chờ khám";

                    context.SaveChanges();
                    MessageBox.Show("Cập nhật lịch khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToDataGridView();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaLich == 0)
            {
                MessageBox.Show("Vui lòng chọn lịch khám cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa lịch khám này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (var context = new AnKhangClinicDBContext())
                {
                    var lich = context.LichKhams.Find(selectedMaLich);
                    if (lich != null)
                    {
                        context.LichKhams.Remove(lich);
                        context.SaveChanges();

                        MessageBox.Show("Xóa lịch khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataToDataGridView();
                        btnLamMoi_Click(sender, e);
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            selectedMaLich = 0;
            txtTenBenhNhan.Text = "";
            txtSDT.Text = "";
            dtpNgayKham.Value = DateTime.Now;
            dtpGioKham.Value = DateTime.Now;
            if (cboBacSi.Items.Count > 0) cboBacSi.SelectedIndex = 0;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
            LoadDataToDataGridView();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            DateOnly tuNgay = DateOnly.FromDateTime(dtpTuNgay.Value);
            DateOnly denNgay = DateOnly.FromDateTime(dtpDenNgay.Value);

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu ('Từ ngày') không được lớn hơn ngày kết thúc ('Đến ngày')!", "Lỗi tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maBSTimKiem = Convert.ToInt32(cboTimKiemBacSi.SelectedValue);
            string trangThaiTim = cboTimKiemTrangThai.SelectedItem?.ToString() ?? "Tất cả trạng thái";

            using (var context = new AnKhangClinicDBContext())
            {
                var query = context.LichKhams
                    .Include(lk => lk.MaBsNavigation)
                    .Where(lk => lk.NgayKham >= tuNgay && lk.NgayKham <= denNgay)
                    .AsQueryable();

                if (maBSTimKiem > 0)
                {
                    query = query.Where(lk => lk.MaBs == maBSTimKiem);
                }

                if (trangThaiTim != "Tất cả trạng thái")
                {
                    query = query.Where(lk => lk.TrangThai == trangThaiTim);
                }

                var resultList = query.ToList();

                var dt = new DataTable();
                dt.Columns.Add("MaLich", typeof(int));
                dt.Columns.Add("TenBenhNhan", typeof(string));
                dt.Columns.Add("SDT", typeof(string));
                dt.Columns.Add("NgayKham", typeof(string));
                dt.Columns.Add("GioKham", typeof(string));
                dt.Columns.Add("HoTenBS", typeof(string));
                dt.Columns.Add("ChuyenKhoa", typeof(string));
                dt.Columns.Add("TrangThai", typeof(string));
                dt.Columns.Add("MaBS", typeof(int));

                foreach (var item in resultList)
                {
                    dt.Rows.Add(
                        item.MaLich,
                        item.TenBenhNhan,
                        item.Sdt,
                        item.NgayKham.HasValue ? item.NgayKham.Value.ToString("dd/MM/yyyy") : "",
                        item.GioKham ?? "",
                        item.MaBsNavigation?.HoTen ?? "",
                        item.MaBsNavigation?.ChuyenKhoa ?? "",
                        item.TrangThai,
                        item.MaBs
                    );
                }

                dgvLichKham.DataSource = dt;

                if (dgvLichKham.Columns["MaBS"] != null)
                {
                    dgvLichKham.Columns["MaBS"].Visible = false;
                }

                MessageBox.Show($"Tìm thấy {resultList.Count} kết quả phù hợp.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}