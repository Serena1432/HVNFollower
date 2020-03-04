namespace HVNFollower
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mởRộngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.danhSáchChủThớtĐãTheoDõiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thôngBáoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thoátHVNFollowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nhập Link User của chủ thớt (https://hentaivn.net/user-xxxxx):";
            this.toolTip1.SetToolTip(this.label1, "Nhấn vào dòng chữ để lấy link mẫu.");
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(423, 22);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(269, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "Thêm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(17, 92);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(260, 18);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Chạy HVN Follower khi khởi động Windows";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 25);
            this.button2.TabIndex = 4;
            this.button2.Text = "Đã theo dõi";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "HVN Follower đã được ẩn vào thanh tác vụ.";
            this.notifyIcon1.BalloonTipTitle = "Thông báo";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "HVN Follower";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mởRộngToolStripMenuItem,
            this.danhSáchChủThớtĐãTheoDõiToolStripMenuItem,
            this.thôngBáoToolStripMenuItem,
            this.thoátHVNFollowerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            this.contextMenuStrip1.Text = "Mở rộng";
            // 
            // mởRộngToolStripMenuItem
            // 
            this.mởRộngToolStripMenuItem.Name = "mởRộngToolStripMenuItem";
            this.mởRộngToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.mởRộngToolStripMenuItem.Text = "Mở rộng";
            this.mởRộngToolStripMenuItem.Click += new System.EventHandler(this.mởRộngToolStripMenuItem_Click);
            // 
            // danhSáchChủThớtĐãTheoDõiToolStripMenuItem
            // 
            this.danhSáchChủThớtĐãTheoDõiToolStripMenuItem.Name = "danhSáchChủThớtĐãTheoDõiToolStripMenuItem";
            this.danhSáchChủThớtĐãTheoDõiToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.danhSáchChủThớtĐãTheoDõiToolStripMenuItem.Text = "Đã theo dõi";
            this.danhSáchChủThớtĐãTheoDõiToolStripMenuItem.Click += new System.EventHandler(this.button2_Click);
            // 
            // thôngBáoToolStripMenuItem
            // 
            this.thôngBáoToolStripMenuItem.Name = "thôngBáoToolStripMenuItem";
            this.thôngBáoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.thôngBáoToolStripMenuItem.Text = "Thông báo (0)";
            this.thôngBáoToolStripMenuItem.Click += new System.EventHandler(this.button4_Click);
            // 
            // thoátHVNFollowerToolStripMenuItem
            // 
            this.thoátHVNFollowerToolStripMenuItem.Name = "thoátHVNFollowerToolStripMenuItem";
            this.thoátHVNFollowerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.thoátHVNFollowerToolStripMenuItem.Text = "Thoát HVN Follower";
            this.thoátHVNFollowerToolStripMenuItem.Click += new System.EventHandler(this.thoátHVNFollowerToolStripMenuItem_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(414, 116);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(23, 22);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(23, 22);
            this.webBrowser1.TabIndex = 6;
            this.webBrowser1.Visible = false;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.BrowserDocumentCompleted);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 151);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(451, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Font = new System.Drawing.Font("Tahoma", 9F);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(56, 17);
            this.status.Text = "Sẵn sàng";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(331, 116);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(106, 25);
            this.button3.TabIndex = 8;
            this.button3.Text = "Cài đặt";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 30000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // timer4
            // 
            this.timer4.Enabled = true;
            this.timer4.Interval = 8000;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(226, 116);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(98, 25);
            this.button4.TabIndex = 10;
            this.button4.Text = "Thông báo (0)";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Thêm Chủ thớt",
            "Thêm Tác giả",
            "Thêm Doujinshi",
            "Thêm Nhóm dịch"});
            this.comboBox1.Location = new System.Drawing.Point(117, 61);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(140, 22);
            this.comboBox1.TabIndex = 11;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 173);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HVN Follower v1.10 by LilShieru";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mởRộngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem danhSáchChủThớtĐãTheoDõiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thôngBáoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thoátHVNFollowerToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer timer4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox1;
        internal System.Windows.Forms.TextBox textBox1;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.ToolStripStatusLabel status;
    }
}

