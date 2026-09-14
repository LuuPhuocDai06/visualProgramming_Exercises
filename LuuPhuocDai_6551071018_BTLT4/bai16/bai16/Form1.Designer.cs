using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai16
{
   
    public class Form1 : Form
    {
        private MenuStrip mnuMainMenu;
        private ContextMenuStrip mnuContext;
        private RichTextBox rtbText;

        private OpenFileDialog dlgOpenFile;
        private SaveFileDialog dlgSaveFile;
        private FontDialog dlgFont;
        private ColorDialog dlgColor;

        public Form1()
        {
            this.Text = "Bài 16 - MainMenu & ContextMenu";
            this.Width = 700;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ----- vùng soạn thảo -----
            rtbText = new RichTextBox
            {
                Dock = DockStyle.Fill
            };

            // ----- các Dialog dùng chung -----
            dlgOpenFile = new OpenFileDialog
            {
                Filter = "Rich Text Files (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };
            dlgSaveFile = new SaveFileDialog
            {
                Filter = "Rich Text Files (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };
            dlgFont = new FontDialog();
            dlgColor = new ColorDialog();

            BuildMainMenu();
            BuildContextMenu();

            rtbText.ContextMenuStrip = mnuContext;

            this.MainMenuStrip = mnuMainMenu;
            this.Controls.Add(rtbText);
            this.Controls.Add(mnuMainMenu); // MenuStrip phải add sau để Dock.Top nằm trên cùng

            this.FormClosing += Form1_FormClosing;
        }

     
        private void BuildMainMenu()
        {
            mnuMainMenu = new MenuStrip();

            // ---- Menu File ----
            ToolStripMenuItem mnuFile = new ToolStripMenuItem("&File");

            ToolStripMenuItem mnuNew = new ToolStripMenuItem("&New");
            mnuNew.Click += mnuNew_Click;

            ToolStripMenuItem mnuOpen = new ToolStripMenuItem("&Open...");
            mnuOpen.Click += mnuOpen_Click;

            ToolStripMenuItem mnuSaveAs = new ToolStripMenuItem("Save &As...");
            mnuSaveAs.Click += mnuSaveAs_Click;

            ToolStripMenuItem mnuExit = new ToolStripMenuItem("E&xit");
            mnuExit.Click += (s, e) => this.Close();

            mnuFile.DropDownItems.Add(mnuNew);
            mnuFile.DropDownItems.Add(mnuOpen);
            mnuFile.DropDownItems.Add(mnuSaveAs);
            mnuFile.DropDownItems.Add(new ToolStripSeparator());
            mnuFile.DropDownItems.Add(mnuExit);

            // ---- Menu Format ----
            ToolStripMenuItem mnuFormat = new ToolStripMenuItem("F&ormat");

            ToolStripMenuItem mnuFont = new ToolStripMenuItem("&Font...");
            mnuFont.Click += mnuFont_Click;

            ToolStripMenuItem mnuColor = new ToolStripMenuItem("&Color...");
            mnuColor.Click += mnuColor_Click;

            mnuFormat.DropDownItems.Add(mnuFont);
            mnuFormat.DropDownItems.Add(mnuColor);

            // ---- Menu Help ----
            ToolStripMenuItem mnuHelp = new ToolStripMenuItem("&Help");
            ToolStripMenuItem mnuAbout = new ToolStripMenuItem("&About...");
            mnuAbout.Click += (s, e) =>
                MessageBox.Show("Bài 16 - Demo MainMenu & ContextMenu\nSinh viên thực hiện.",
                    "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mnuHelp.DropDownItems.Add(mnuAbout);

            mnuMainMenu.Items.Add(mnuFile);
            mnuMainMenu.Items.Add(mnuFormat);
            mnuMainMenu.Items.Add(mnuHelp);
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            if (rtbText.Text.Length > 0)
            {
                DialogResult dl = MessageBox.Show("Nội dung hiện tại chưa lưu, bạn có muốn xóa để tạo mới?",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dl == DialogResult.No) return;
            }
            rtbText.Clear();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                string path = dlgOpenFile.FileName;
                if (path.ToLower().EndsWith(".rtf"))
                    rtbText.LoadFile(path, RichTextBoxStreamType.RichText);
                else
                    rtbText.LoadFile(path, RichTextBoxStreamType.PlainText);
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                string path = dlgSaveFile.FileName;
                if (path.ToLower().EndsWith(".rtf"))
                    rtbText.SaveFile(path, RichTextBoxStreamType.RichText);
                else
                    rtbText.SaveFile(path, RichTextBoxStreamType.PlainText);
            }
        }

        private void mnuFont_Click(object sender, EventArgs e)
        {
            dlgFont.Font = rtbText.SelectionFont ?? rtbText.Font;
            if (dlgFont.ShowDialog() == DialogResult.OK)
            {
                rtbText.SelectionFont = dlgFont.Font;
            }
        }

        private void mnuColor_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                rtbText.SelectionColor = dlgColor.Color;
            }
        }

        private void BuildContextMenu()
        {
            mnuContext = new ContextMenuStrip();

            ToolStripMenuItem mnuCut = new ToolStripMenuItem("Cu&t");
            mnuCut.Click += (s, e) => rtbText.Cut();

            ToolStripMenuItem mnuCopy = new ToolStripMenuItem("&Copy");
            mnuCopy.Click += (s, e) => rtbText.Copy();

            ToolStripMenuItem mnuPaste = new ToolStripMenuItem("&Paste");
            mnuPaste.Click += (s, e) => rtbText.Paste();

            mnuContext.Items.Add(mnuCut);
            mnuContext.Items.Add(mnuCopy);
            mnuContext.Items.Add(mnuPaste);

            // vô hiệu hóa Cut/Copy khi không có vùng chọn (xử lý ngay trước khi menu hiện lên)
            mnuContext.Opening += (s, e) =>
            {
                bool hasSelection = rtbText.SelectionLength > 0;
                mnuCut.Enabled = hasSelection;
                mnuCopy.Enabled = hasSelection;
                mnuPaste.Enabled = Clipboard.ContainsText();
            };
        }

        // đóng form có xác nhận (giống các bài trước)
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dl = MessageBox.Show("Có chắc bạn muốn đóng ứng dụng?", "Thông báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (dl == DialogResult.No)
                e.Cancel = true;
        }
    }
}