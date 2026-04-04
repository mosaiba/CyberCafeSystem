using System.Text;
using CyberCafe.Core.Data;

namespace CyberCafe.Server.Views
{
    /// <summary>
    /// Form for generating, searching, printing, and deleting vouchers.
    /// </summary>
    public partial class FormVouchers : Form
    {
        private Random random = new Random();
        private System.Windows.Forms.WebBrowser webBrowser1 = new System.Windows.Forms.WebBrowser();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormVouchers"/> class.
        /// </summary>
        public FormVouchers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the FormVouchers control.
        /// Initializes the UI components, sets default date ranges, and loads initial voucher data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FormVouchers_Load(object sender, EventArgs e)
        {
            RefreshGrid();
            this.Controls.Add(webBrowser1);
            webBrowser1.Visible = false;
            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            
            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;
            cboStatus.SelectedIndex = 0;
            
            // Set default price range to include all possible values
            numPriceFrom.Value = 0;
            numPriceTo.Value = 5000;
            
            btnSearch_Click(null, null);
        }

        /// <summary>
        /// Handles the Click event of the Generate button.
        /// Generates a specified number of vouchers with the given parameters and saves them to the database.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int count = (int)numCount.Value;
            int minutes = (int)numMinutes.Value;
            decimal price = numPrice.Value;
            int validityDays = (int)numValidityDays.Value;

            if (count <= 0 || minutes <= 0)
            {
                MessageBox.Show("Please enter valid values.");
                return;
            }

            try
            {
                for (int i = 0; i < count; i++)
                {
                    string code = GenerateCode();
                    DatabaseManager.AddVoucher(code, minutes, price, validityDays);
                }

                MessageBox.Show($"Successfully generated {count} vouchers!", "Success");
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles the Click event of the Print button.
        /// Generates an HTML representation of the vouchers for printing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvVouchers.Rows.Count == 0)
            {
                MessageBox.Show("No vouchers available to print.");
                return;
            }

            StringBuilder html = new StringBuilder();
            html.Append("<html><head><style>");

            // Page settings
            html.Append("body { font-family: Arial; margin: 0; padding: 5mm; }");

            // Table styling
            html.Append("table { width: 100%; border-collapse: collapse; }");
            html.Append("td { width: 33%; padding: 2mm; vertical-align: top; }");

            // Voucher design
            html.Append(".card { border: 1px dashed #000; height: 115px; padding: 5px; text-align: center; page-break-inside: avoid; }");
            html.Append(".header { font-size: 14px; font-weight: bold; border-bottom: 1px solid #ccc; margin-bottom: 5px; }");
            html.Append(".code { font-size: 22px; font-weight: bold; margin: 5px 0; }");

            // Information styling
            html.Append(".info { font-size: 10px; color: #666; }"); // Time and validity
            html.Append(".price { font-size: 16px; font-weight: bold; color: #c0392b; margin-top: 5px; }"); // Prominent price

            html.Append("</style></head><body>");
            html.Append("<table>");

            int col = 0;
            foreach (DataGridViewRow row in dgvVouchers.Rows)
            {
                if (row.Cells["VoucherCode"].Value != null)
                {
                    if (col == 0) html.Append("<tr>");

                    string code = row.Cells["VoucherCode"].Value.ToString();
                    string minutes = row.Cells["TotalMinutes"].Value.ToString();
                    string price = (row.Cells["Price"].Value != null) ? row.Cells["Price"].Value.ToString() : "0";
                    string validity = (row.Cells["ValidityDays"].Value != null) ? row.Cells["ValidityDays"].Value.ToString() : "30";

                    html.Append("<td>");
                    html.Append("<div class='card'>");
                    html.Append("<div class='header'>Cyber Cafe</div>");
                    html.Append($"<div class='code'>{code}</div>");
                    html.Append($"<div class='info'>Time: {minutes} Min | Valid: {validity} D</div>");
                    html.Append($"<div class='price'>Price: {price}</div>");
                    html.Append("</div>");
                    html.Append("</td>");

                    col++;
                    if (col == 3) { html.Append("</tr>"); col = 0; }
                }
            }

            if (col != 0)
            {
                while (col < 3) { html.Append("<td></td>"); col++; }
                html.Append("</tr>");
            }

            html.Append("</table>");
            html.Append("</body></html>");

            webBrowser1.DocumentText = html.ToString();
        }

        /// <summary>
        /// Generates a random 8-character alphanumeric voucher code.
        /// </summary>
        /// <returns>A random alphanumeric string.</returns>
        private string GenerateCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] buffer = new char[8];
            for (int i = 0; i < 8; i++)
            {
                buffer[i] = chars[random.Next(chars.Length)];
            }
            return new string(buffer);
        }

        /// <summary>
        /// Refreshes the voucher grid with all available vouchers.
        /// </summary>
        private void RefreshGrid()
        {
            dgvVouchers.DataSource = DatabaseManager.GetFilteredVouchers("ALL", null, null, 0, 10000);
        }

        /// <summary>
        /// Handles the DocumentCompleted event of the WebBrowser control.
        /// Displays the print preview dialog once the HTML content is fully loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="WebBrowserDocumentCompletedEventArgs"/> instance containing the event data.</param>
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }

        /// <summary>
        /// Handles the Click event of the Search button.
        /// Filters the vouchers based on status, date range, and price range.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string status = cboStatus.SelectedItem?.ToString();

            // Use .Date to get the start of the day (00:00:00)
            DateTime? from = dtpFrom.Value.Date;
            DateTime? to = dtpTo.Value.Date;

            decimal priceFrom = numPriceFrom.Value;
            decimal priceTo = numPriceTo.Value;

            dgvVouchers.DataSource = DatabaseManager.GetFilteredVouchers(status, from, to, priceFrom, priceTo);
        }

        /// <summary>
        /// Handles the Click event of the Delete button.
        /// Deletes the selected voucher from the database after user confirmation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            // 1. Verify that a row is selected
            if (dgvVouchers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a voucher to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Retrieve the voucher code from the selected row
            string code = dgvVouchers.SelectedRows[0].Cells["VoucherCode"].Value.ToString();

            // 3. Confirm deletion with the user
            if (MessageBox.Show($"Are you sure you want to delete voucher: {code}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // 4. Call the database delete method
                bool success = DatabaseManager.DeleteVoucher(code);

                if (success)
                {
                    MessageBox.Show("Voucher deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show("Cannot delete this voucher.\nIt might be sold or already used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
