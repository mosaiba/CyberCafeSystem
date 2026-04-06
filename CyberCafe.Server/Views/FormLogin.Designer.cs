namespace CyberCafe.Server.Views
{
    partial class FormLogin
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
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnExit = new Button();
            label1 = new Label();
            label2 = new Label();
            lblTitle = new Label();
            panelLine1 = new Panel();
            panelLine2 = new Panel();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.FromArgb(50, 50, 55);
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.ForeColor = Color.White;
            txtUsername.Location = new Point(103, 133);
            txtUsername.Margin = new Padding(3, 4, 3, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(229, 25);
            txtUsername.TabIndex = 0;
            txtUsername.TextAlign = HorizontalAlignment.Center;
            txtUsername.KeyDown += txtUsername_KeyDown;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(50, 50, 55);
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.ForeColor = Color.White;
            txtPassword.Location = new Point(103, 233);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(229, 25);
            txtPassword.TabIndex = 1;
            txtPassword.TextAlign = HorizontalAlignment.Center;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyDown += txtPassword_KeyDown;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(103, 303);
            btnLogin.Margin = new Padding(3, 4, 3, 4);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(229, 51);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "LOGIN";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(192, 0, 0);
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(103, 373);
            btnExit.Margin = new Padding(3, 4, 3, 4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(229, 47);
            btnExit.TabIndex = 3;
            btnExit.Text = "Turn Off The Server";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.ForeColor = Color.FromArgb(200, 200, 200);
            label1.Location = new Point(103, 109);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 4;
            label1.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.ForeColor = Color.FromArgb(200, 200, 200);
            label2.Location = new Point(103, 209);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 5;
            label2.Text = "Password";
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 40);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(434, 47);
            lblTitle.TabIndex = 10;
            lblTitle.Text = "CyberCafe Server";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelLine1
            // 
            panelLine1.BackColor = Color.FromArgb(0, 122, 204);
            panelLine1.Location = new Point(103, 171);
            panelLine1.Margin = new Padding(3, 4, 3, 4);
            panelLine1.Name = "panelLine1";
            panelLine1.Size = new Size(229, 3);
            panelLine1.TabIndex = 11;
            // 
            // panelLine2
            // 
            panelLine2.BackColor = Color.FromArgb(0, 122, 204);
            panelLine2.Location = new Point(103, 271);
            panelLine2.Margin = new Padding(3, 4, 3, 4);
            panelLine2.Name = "panelLine2";
            panelLine2.Size = new Size(229, 3);
            panelLine2.TabIndex = 12;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            CancelButton = btnExit;
            ClientSize = new Size(434, 453);
            Controls.Add(panelLine2);
            Controls.Add(panelLine1);
            Controls.Add(lblTitle);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnExit);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private Label label1;
        private Label label2;
        private Label lblTitle;
        private Panel panelLine1;
        private Panel panelLine2;
    }
}