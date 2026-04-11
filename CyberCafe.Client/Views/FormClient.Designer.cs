namespace CyberCafe.Client
{
    partial class FormClient
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
            pnlLogin = new Panel();
            pnlLoginContainer = new Panel();
            btnRetry = new Button();
            lblStatus = new Label();
            btnLogin = new Button();
            txtCode = new TextBox();
            lblWelcome = new Label();
            pnlSession = new Panel();
            lblTime = new Label();
            btnLogout = new Button();
            pnlLogin.SuspendLayout();
            pnlLoginContainer.SuspendLayout();
            pnlSession.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLogin
            // 
            pnlLogin.BackColor = Color.FromArgb(30, 30, 30);
            pnlLogin.Controls.Add(pnlLoginContainer);
            pnlLogin.Dock = DockStyle.Fill;
            pnlLogin.Location = new Point(0, 0);
            pnlLogin.Margin = new Padding(3, 4, 3, 4);
            pnlLogin.Name = "pnlLogin";
            pnlLogin.Size = new Size(914, 600);
            pnlLogin.TabIndex = 0;
            // 
            // pnlLoginContainer
            // 
            pnlLoginContainer.BackColor = Color.FromArgb(30, 30, 30);
            pnlLoginContainer.Controls.Add(btnRetry);
            pnlLoginContainer.Controls.Add(lblStatus);
            pnlLoginContainer.Controls.Add(btnLogin);
            pnlLoginContainer.Controls.Add(txtCode);
            pnlLoginContainer.Controls.Add(lblWelcome);
            pnlLoginContainer.Location = new Point(286, 200);
            pnlLoginContainer.Margin = new Padding(3, 4, 3, 4);
            pnlLoginContainer.Name = "pnlLoginContainer";
            pnlLoginContainer.Size = new Size(343, 342);
            pnlLoginContainer.TabIndex = 0;
            // 
            // btnRetry
            // 
            btnRetry.Anchor = AnchorStyles.None;
            btnRetry.BackColor = Color.FromArgb(70, 70, 70);
            btnRetry.Cursor = Cursors.Hand;
            btnRetry.FlatStyle = FlatStyle.Flat;
            btnRetry.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRetry.ForeColor = Color.White;
            btnRetry.Location = new Point(129, 271);
            btnRetry.Margin = new Padding(3, 4, 3, 4);
            btnRetry.Name = "btnRetry";
            btnRetry.Size = new Size(80, 37);
            btnRetry.TabIndex = 5;
            btnRetry.Text = "Retry";
            btnRetry.TextAlign = ContentAlignment.TopCenter;
            btnRetry.UseVisualStyleBackColor = false;
            btnRetry.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.Transparent;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.Silver;
            lblStatus.Location = new Point(29, 213);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(286, 40);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Waiting...";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(29, 133);
            btnLogin.Margin = new Padding(3, 4, 3, 4);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(286, 53);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "START SESSION";
            btnLogin.UseVisualStyleBackColor = false;
            // 
            // txtCode
            // 
            txtCode.BackColor = Color.FromArgb(50, 50, 50);
            txtCode.BorderStyle = BorderStyle.FixedSingle;
            txtCode.Font = new Font("Segoe UI", 14F);
            txtCode.ForeColor = Color.White;
            txtCode.Location = new Point(29, 67);
            txtCode.Margin = new Padding(3, 4, 3, 4);
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(285, 39);
            txtCode.TabIndex = 1;
            txtCode.Text = "Enter Code";
            txtCode.TextAlign = HorizontalAlignment.Center;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Location = new Point(34, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(293, 46);
            lblWelcome.TabIndex = 6;
            lblWelcome.Text = "Cyber Cafe Client";
            // 
            // pnlSession
            // 
            pnlSession.BackColor = Color.FromArgb(20, 20, 20);
            pnlSession.Controls.Add(lblTime);
            pnlSession.Controls.Add(btnLogout);
            pnlSession.Location = new Point(0, 0);
            pnlSession.Margin = new Padding(3, 4, 3, 4);
            pnlSession.Name = "pnlSession";
            pnlSession.Size = new Size(194, 60);
            pnlSession.TabIndex = 1;
            pnlSession.Visible = false;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTime.ForeColor = Color.LimeGreen;
            lblTime.Location = new Point(6, 8);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(88, 25);
            lblTime.TabIndex = 0;
            lblTime.Text = "00:00:00";
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.Crimson;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(91, 7);
            btnLogout.Margin = new Padding(3, 4, 3, 4);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(34, 29);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "X";
            btnLogout.UseVisualStyleBackColor = false;
            // 
            // FormClient
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(pnlSession);
            Controls.Add(pnlLogin);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormClient";
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
        private Button btnRetry;
    }
}