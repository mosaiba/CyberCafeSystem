namespace CyberCafe.Server.Views
{
    partial class FormVouchers
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
            numCount = new NumericUpDown();
            numMinutes = new NumericUpDown();
            btnGenerate = new Button();
            dgvVouchers = new DataGridView();
            btnPrint = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            numValidityDays = new NumericUpDown();
            numPrice = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            count = new Label();
            panel1 = new Panel();
            label8 = new Label();
            label7 = new Label();
            numPriceTo = new NumericUpDown();
            numPriceFrom = new NumericUpDown();
            btnSearch = new Button();
            dtpTo = new DateTimePicker();
            label6 = new Label();
            dtpFrom = new DateTimePicker();
            label5 = new Label();
            cboStatus = new ComboBox();
            label4 = new Label();
            btnDelete = new Button();
            panelRight = new Panel();
            lblHeader = new Label();
            ((System.ComponentModel.ISupportInitialize)numCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinutes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVouchers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numValidityDays).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPriceTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPriceFrom).BeginInit();
            panelRight.SuspendLayout();
            SuspendLayout();
            // 
            // numCount
            // 
            numCount.BackColor = Color.FromArgb(45, 45, 48);
            numCount.ForeColor = Color.White;
            numCount.Location = new Point(23, 127);
            numCount.Margin = new Padding(3, 4, 3, 4);
            numCount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numCount.Name = "numCount";
            numCount.Size = new Size(297, 27);
            numCount.TabIndex = 40;
            // 
            // numMinutes
            // 
            numMinutes.BackColor = Color.FromArgb(45, 45, 48);
            numMinutes.ForeColor = Color.White;
            numMinutes.Location = new Point(23, 407);
            numMinutes.Margin = new Padding(3, 4, 3, 4);
            numMinutes.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numMinutes.Name = "numMinutes";
            numMinutes.Size = new Size(297, 27);
            numMinutes.TabIndex = 34;
            // 
            // btnGenerate
            // 
            btnGenerate.BackColor = Color.FromArgb(0, 122, 204);
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Font = new Font("Segoe UI Semibold", 10F);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.Location = new Point(23, 473);
            btnGenerate.Margin = new Padding(3, 4, 3, 4);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(297, 51);
            btnGenerate.TabIndex = 33;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // dgvVouchers
            // 
            dgvVouchers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVouchers.BackgroundColor = Color.WhiteSmoke;
            dgvVouchers.BorderStyle = BorderStyle.None;
            dgvVouchers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVouchers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvVouchers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvVouchers.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvVouchers.DefaultCellStyle = dataGridViewCellStyle2;
            dgvVouchers.Dock = DockStyle.Fill;
            dgvVouchers.EnableHeadersVisualStyles = false;
            dgvVouchers.GridColor = Color.LightGray;
            dgvVouchers.Location = new Point(0, 160);
            dgvVouchers.Margin = new Padding(3, 4, 3, 4);
            dgvVouchers.Name = "dgvVouchers";
            dgvVouchers.RowHeadersVisible = false;
            dgvVouchers.RowHeadersWidth = 51;
            dgvVouchers.RowTemplate.Height = 28;
            dgvVouchers.Size = new Size(914, 620);
            dgvVouchers.TabIndex = 0;
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.FromArgb(45, 45, 48);
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Font = new Font("Segoe UI Semibold", 9F);
            btnPrint.ForeColor = Color.White;
            btnPrint.Location = new Point(23, 540);
            btnPrint.Margin = new Padding(3, 4, 3, 4);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(143, 47);
            btnPrint.TabIndex = 32;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            // 
            // numValidityDays
            // 
            numValidityDays.BackColor = Color.FromArgb(45, 45, 48);
            numValidityDays.ForeColor = Color.White;
            numValidityDays.Location = new Point(23, 220);
            numValidityDays.Margin = new Padding(3, 4, 3, 4);
            numValidityDays.Name = "numValidityDays";
            numValidityDays.Size = new Size(297, 27);
            numValidityDays.TabIndex = 38;
            // 
            // numPrice
            // 
            numPrice.BackColor = Color.FromArgb(45, 45, 48);
            numPrice.ForeColor = Color.White;
            numPrice.Location = new Point(23, 313);
            numPrice.Margin = new Padding(3, 4, 3, 4);
            numPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(297, 27);
            numPrice.TabIndex = 36;
            // 
            // label1
            // 
            label1.ForeColor = Color.Silver;
            label1.Location = new Point(23, 187);
            label1.Name = "label1";
            label1.Size = new Size(114, 31);
            label1.TabIndex = 39;
            label1.Text = "Validity (Days)";
            // 
            // label2
            // 
            label2.ForeColor = Color.Silver;
            label2.Location = new Point(23, 373);
            label2.Name = "label2";
            label2.Size = new Size(114, 31);
            label2.TabIndex = 35;
            label2.Text = "Minutes";
            // 
            // label3
            // 
            label3.ForeColor = Color.Silver;
            label3.Location = new Point(23, 280);
            label3.Name = "label3";
            label3.Size = new Size(114, 31);
            label3.TabIndex = 37;
            label3.Text = "Price";
            // 
            // count
            // 
            count.ForeColor = Color.Silver;
            count.Location = new Point(23, 93);
            count.Name = "count";
            count.Size = new Size(114, 31);
            count.TabIndex = 41;
            count.Text = "Count";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(35, 35, 35);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(numPriceTo);
            panel1.Controls.Add(numPriceFrom);
            panel1.Controls.Add(btnSearch);
            panel1.Controls.Add(dtpTo);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(dtpFrom);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(cboStatus);
            panel1.Controls.Add(label4);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(17, 13, 17, 13);
            panel1.Size = new Size(1257, 160);
            panel1.TabIndex = 11;
            // 
            // label8
            // 
            label8.ForeColor = Color.White;
            label8.Location = new Point(545, 85);
            label8.Name = "label8";
            label8.Size = new Size(37, 31);
            label8.TabIndex = 0;
            label8.Text = "To";
            // 
            // label7
            // 
            label7.ForeColor = Color.White;
            label7.Location = new Point(297, 87);
            label7.Name = "label7";
            label7.Size = new Size(81, 31);
            label7.TabIndex = 1;
            label7.Text = "Price From";
            // 
            // numPriceTo
            // 
            numPriceTo.BackColor = Color.FromArgb(45, 45, 48);
            numPriceTo.ForeColor = Color.White;
            numPriceTo.Location = new Point(588, 85);
            numPriceTo.Margin = new Padding(3, 4, 3, 4);
            numPriceTo.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numPriceTo.Name = "numPriceTo";
            numPriceTo.Size = new Size(114, 27);
            numPriceTo.TabIndex = 2;
            // 
            // numPriceFrom
            // 
            numPriceFrom.BackColor = Color.FromArgb(45, 45, 48);
            numPriceFrom.ForeColor = Color.White;
            numPriceFrom.Location = new Point(394, 85);
            numPriceFrom.Margin = new Padding(3, 4, 3, 4);
            numPriceFrom.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numPriceFrom.Name = "numPriceFrom";
            numPriceFrom.Size = new Size(114, 27);
            numPriceFrom.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(0, 150, 136);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI Semibold", 9F);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(91, 80);
            btnSearch.Margin = new Padding(3, 4, 3, 4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(149, 43);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // dtpTo
            // 
            dtpTo.ForeColor = Color.White;
            dtpTo.Location = new Point(640, 23);
            dtpTo.Margin = new Padding(3, 4, 3, 4);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(228, 27);
            dtpTo.TabIndex = 5;
            // 
            // label6
            // 
            label6.ForeColor = Color.White;
            label6.Location = new Point(606, 27);
            label6.Name = "label6";
            label6.Size = new Size(114, 31);
            label6.TabIndex = 6;
            label6.Text = "To";
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = Color.FromArgb(45, 45, 48);
            dtpFrom.ForeColor = Color.White;
            dtpFrom.Location = new Point(354, 23);
            dtpFrom.Margin = new Padding(3, 4, 3, 4);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(228, 27);
            dtpFrom.TabIndex = 7;
            // 
            // label5
            // 
            label5.ForeColor = Color.White;
            label5.Location = new Point(297, 27);
            label5.Name = "label5";
            label5.Size = new Size(114, 31);
            label5.TabIndex = 8;
            label5.Text = "From";
            // 
            // cboStatus
            // 
            cboStatus.BackColor = Color.FromArgb(45, 45, 48);
            cboStatus.FlatStyle = FlatStyle.Flat;
            cboStatus.ForeColor = Color.White;
            cboStatus.FormattingEnabled = true;
            cboStatus.Items.AddRange(new object[] { "ALL", "New(0)", "Used(2)", "Expaired(3)" });
            cboStatus.Location = new Point(91, 23);
            cboStatus.Margin = new Padding(3, 4, 3, 4);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(171, 28);
            cboStatus.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(23, 27);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 9;
            label4.Text = "Status";
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(209, 71, 82);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI Semibold", 9F);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(177, 540);
            btnDelete.Margin = new Padding(3, 4, 3, 4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(143, 47);
            btnDelete.TabIndex = 31;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(28, 28, 28);
            panelRight.Controls.Add(lblHeader);
            panelRight.Controls.Add(btnDelete);
            panelRight.Controls.Add(btnPrint);
            panelRight.Controls.Add(btnGenerate);
            panelRight.Controls.Add(numMinutes);
            panelRight.Controls.Add(label2);
            panelRight.Controls.Add(numPrice);
            panelRight.Controls.Add(label3);
            panelRight.Controls.Add(numValidityDays);
            panelRight.Controls.Add(label1);
            panelRight.Controls.Add(numCount);
            panelRight.Controls.Add(count);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(914, 160);
            panelRight.Margin = new Padding(3, 4, 3, 4);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(17, 0, 17, 20);
            panelRight.Size = new Size(343, 620);
            panelRight.TabIndex = 12;
            // 
            // lblHeader
            // 
            lblHeader.BackColor = Color.FromArgb(20, 20, 20);
            lblHeader.Dock = DockStyle.Top;
            lblHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Location = new Point(17, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(309, 67);
            lblHeader.TabIndex = 30;
            lblHeader.Text = "Generate Vouchers";
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormVouchers
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 28, 28);
            ClientSize = new Size(1257, 780);
            Controls.Add(dgvVouchers);
            Controls.Add(panelRight);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormVouchers";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CyberCafe - Voucher Management";
            Load += FormVouchers_Load;
            ((System.ComponentModel.ISupportInitialize)numCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinutes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVouchers).EndInit();
            ((System.ComponentModel.ISupportInitialize)numValidityDays).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPriceTo).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPriceFrom).EndInit();
            panelRight.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private NumericUpDown numCount;
        private NumericUpDown numMinutes;
        private Button btnGenerate;
        private DataGridView dgvVouchers;
        private Button btnPrint;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private NumericUpDown numValidityDays;
        private NumericUpDown numPrice;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label count;
        private Panel panel1;
        private ComboBox cboStatus;
        private Label label4;
        private Button btnSearch;
        private DateTimePicker dtpTo;
        private Label label6;
        private DateTimePicker dtpFrom;
        private Label label5;
        private NumericUpDown numPriceTo;
        private NumericUpDown numPriceFrom;
        private Label label8;
        private Label label7;
        private Button btnDelete;
        private Panel panelRight;
        private Label lblHeader;
    }
}