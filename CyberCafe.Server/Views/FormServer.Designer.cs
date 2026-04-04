namespace CyberCafe.Server
{
    partial class FormServer
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            dgvDevices = new DataGridView();
            dgvLogs = new DataGridView();
            btnManageVouchers = new Button();
            txtSellCode = new TextBox();
            btnSellVoucher = new Button();
            label1 = new Label();
            btnManageEmployees = new Button();
            lblUserStatus = new Label();
            Logout = new Button();
            btnShutdown = new Button();
            btnRestart = new Button();
            btnForceLogout = new Button();
            btnSendMessage = new Button();
            btnReports = new Button();
            panelTitleBar = new Panel();
            lblFormTitle = new Label();
            btnMinimize = new Button();
            btnClose = new Button();
            panelSidebar = new Panel();
            panelMain = new Panel();
            panelControls = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvDevices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).BeginInit();
            panelTitleBar.SuspendLayout();
            panelSidebar.SuspendLayout();
            panelMain.SuspendLayout();
            panelControls.SuspendLayout();
            SuspendLayout();
            // 
            // dgvDevices
            // 
            dgvDevices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDevices.BackgroundColor = Color.WhiteSmoke;
            dgvDevices.BorderStyle = BorderStyle.None;
            dgvDevices.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDevices.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDevices.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDevices.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDevices.EnableHeadersVisualStyles = false;
            dgvDevices.GridColor = Color.LightGray;
            dgvDevices.Location = new Point(23, 18);
            dgvDevices.Margin = new Padding(3, 4, 3, 4);
            dgvDevices.Name = "dgvDevices";
            dgvDevices.ReadOnly = true;
            dgvDevices.RowHeadersVisible = false;
            dgvDevices.RowHeadersWidth = 51;
            dgvDevices.RowTemplate.Height = 30;
            dgvDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDevices.Size = new Size(953, 868);
            dgvDevices.TabIndex = 0;
            dgvDevices.CellDoubleClick += DgvDevices_CellDoubleClick;
            // 
            // dgvLogs
            // 
            dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLogs.BackgroundColor = Color.WhiteSmoke;
            dgvLogs.BorderStyle = BorderStyle.None;
            dgvLogs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLogs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle3.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dgvLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvLogs.ColumnHeadersHeight = 35;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvLogs.DefaultCellStyle = dataGridViewCellStyle4;
            dgvLogs.EnableHeadersVisualStyles = false;
            dgvLogs.GridColor = Color.LightGray;
            dgvLogs.Location = new Point(996, 18);
            dgvLogs.Margin = new Padding(3, 4, 3, 4);
            dgvLogs.Name = "dgvLogs";
            dgvLogs.ReadOnly = true;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.RowHeadersWidth = 51;
            dgvLogs.RowTemplate.Height = 28;
            dgvLogs.Size = new Size(558, 868);
            dgvLogs.TabIndex = 1;
            // 
            // btnManageVouchers
            // 
            btnManageVouchers.BackColor = Color.FromArgb(45, 45, 48);
            btnManageVouchers.FlatAppearance.BorderSize = 0;
            btnManageVouchers.FlatStyle = FlatStyle.Flat;
            btnManageVouchers.Font = new Font("Segoe UI Semibold", 10F);
            btnManageVouchers.ForeColor = Color.White;
            btnManageVouchers.Location = new Point(17, 27);
            btnManageVouchers.Margin = new Padding(3, 4, 3, 4);
            btnManageVouchers.Name = "btnManageVouchers";
            btnManageVouchers.Size = new Size(286, 53);
            btnManageVouchers.TabIndex = 2;
            btnManageVouchers.Text = "Manage Vouchers";
            btnManageVouchers.TextAlign = ContentAlignment.MiddleLeft;
            btnManageVouchers.UseVisualStyleBackColor = false;
            btnManageVouchers.Click += btnManageVouchers_Click;
            // 
            // txtSellCode
            // 
            txtSellCode.BackColor = Color.FromArgb(45, 45, 48);
            txtSellCode.BorderStyle = BorderStyle.FixedSingle;
            txtSellCode.Font = new Font("Segoe UI", 11F);
            txtSellCode.ForeColor = Color.White;
            txtSellCode.Location = new Point(17, 293);
            txtSellCode.Margin = new Padding(3, 4, 3, 4);
            txtSellCode.Name = "txtSellCode";
            txtSellCode.Size = new Size(285, 32);
            txtSellCode.TabIndex = 3;
            // 
            // btnSellVoucher
            // 
            btnSellVoucher.BackColor = Color.FromArgb(0, 122, 204);
            btnSellVoucher.FlatAppearance.BorderSize = 0;
            btnSellVoucher.FlatStyle = FlatStyle.Flat;
            btnSellVoucher.Font = new Font("Segoe UI Semibold", 10F);
            btnSellVoucher.ForeColor = Color.White;
            btnSellVoucher.Location = new Point(17, 347);
            btnSellVoucher.Margin = new Padding(3, 4, 3, 4);
            btnSellVoucher.Name = "btnSellVoucher";
            btnSellVoucher.Size = new Size(286, 47);
            btnSellVoucher.TabIndex = 4;
            btnSellVoucher.Text = "Sell Voucher";
            btnSellVoucher.UseVisualStyleBackColor = false;
            btnSellVoucher.Click += btnSellVoucher_Click;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(17, 253);
            label1.Name = "label1";
            label1.Size = new Size(114, 33);
            label1.TabIndex = 5;
            label1.Text = "Quick Sell";
            // 
            // btnManageEmployees
            // 
            btnManageEmployees.BackColor = Color.FromArgb(45, 45, 48);
            btnManageEmployees.FlatAppearance.BorderSize = 0;
            btnManageEmployees.FlatStyle = FlatStyle.Flat;
            btnManageEmployees.Font = new Font("Segoe UI Semibold", 10F);
            btnManageEmployees.ForeColor = Color.White;
            btnManageEmployees.Location = new Point(17, 93);
            btnManageEmployees.Margin = new Padding(3, 4, 3, 4);
            btnManageEmployees.Name = "btnManageEmployees";
            btnManageEmployees.Size = new Size(286, 53);
            btnManageEmployees.TabIndex = 6;
            btnManageEmployees.Text = "Manage Employees";
            btnManageEmployees.TextAlign = ContentAlignment.MiddleLeft;
            btnManageEmployees.UseVisualStyleBackColor = false;
            btnManageEmployees.Click += btnManageEmployees_Click;
            // 
            // lblUserStatus
            // 
            lblUserStatus.Anchor = AnchorStyles.Right;
            lblUserStatus.Font = new Font("Segoe UI", 10F);
            lblUserStatus.ForeColor = Color.Silver;
            lblUserStatus.Location = new Point(1348, 20);
            lblUserStatus.Name = "lblUserStatus";
            lblUserStatus.Size = new Size(343, 27);
            lblUserStatus.TabIndex = 3;
            lblUserStatus.Text = "Logged in as: Admin";
            lblUserStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Logout
            // 
            Logout.BackColor = Color.FromArgb(209, 71, 82);
            Logout.Dock = DockStyle.Bottom;
            Logout.FlatAppearance.BorderSize = 0;
            Logout.FlatStyle = FlatStyle.Flat;
            Logout.Font = new Font("Segoe UI Semibold", 10F);
            Logout.ForeColor = Color.White;
            Logout.Location = new Point(11, 942);
            Logout.Margin = new Padding(3, 4, 3, 4);
            Logout.Name = "Logout";
            Logout.Size = new Size(298, 80);
            Logout.TabIndex = 8;
            Logout.Text = "Logout";
            Logout.UseVisualStyleBackColor = false;
            Logout.Click += btnLogout_Click;
            // 
            // btnShutdown
            // 
            btnShutdown.BackColor = Color.FromArgb(209, 71, 82);
            btnShutdown.FlatAppearance.BorderSize = 0;
            btnShutdown.FlatStyle = FlatStyle.Flat;
            btnShutdown.Font = new Font("Segoe UI Semibold", 9F);
            btnShutdown.ForeColor = Color.White;
            btnShutdown.Location = new Point(571, 27);
            btnShutdown.Margin = new Padding(3, 4, 3, 4);
            btnShutdown.Name = "btnShutdown";
            btnShutdown.Size = new Size(160, 53);
            btnShutdown.TabIndex = 9;
            btnShutdown.Text = "Shutdown";
            btnShutdown.UseVisualStyleBackColor = false;
            btnShutdown.Click += btnShutdownClient_Click;
            // 
            // btnRestart
            // 
            btnRestart.BackColor = Color.FromArgb(0, 122, 204);
            btnRestart.FlatAppearance.BorderSize = 0;
            btnRestart.FlatStyle = FlatStyle.Flat;
            btnRestart.Font = new Font("Segoe UI Semibold", 9F);
            btnRestart.ForeColor = Color.White;
            btnRestart.Location = new Point(389, 27);
            btnRestart.Margin = new Padding(3, 4, 3, 4);
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new Size(160, 53);
            btnRestart.TabIndex = 10;
            btnRestart.Text = "Restart";
            btnRestart.UseVisualStyleBackColor = false;
            btnRestart.Click += btnRestartClient_Click;
            // 
            // btnForceLogout
            // 
            btnForceLogout.BackColor = Color.FromArgb(0, 150, 136);
            btnForceLogout.FlatAppearance.BorderSize = 0;
            btnForceLogout.FlatStyle = FlatStyle.Flat;
            btnForceLogout.Font = new Font("Segoe UI Semibold", 9F);
            btnForceLogout.ForeColor = Color.White;
            btnForceLogout.Location = new Point(206, 27);
            btnForceLogout.Margin = new Padding(3, 4, 3, 4);
            btnForceLogout.Name = "btnForceLogout";
            btnForceLogout.Size = new Size(160, 53);
            btnForceLogout.TabIndex = 11;
            btnForceLogout.Text = "Force Logout";
            btnForceLogout.UseVisualStyleBackColor = false;
            btnForceLogout.Click += btnForceLogout_Click;
            // 
            // btnSendMessage
            // 
            btnSendMessage.BackColor = Color.FromArgb(45, 45, 48);
            btnSendMessage.FlatAppearance.BorderSize = 0;
            btnSendMessage.FlatStyle = FlatStyle.Flat;
            btnSendMessage.Font = new Font("Segoe UI Semibold", 9F);
            btnSendMessage.ForeColor = Color.White;
            btnSendMessage.Location = new Point(23, 27);
            btnSendMessage.Margin = new Padding(3, 4, 3, 4);
            btnSendMessage.Name = "btnSendMessage";
            btnSendMessage.Size = new Size(160, 53);
            btnSendMessage.TabIndex = 12;
            btnSendMessage.Text = "Send Message";
            btnSendMessage.UseVisualStyleBackColor = false;
            btnSendMessage.Click += btnSendMessage_Click;
            // 
            // btnReports
            // 
            btnReports.BackColor = Color.FromArgb(45, 45, 48);
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Segoe UI Semibold", 10F);
            btnReports.ForeColor = Color.White;
            btnReports.Location = new Point(17, 160);
            btnReports.Margin = new Padding(3, 4, 3, 4);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(286, 53);
            btnReports.TabIndex = 13;
            btnReports.Text = "Reports";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = false;
            btnReports.Click += btnReports_Click;
            // 
            // panelTitleBar
            // 
            panelTitleBar.BackColor = Color.FromArgb(20, 20, 20);
            panelTitleBar.Controls.Add(lblFormTitle);
            panelTitleBar.Controls.Add(lblUserStatus);
            panelTitleBar.Controls.Add(btnMinimize);
            panelTitleBar.Controls.Add(btnClose);
            panelTitleBar.Dock = DockStyle.Top;
            panelTitleBar.Location = new Point(0, 0);
            panelTitleBar.Margin = new Padding(3, 4, 3, 4);
            panelTitleBar.Name = "panelTitleBar";
            panelTitleBar.Size = new Size(1942, 67);
            panelTitleBar.TabIndex = 100;
            // 
            // lblFormTitle
            // 
            lblFormTitle.AutoSize = true;
            lblFormTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Location = new Point(17, 17);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Size = new Size(175, 28);
            lblFormTitle.TabIndex = 0;
            lblFormTitle.Text = "CyberCafe Server";
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.Right;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 45, 48);
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.Font = new Font("Segoe UI", 14F);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.Location = new Point(1828, 7);
            btnMinimize.Margin = new Padding(3, 4, 3, 4);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(46, 53);
            btnMinimize.TabIndex = 1;
            btnMinimize.Text = "—";
            btnMinimize.UseVisualStyleBackColor = true;
            btnMinimize.Click += btnMinimize_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 17, 35);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 14F);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1874, 7);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(46, 53);
            btnClose.TabIndex = 2;
            btnClose.Text = "✕";
            btnClose.Click += btnClose_Click;
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(28, 28, 28);
            panelSidebar.Controls.Add(btnReports);
            panelSidebar.Controls.Add(btnManageEmployees);
            panelSidebar.Controls.Add(btnManageVouchers);
            panelSidebar.Controls.Add(label1);
            panelSidebar.Controls.Add(txtSellCode);
            panelSidebar.Controls.Add(btnSellVoucher);
            panelSidebar.Controls.Add(Logout);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 67);
            panelSidebar.Margin = new Padding(3, 4, 3, 4);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Padding = new Padding(11, 27, 11, 13);
            panelSidebar.Size = new Size(320, 1035);
            panelSidebar.TabIndex = 101;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(28, 28, 28);
            panelMain.Controls.Add(dgvLogs);
            panelMain.Controls.Add(dgvDevices);
            panelMain.Controls.Add(panelControls);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(320, 67);
            panelMain.Margin = new Padding(3, 4, 3, 4);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1622, 1035);
            panelMain.TabIndex = 102;
            // 
            // panelControls
            // 
            panelControls.BackColor = Color.FromArgb(28, 28, 28);
            panelControls.Controls.Add(btnSendMessage);
            panelControls.Controls.Add(btnForceLogout);
            panelControls.Controls.Add(btnRestart);
            panelControls.Controls.Add(btnShutdown);
            panelControls.Dock = DockStyle.Bottom;
            panelControls.Location = new Point(0, 928);
            panelControls.Margin = new Padding(3, 4, 3, 4);
            panelControls.Name = "panelControls";
            panelControls.Padding = new Padding(11, 20, 11, 20);
            panelControls.Size = new Size(1622, 107);
            panelControls.TabIndex = 103;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 28, 28);
            ClientSize = new Size(1942, 1102);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            Controls.Add(panelTitleBar);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "CyberCafe Server";
            WindowState = FormWindowState.Maximized;
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDevices).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).EndInit();
            panelTitleBar.ResumeLayout(false);
            panelTitleBar.PerformLayout();
            panelSidebar.ResumeLayout(false);
            panelSidebar.PerformLayout();
            panelMain.ResumeLayout(false);
            panelControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvDevices;
        private DataGridView dgvLogs;
        private Button btnManageVouchers;
        private TextBox txtSellCode;
        private Button btnSellVoucher;
        private Label label1;
        private Button btnManageEmployees;
        private Label lblUserStatus;
        private Button Logout;
        private Button btnShutdown;
        private Button btnRestart;
        private Button btnForceLogout;
        private Button btnSendMessage;
        private Button btnReports;

        private Panel panelTitleBar;
        private Label lblFormTitle;
        private Button btnMinimize;
        private Button btnClose;
        private Panel panelSidebar;
        private Panel panelMain;
        private Panel panelControls;
    }
}