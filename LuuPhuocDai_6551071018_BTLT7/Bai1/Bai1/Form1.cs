using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Bai1.Models;

namespace Bai1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new TriThucBooksDBContext())
            {
                dgvTheLoai.DataSource = context.TheLoaiSaches.ToList();
            }
        }

        private void dgvTheLoai_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow != null && dgvTheLoai.CurrentRow.DataBoundItem is TheLoaiSach selected)
            {
                txtMaTL.Text = selected.MaTl.ToString();
                txtTenTheLoai.Text = selected.TenTheLoai;
                txtMoTa.Text = selected.MoTa;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTL.Text = "";
            txtTenTheLoai.Text = "";
            txtMoTa.Text = "";
            txtTimKiem.Text = "";
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenTheLoai.Text))
            {
                MessageBox.Show("Tên thể loại không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new TriThucBooksDBContext())
            {
                string tenMoi = txtTenTheLoai.Text.Trim();

                // Validate: Không cho phép trùng tên thể loại
                if (context.TheLoaiSaches.Any(x => x.TenTheLoai == tenMoi))
                {
                    MessageBox.Show("Tên thể loại này đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var item = new TheLoaiSach
                {
                    TenTheLoai = tenMoi,
                    MoTa = txtMoTa.Text.Trim(),
                    SoLuongSach = 0,
                    NgayTao = DateTime.Now
                };

                context.TheLoaiSaches.Add(item);
                context.SaveChanges();

                MessageBox.Show("Thêm thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                btnLamMoi_Click(sender, e);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTL.Text))
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenTheLoai.Text))
            {
                MessageBox.Show("Tên thể loại không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtMaTL.Text);
            using (var context = new TriThucBooksDBContext())
            {
                var item = context.TheLoaiSaches.Find(id);
                if (item != null)
                {
                    string tenMoi = txtTenTheLoai.Text.Trim();
                    // Kiểm tra trùng tên với các bản ghi khác
                    if (context.TheLoaiSaches.Any(x => x.TenTheLoai == tenMoi && x.MaTl != id))
                    {
                        MessageBox.Show("Tên thể loại này đã trùng với thể loại khác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    item.TenTheLoai = tenMoi;
                    item.MoTa = txtMoTa.Text.Trim();

                    context.SaveChanges();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTL.Text))
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa thể loại này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                int id = int.Parse(txtMaTL.Text);
                using (var context = new TriThucBooksDBContext())
                {
                    try
                    {
                        var item = context.TheLoaiSaches.Find(id);
                        if (item != null)
                        {
                            context.TheLoaiSaches.Remove(item);
                            context.SaveChanges();
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            btnLamMoi_Click(sender, e);
                        }
                    }
                    catch (DbUpdateException)
                    {
                        // Bắt lỗi ràng buộc khi thể loại đang được tham chiếu bởi bảng khác
                        MessageBox.Show("Không thể xóa vì thể loại này đang được sách tham chiếu!", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            using (var context = new TriThucBooksDBContext())
            {
                var result = context.TheLoaiSaches
                    .Where(x => x.TenTheLoai.ToLower().Contains(keyword))
                    .ToList();
                dgvTheLoai.DataSource = result;
            }
        }
    }
}
