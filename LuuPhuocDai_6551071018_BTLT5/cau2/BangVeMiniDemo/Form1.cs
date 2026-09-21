using System;
using System.Drawing;
using System.Windows.Forms;

namespace BangVeMiniDemo
{
    public partial class Form1 : Form
    {
        // Bitmap dùng làm "canvas" để lưu lại nét vẽ (không bị mất khi Panel vẽ lại)
        private Bitmap canvasBitmap;

        // Cờ đánh dấu đang giữ chuột trái để vẽ hay không
        private bool dangVe = false;

        // Điểm chuột ở lần MouseMove trước đó, dùng để nối thành đoạn thẳng
        private Point diemTruoc;

        public Form1()
        {
            InitializeComponent();
        }

        // Khởi tạo Bitmap trắng có kích thước bằng Panel khi Form load
        private void Form1_Load(object sender, EventArgs e)
        {
            TaoCanvasMoi();
        }

        private void TaoCanvasMoi()
        {
            canvasBitmap = new Bitmap(pnlCanvas.Width, pnlCanvas.Height);
            using (Graphics g = Graphics.FromImage(canvasBitmap))
            {
                g.Clear(Color.White);
            }
            pnlCanvas.Invalidate();
        }

        // Vẽ lại nội dung Bitmap lên Panel mỗi khi Panel cần refresh
        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (canvasBitmap != null)
            {
                e.Graphics.DrawImage(canvasBitmap, 0, 0);
            }
        }

        // MouseDown: đánh dấu bắt đầu nét vẽ khi giữ chuột trái
        private void pnlCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dangVe = true;
                diemTruoc = e.Location;
                lblViTri.Text = "Đang vẽ... | X: " + e.X + ", Y: " + e.Y;
            }
        }

        // MouseMove: luôn cập nhật tọa độ; nếu đang giữ chuột trái thì vẽ đoạn nối liên tục
        private void pnlCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Cập nhật tọa độ chuột hiện tại, kèm trạng thái đang vẽ hay sẵn sàng
            string trangThai = dangVe ? "Đang vẽ..." : "Sẵn sàng";
            lblViTri.Text = trangThai + " | X: " + e.X + ", Y: " + e.Y;

            if (dangVe && e.Button == MouseButtons.Left)
            {
                using (Graphics g = Graphics.FromImage(canvasBitmap))
                using (Pen but = new Pen(Color.Black, 3))
                {
                    but.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    but.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    g.DrawLine(but, diemTruoc, e.Location);
                }
                diemTruoc = e.Location;
                pnlCanvas.Invalidate();
            }
        }

        // MouseUp: kết thúc nét vẽ
        private void pnlCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dangVe = false;
                lblViTri.Text = "Sẵn sàng | X: " + e.X + ", Y: " + e.Y;
            }
        }

        // MouseClick chuột phải: xóa trắng toàn bộ Panel
        private void pnlCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TaoCanvasMoi();
            }
        }
    }
}