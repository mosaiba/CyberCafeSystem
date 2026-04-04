namespace CyberCafe.Server.Views
{
    partial class FormReports
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnPrintDaily = new Button();
            lblDailyResult = new Label();
            btnShowDaily = new Button();
            dtpDaily = new DateTimePicker();
            tabPage2 = new TabPage();
            dgvEmpReport = new DataGridView();
            btnPrintEmp = new Button();
            btnShowEmp = new Button();
            label3 = new Label();
            label2 = new Label();
            dtpTo = new DateTimePicker();
            dtpFrom = new DateTimePicker();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmpReport).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1052, 667);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.FromArgb(28, 28, 28);
            tabPage1.Controls.Add(btnPrintDaily);
            tabPage1.Controls.Add(lblDailyResult);
            tabPage1.Controls.Add(btnShowDaily);
            tabPage1.Controls.Add(dtpDaily);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Margin = new Padding(3, 4, 3, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(23, 27, 23, 27);
            tabPage1.Size = new Size(1044, 634);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Daily Sales";
            // 
            // btnPrintDaily
            // 
            btnPrintDaily.BackColor = Color.FromArgb(0, 150, 136);
            btnPrintDaily.FlatAppearance.BorderSize = 0;
            btnPrintDaily.FlatStyle = FlatStyle.Flat;
            btnPrintDaily.Font = new Font("Segoe UI Semibold", 10F);
            btnPrintDaily.ForeColor = Color.White;
            btnPrintDaily.Location = new Point(171, 93);
            btnPrintDaily.Margin = new Padding(3, 4, 3, 4);
            btnPrintDaily.Name = "btnPrintDaily";
            btnPrintDaily.Size = new Size(137, 47);
            btnPrintDaily.TabIndex = 3;
            btnPrintDaily.Text = "Print";
            btnPrintDaily.UseVisualStyleBackColor = false;
            btnPrintDaily.Click += btnPrintDaily_Click;
            // 
            // lblDailyResult
            // 
            lblDailyResult.AutoSize = true;
            lblDailyResult.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDailyResult.ForeColor = Color.White;
            lblDailyResult.Location = new Point(23, 173);
            lblDailyResult.Name = "lblDailyResult";
            lblDailyResult.Size = new Size(111, 28);
            lblDailyResult.TabIndex = 2;
            lblDailyResult.Text = "Total: 0.00";
            // 
            // btnShowDaily
            // 
            btnShowDaily.BackColor = Color.FromArgb(0, 122, 204);
            btnShowDaily.FlatAppearance.BorderSize = 0;
            btnShowDaily.FlatStyle = FlatStyle.Flat;
            btnShowDaily.Font = new Font("Segoe UI Semibold", 10F);
            btnShowDaily.ForeColor = Color.White;
            btnShowDaily.Location = new Point(23, 93);
            btnShowDaily.Margin = new Padding(3, 4, 3, 4);
            btnShowDaily.Name = "btnShowDaily";
            btnShowDaily.Size = new Size(137, 47);
            btnShowDaily.TabIndex = 1;
            btnShowDaily.Text = "Show Report";
            btnShowDaily.UseVisualStyleBackColor = false;
            btnShowDaily.Click += btnShowDaily_Click;
            // 
            // dtpDaily
            // 
            dtpDaily.BackColor = Color.FromArgb(45, 45, 48);
            dtpDaily.Font = new Font("Segoe UI", 10F);
            dtpDaily.ForeColor = Color.White;
            dtpDaily.Location = new Point(23, 27);
            dtpDaily.Margin = new Padding(3, 4, 3, 4);
            dtpDaily.Name = "dtpDaily";
            dtpDaily.Size = new Size(285, 30);
            dtpDaily.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.FromArgb(28, 28, 28);
            tabPage2.Controls.Add(dgvEmpReport);
            tabPage2.Controls.Add(btnPrintEmp);
            tabPage2.Controls.Add(btnShowEmp);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(dtpTo);
            tabPage2.Controls.Add(dtpFrom);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(3, 4, 3, 4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(23, 27, 23, 27);
            tabPage2.Size = new Size(1044, 634);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Employee Performance";
            // 
            // dgvEmpReport
            // 
            dgvEmpReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmpReport.BackgroundColor = Color.WhiteSmoke;
            dgvEmpReport.BorderStyle = BorderStyle.None;
            dgvEmpReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEmpReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvEmpReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvEmpReport.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvEmpReport.DefaultCellStyle = dataGridViewCellStyle2;
            dgvEmpReport.EnableHeadersVisualStyles = false;
            dgvEmpReport.GridColor = Color.LightGray;
            dgvEmpReport.Location = new Point(294, 49);
            dgvEmpReport.Margin = new Padding(3, 4, 3, 4);
            dgvEmpReport.Name = "dgvEmpReport";
            dgvEmpReport.RowHeadersVisible = false;
            dgvEmpReport.RowHeadersWidth = 51;
            dgvEmpReport.RowTemplate.Height = 28;
            dgvEmpReport.Size = new Size(686, 533);
            dgvEmpReport.TabIndex = 5;
            // 
            // btnPrintEmp
            // 
            btnPrintEmp.BackColor = Color.FromArgb(0, 150, 136);
            btnPrintEmp.FlatAppearance.BorderSize = 0;
            btnPrintEmp.FlatStyle = FlatStyle.Flat;
            btnPrintEmp.Font = new Font("Segoe UI Semibold", 10F);
            btnPrintEmp.ForeColor = Color.White;
            btnPrintEmp.Location = new Point(29, 535);
            btnPrintEmp.Margin = new Padding(3, 4, 3, 4);
            btnPrintEmp.Name = "btnPrintEmp";
            btnPrintEmp.Size = new Size(229, 47);
            btnPrintEmp.TabIndex = 6;
            btnPrintEmp.Text = "Print";
            btnPrintEmp.UseVisualStyleBackColor = false;
            btnPrintEmp.Click += btnPrintEmp_Click;
            // 
            // btnShowEmp
            // 
            btnShowEmp.BackColor = Color.FromArgb(0, 122, 204);
            btnShowEmp.FlatAppearance.BorderSize = 0;
            btnShowEmp.FlatStyle = FlatStyle.Flat;
            btnShowEmp.Font = new Font("Segoe UI Semibold", 10F);
            btnShowEmp.ForeColor = Color.White;
            btnShowEmp.Location = new Point(26, 253);
            btnShowEmp.Margin = new Padding(3, 4, 3, 4);
            btnShowEmp.Name = "btnShowEmp";
            btnShowEmp.Size = new Size(229, 47);
            btnShowEmp.TabIndex = 4;
            btnShowEmp.Text = "Show Report";
            btnShowEmp.UseVisualStyleBackColor = false;
            btnShowEmp.Click += btnShowEmp_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F);
            label3.ForeColor = Color.Silver;
            label3.Location = new Point(29, 140);
            label3.Name = "label3";
            label3.Size = new Size(25, 20);
            label3.TabIndex = 3;
            label3.Text = "To";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.ForeColor = Color.Silver;
            label2.Location = new Point(26, 49);
            label2.Name = "label2";
            label2.Size = new Size(43, 20);
            label2.TabIndex = 2;
            label2.Text = "From";
            // 
            // dtpTo
            // 
            dtpTo.BackColor = Color.FromArgb(45, 45, 48);
            dtpTo.Font = new Font("Segoe UI", 10F);
            dtpTo.ForeColor = Color.White;
            dtpTo.Location = new Point(26, 164);
            dtpTo.Margin = new Padding(3, 4, 3, 4);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(228, 30);
            dtpTo.TabIndex = 1;
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = Color.FromArgb(45, 45, 48);
            dtpFrom.Font = new Font("Segoe UI", 10F);
            dtpFrom.ForeColor = Color.White;
            dtpFrom.Location = new Point(26, 79);
            dtpFrom.Margin = new Padding(3, 4, 3, 4);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(228, 30);
            dtpFrom.TabIndex = 0;
            // 
            // FormReports
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 28, 28);
            ClientSize = new Size(1052, 667);
            Controls.Add(tabControl1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormReports";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CyberCafe - Reports";
            Load += FormReports_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmpReport).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label lblDailyResult;
        private Button btnShowDaily;
        private DateTimePicker dtpDaily;
        private DateTimePicker dtpTo;
        private DateTimePicker dtpFrom;
        private DataGridView dgvEmpReport;
        private Button btnShowEmp;
        private Label label3;
        private Label label2;
        private Button btnPrintDaily;
        private Button btnPrintEmp;
    }
}