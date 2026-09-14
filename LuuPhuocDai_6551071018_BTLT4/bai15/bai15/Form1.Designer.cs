using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai15
{
    /// <summary>
    /// Bài 15 - Sử dụng các control của Visual Studio:
    /// Ex1: DomainUpDown & NumericUpDown
    /// Ex2: MonthCalendar & DateTimePicker
    /// Ex3: Timer, TrackBar, ProgressBar
    /// Ex4: TabControl (tạo message box theo lựa chọn)
    /// Toàn bộ control được tạo bằng code (không cần file Designer riêng).
    /// </summary>
    public class Form1 : Form
    {
        // ====== Ex1: DomainUpDown & NumericUpDown ======
        private DomainUpDown dudColor;
        private NumericUpDown nudSize;
        private Label lblSampleText;

        // ====== Ex2: MonthCalendar & DateTimePicker ======
        private MonthCalendar mcalTravel;
        private Label lblStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpLaunch;
        private RadioButton radLongDate;
        private RadioButton radShortDate;

        // ====== Ex3: Timer, TrackBar, ProgressBar ======
        private ProgressBar progressBar1;
        private Label lblPercent;
        private TrackBar trackBar1;
        private System.Windows.Forms.Timer timer1;

        // ====== Ex4: TabControl demo tạo MessageBox ======
        private TabControl tabControlDemo;
        private TextBox txtMessage;
        private TextBox txtCaption;
        private RadioButton[] radButtons;
        private RadioButton[] radIcons;
        private RadioButton[] radTabLayout;

        private TabControl tabControlMain;

        public Form1()
        {
            this.Text = "Bài 15 - Step by step Controls";
            this.Width = 620;
            this.Height = 520;
            this.StartPosition = FormStartPosition.CenterScreen;

            tabControlMain = new TabControl();
            tabControlMain.Dock = DockStyle.Fill;
            this.Controls.Add(tabControlMain);

            TabPage tab1 = new TabPage("Ex1: Up-Down Controls");
            TabPage tab2 = new TabPage("Ex2: Calendar/DateTimePicker");
            TabPage tab3 = new TabPage("Ex3: Timer/TrackBar/ProgressBar");
            TabPage tab4 = new TabPage("Ex4: TabControl Demo");
            tabControlMain.TabPages.AddRange(new TabPage[] { tab1, tab2, tab3, tab4 });

            BuildTab1(tab1);
            BuildTab2(tab2);
            BuildTab3(tab3);
            BuildTab4(tab4);
        }

        // ===================================================================
        // Ex1: Using DomainUpDown and NumericUpDown Controls
        // ===================================================================
        private void BuildTab1(TabPage tab)
        {
            Label lblColor = new Label { Text = "Color:", Location = new Point(30, 40), AutoSize = true };
            dudColor = new DomainUpDown { Location = new Point(120, 37), Width = 150 };
            dudColor.Items.Add("Red");
            dudColor.Items.Add("Green");
            dudColor.Items.Add("Blue");
            dudColor.Items.Add("Black");
            dudColor.Items.Add("Orange");
            dudColor.SelectedIndex = 0;
            dudColor.SelectedItemChanged += dudColor_SelectedItemChanged;

            Label lblSize = new Label { Text = "Size:", Location = new Point(30, 90), AutoSize = true };
            nudSize = new NumericUpDown
            {
                Location = new Point(120, 87),
                Width = 100,
                Minimum = 8,
                Maximum = 72,
                Value = 16
            };
            nudSize.ValueChanged += nudSize_ValueChanged;

            lblSampleText = new Label
            {
                Text = "Sample Text",
                Location = new Point(30, 150),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 16),
                ForeColor = Color.Red
            };

            tab.Controls.AddRange(new Control[] { lblColor, dudColor, lblSize, nudSize, lblSampleText });
        }

        // sự kiện khi đổi màu trong DomainUpDown
        private void dudColor_SelectedItemChanged(object sender, EventArgs e)
        {
            DomainUpDown dud = (DomainUpDown)sender;
            lblSampleText.ForeColor = Color.FromName(dud.Text);
        }

        // sự kiện khi đổi size trong NumericUpDown
        private void nudSize_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            lblSampleText.Font = new Font(lblSampleText.Font.FontFamily, (float)nud.Value);
        }

        // ===================================================================
        // Ex2: The MonthCalendar and DateTimePicker Controls
        // ===================================================================
        private void BuildTab2(TabPage tab)
        {
            Label lblHint = new Label
            {
                Text = "Select the days you will travel:",
                Location = new Point(20, 15),
                AutoSize = true
            };

            mcalTravel = new MonthCalendar
            {
                Location = new Point(20, 40),
                MaxSelectionCount = 7
            };
            mcalTravel.DateChanged += mcalTravel_DateChanged;

            Label lblStartCaption = new Label { Text = "StartDate:", Location = new Point(280, 40), AutoSize = true };
            lblStartDate = new Label { Location = new Point(280, 60), AutoSize = true, Text = "" };
            Label lblEndCaption = new Label { Text = "EndDate:", Location = new Point(280, 90), AutoSize = true };
            lblEndDate = new Label { Location = new Point(280, 110), AutoSize = true, Text = "" };

            Label lblLaunch = new Label
            {
                Text = "Product Launch Date:",
                Location = new Point(20, 250),
                AutoSize = true
            };
            dtpLaunch = new DateTimePicker
            {
                Location = new Point(20, 275),
                Width = 250,
                Format = DateTimePickerFormat.Long
            };

            radLongDate = new RadioButton { Text = "Long Date", Location = new Point(20, 310), Checked = true };
            radShortDate = new RadioButton { Text = "Short Date", Location = new Point(150, 310) };
            radLongDate.CheckedChanged += radDateFormat_CheckedChanged;
            radShortDate.CheckedChanged += radDateFormat_CheckedChanged;

            tab.Controls.AddRange(new Control[]
            {
                lblHint, mcalTravel, lblStartCaption, lblStartDate,
                lblEndCaption, lblEndDate, lblLaunch, dtpLaunch,
                radLongDate, radShortDate
            });

            // khởi tạo giá trị ban đầu
            lblStartDate.Text = mcalTravel.SelectionStart.ToShortDateString();
            lblEndDate.Text = mcalTravel.SelectionEnd.ToShortDateString();
        }

        private void mcalTravel_DateChanged(object sender, DateRangeEventArgs e)
        {
            lblStartDate.Text = e.Start.ToShortDateString();
            lblEndDate.Text = e.End.ToShortDateString();
        }

        private void radDateFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (radLongDate.Checked)
                dtpLaunch.Format = DateTimePickerFormat.Long;
            else
                dtpLaunch.Format = DateTimePickerFormat.Short;
        }

        // ===================================================================
        // Ex3: The Timer, TrackBar, and ProgressBar Controls
        // ===================================================================
        private void BuildTab3(TabPage tab)
        {
            Label lblProgress = new Label { Text = "Progress Indicator:", Location = new Point(20, 20), AutoSize = true };
            progressBar1 = new ProgressBar
            {
                Location = new Point(20, 45),
                Width = 400,
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };

            lblPercent = new Label { Text = "Percentage Complete: 0%", Location = new Point(20, 80), AutoSize = true };

            Label lblTrack = new Label
            {
                Text = "Slide the TrackBar to control processing speed:",
                Location = new Point(20, 130),
                AutoSize = true
            };
            trackBar1 = new TrackBar
            {
                Location = new Point(20, 155),
                Width = 400,
                Minimum = 1,
                Maximum = 10,
                Value = 5
            };
            trackBar1.ValueChanged += trackBar1_ValueChanged;

            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1000 / trackBar1.Value; // tốc độ ban đầu theo TrackBar
            timer1.Tick += timer1_Tick;
            timer1.Enabled = true;

            tab.Controls.AddRange(new Control[]
            {
                lblProgress, progressBar1, lblPercent, lblTrack, trackBar1
            });
        }

        // mỗi lần Tick tăng ProgressBar lên 1, hết 100 thì quay lại 0
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value >= progressBar1.Maximum)
                progressBar1.Value = progressBar1.Minimum;
            else
                progressBar1.Value++;

            lblPercent.Text = string.Format("Percentage Complete: {0}%", progressBar1.Value);
        }

        // kéo TrackBar để đổi tốc độ (Interval) của Timer
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            // giá trị TrackBar càng lớn thì chạy càng nhanh (Interval càng nhỏ)
            timer1.Interval = Math.Max(10, 1000 / trackBar1.Value);
        }

        // ===================================================================
        // Ex4: Using TabControl - demo tạo MessageBox theo lựa chọn của người dùng
        // ===================================================================
        private void BuildTab4(TabPage tab)
        {
            tabControlDemo = new TabControl { Location = new Point(20, 20), Width = 500, Height = 350 };

            TabPage pgMessage = new TabPage("Message");
            TabPage pgButtons = new TabPage("Buttons");
            TabPage pgIcon = new TabPage("Icon");
            tabControlDemo.TabPages.AddRange(new TabPage[] { pgMessage, pgButtons, pgIcon });

            // --- Tab Message ---
            Label lblMsg = new Label { Text = "Enter the message to display:", Location = new Point(20, 20), AutoSize = true };
            txtMessage = new TextBox { Location = new Point(20, 45), Width = 400, Height = 60, Multiline = true, Text = "Sample Message" };
            Label lblCap = new Label { Text = "Enter a Caption for MessageBox:", Location = new Point(20, 120), AutoSize = true };
            txtCaption = new TextBox { Location = new Point(20, 145), Width = 400, Text = "TabControl Demo" };
            pgMessage.Controls.AddRange(new Control[] { lblMsg, txtMessage, lblCap, txtCaption });

            // --- Tab Buttons ---
            string[] buttonNames = { "OK", "OK and Cancel", "Retry and Cancel", "Yes and No", "Yes, No and Cancel" };
            radButtons = new RadioButton[buttonNames.Length];
            for (int i = 0; i < buttonNames.Length; i++)
            {
                radButtons[i] = new RadioButton
                {
                    Text = buttonNames[i],
                    Location = new Point(20, 20 + i * 30),
                    AutoSize = true
                };
                pgButtons.Controls.Add(radButtons[i]);
            }
            radButtons[0].Checked = true;

            // --- Tab Icon ---
            string[] iconNames = { "Error", "Information", "None", "Question", "Warning" };
            radIcons = new RadioButton[iconNames.Length];
            for (int i = 0; i < iconNames.Length; i++)
            {
                radIcons[i] = new RadioButton
                {
                    Text = iconNames[i],
                    Location = new Point(20, 20 + i * 30),
                    AutoSize = true
                };
                pgIcon.Controls.Add(radIcons[i]);
            }
            radIcons[1].Checked = true;

            // --- Nút Show! ---
            Button btnShow = new Button { Text = "Show!", Location = new Point(200, 385), Width = 100 };
            btnShow.Click += btnShow_Click;

            // --- Tab Layout ---
            GroupBox grpLayout = new GroupBox { Text = "Tab Layout", Location = new Point(20, 420), Width = 500, Height = 60 };
            string[] layoutNames = { "Left", "Right", "Top", "Bottom" };
            radTabLayout = new RadioButton[layoutNames.Length];
            for (int i = 0; i < layoutNames.Length; i++)
            {
                radTabLayout[i] = new RadioButton
                {
                    Text = layoutNames[i],
                    Location = new Point(20 + i * 110, 25),
                    AutoSize = true
                };
                radTabLayout[i].CheckedChanged += radTabLayout_CheckedChanged;
                grpLayout.Controls.Add(radTabLayout[i]);
            }
            radTabLayout[2].Checked = true; // Top mặc định

            tab.Controls.AddRange(new Control[] { tabControlDemo, btnShow, grpLayout });
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mbButtons = MessageBoxButtons.OK;
            if (radButtons[1].Checked) mbButtons = MessageBoxButtons.OKCancel;
            else if (radButtons[2].Checked) mbButtons = MessageBoxButtons.RetryCancel;
            else if (radButtons[3].Checked) mbButtons = MessageBoxButtons.YesNo;
            else if (radButtons[4].Checked) mbButtons = MessageBoxButtons.YesNoCancel;

            MessageBoxIcon mbIcon = MessageBoxIcon.Information;
            if (radIcons[0].Checked) mbIcon = MessageBoxIcon.Error;
            else if (radIcons[2].Checked) mbIcon = MessageBoxIcon.None;
            else if (radIcons[3].Checked) mbIcon = MessageBoxIcon.Question;
            else if (radIcons[4].Checked) mbIcon = MessageBoxIcon.Warning;

            MessageBox.Show(txtMessage.Text, txtCaption.Text, mbButtons, mbIcon);
        }

        private void radTabLayout_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rad = (RadioButton)sender;
            if (!rad.Checked) return;

            switch (rad.Text)
            {
                case "Left":
                    tabControlDemo.Alignment = TabAlignment.Left;
                    break;
                case "Right":
                    tabControlDemo.Alignment = TabAlignment.Right;
                    break;
                case "Top":
                    tabControlDemo.Alignment = TabAlignment.Top;
                    break;
                case "Bottom":
                    tabControlDemo.Alignment = TabAlignment.Bottom;
                    break;
            }
        }
    }
}