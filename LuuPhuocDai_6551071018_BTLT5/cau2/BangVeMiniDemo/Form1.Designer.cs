namespace BangVeMiniDemo
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Label lblViTri;

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
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.lblViTri = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // lblViTri
            // 
            this.lblViTri.AutoSize = true;
            this.lblViTri.Location = new System.Drawing.Point(12, 9);
            this.lblViTri.Name = "lblViTri";
            this.lblViTri.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblViTri.Size = new System.Drawing.Size(120, 20);
            this.lblViTri.Text = "Sẵn sàng | X: 0, Y: 0";

            // 
            // pnlCanvas
            // 
            this.pnlCanvas.BackColor = System.Drawing.Color.White;
            this.pnlCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCanvas.Location = new System.Drawing.Point(12, 40);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(700, 450);
            this.pnlCanvas.TabIndex = 0;
            this.pnlCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCanvas_Paint);
            this.pnlCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlCanvas_MouseDown);
            this.pnlCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlCanvas_MouseMove);
            this.pnlCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlCanvas_MouseUp);
            this.pnlCanvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlCanvas_MouseClick);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(730, 505);
            this.Controls.Add(this.lblViTri);
            this.Controls.Add(this.pnlCanvas);
            this.Name = "Form1";
            this.Text = "Bảng vẽ mini - Ghi chú nhanh bằng tay";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}