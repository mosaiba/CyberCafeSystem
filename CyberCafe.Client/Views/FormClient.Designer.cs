namespace CyberCafe.Client
{
    partial class FormClient
    {
        private System.ComponentModel.IContainer components = null;

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
            pnlLogin = new Panel();
            pnlLoginContainer = new Panel();
            lblWelcome = new Label();
            txtCode = new TextBox();
            btnLogin = new Button();
            lblStatus = new Label();
            pnlSession = new Panel();
            lblTime = new Label();
            btnLogout = new Button();
            pnlLogin.SuspendLayout();
            pnlSession.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLogin
            // 
            pnlLogin.BackColor = Color.FromArgb(30, 30, 30);
            pnlLogin.Controls.Add(pnlLoginContainer);
            pnlLogin.Dock = DockStyle.Fill;
            pnlLogin.Name = "pnlLogin";
            pnlLogin.Size = new Size(800, 450);
            pnlLogin.TabIndex = 0;
            // 
            // pnlLoginContainer
            // 
            pnlLoginContainer.Controls.Add(lblWelcome);
            pnlLoginContainer.Controls.Add(txtCode);
            pnlLoginContainer.Controls.Add(btnLogin);
            pnlLoginContainer.Controls.Add(lblStatus);
            pnlLoginContainer.Location = new Point(250, 150);
            pnlLoginContainer.Name = "pnlLoginContainer";
            pnlLoginContainer.Size = new Size(300, 200);
            pnlLoginContainer.TabIndex = 0;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Location = new Point(30, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(240, 37);
            lblWelcome.Text = "Cyber Cafe Client";
            // 
            // txtCode
            // 
            txtCode.BackColor = Color.FromArgb(50, 50, 50);
            txtCode.BorderStyle = BorderStyle.FixedSingle;
            txtCode.Font = new Font("Segoe UI", 14F);
            txtCode.ForeColor = Color.White;
            txtCode.Location = new Point(25, 50);
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(250, 32);
            txtCode.TabIndex = 0;
            txtCode.Text = "Enter Code";
            txtCode.TextAlign = HorizontalAlignment.Center;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(25, 100);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(250, 40);
            btnLogin.TabIndex = 1;
            btnLogin.Text = "START SESSION";
            btnLogin.UseVisualStyleBackColor = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = false; // === مهم: تعطيل التمدد التلقائي ===
            lblStatus.BackColor = Color.Transparent; // لجعله شفافاً وأنيقاً
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.Silver;
            lblStatus.Location = new Point(25, 160); // نفس بداية زر الدخول
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(250, 30); // نفس عرض الزر، وارتفاع مناسب لسطرين
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Waiting...";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter; // === مهم: توسيط النص داخل المساحة الثابتة ===
            // 
            // pnlSession (الشريط العلوي)
            // 
            pnlSession.BackColor = Color.FromArgb(20, 20, 20);
            pnlSession.Controls.Add(lblTime);
            pnlSession.Controls.Add(btnLogout);
            pnlSession.Location = new Point(0, 0);
            pnlSession.Name = "pnlSession";
            pnlSession.Size = new Size(170, 45);
            pnlSession.TabIndex = 1;
            pnlSession.Visible = false;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTime.ForeColor = Color.LimeGreen;
            lblTime.Location = new Point(5, 6);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(60, 20);
            lblTime.Text = "00:00:00";
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.Crimson;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(80, 5);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(30, 22);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "X";
            btnLogout.UseVisualStyleBackColor = false;
            // 
            // form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlSession);
            Controls.Add(pnlLogin);
            FormBorderStyle = FormBorderStyle.None;
            Name = "form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Client";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            pnlLogin.ResumeLayout(false);
            pnlLoginContainer.ResumeLayout(false);
            pnlLoginContainer.PerformLayout();
            pnlSession.ResumeLayout(false);
            pnlSession.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLogin;
        private Panel pnlLoginContainer;
        private Label lblWelcome;
        private TextBox txtCode;
        private Button btnLogin;
        private Label lblStatus;
        private Panel pnlSession;
        private Label lblTime;
        private Button btnLogout;
    }
}