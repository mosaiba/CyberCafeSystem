using System.Data;
using System.Text;
using CyberCafe.Core.Data;

namespace CyberCafe.Server.Views
{
    /// <summary>
    /// Form for generating and printing sales and employee performance reports.
    /// </summary>
    public partial class FormReports : Form
    {
        private WebBrowser webBrowser1 = new WebBrowser();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormReports"/> class.
        /// </summary>
        public FormReports()
        {
            InitializeComponent();
            this.Controls.Add(webBrowser1);
            webBrowser1.Visible = false;
        }

        /// <summary>
        /// Handles the Load event of the FormReports control.
        /// Initializes the date pickers with default values.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FormReports_Load(object sender, EventArgs e)
        {
            dtpDaily.Value = DateTime.Today;
            dtpFrom.Value = DateTime.Today.AddDays(-7);
            dtpTo.Value = DateTime.Today;
        }

        // === Daily Sales Report ===

        /// <summary>
        /// Handles the Click event of the Show Daily button.
        /// Retrieves and displays the daily sales summary.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnShowDaily_Click(object sender, EventArgs e)
        {
            DataTable dt = DatabaseManager.GetDailySalesReport(dtpDaily.Value);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                long count = Convert.ToInt64(row["TotalCount"]);
                decimal price = row["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalPrice"]) : 0;
                long mins = row["TotalMinutes"] != DBNull.Value ? Convert.ToInt64(row["TotalMinutes"]) : 0;

                lblDailyResult.Text = $"Total Vouchers: {count}\nTotal Revenue: {price}\nTotal Minutes: {mins}";
            }
            else
            {
                lblDailyResult.Text = "No sales found for this date.";
            }
        }

        /// <summary>
        /// Handles the Click event of the Print Daily button.
        /// Generates an HTML report of the daily sales and opens the print preview dialog.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnPrintDaily_Click(object sender, EventArgs e)
        {
            DataTable dt = DatabaseManager.GetDailySalesReport(dtpDaily.Value);
            string dateStr = dtpDaily.Value.ToString("yyyy-MM-dd");

            StringBuilder html = new StringBuilder();
            html.Append("<html><head><style>body{font-family:Arial;} table{width:100%; border-collapse:collapse;} th,td{border:1px solid #ccc; padding:10px; text-align:center;}</style></head><body>");
            html.Append($"<h2>Daily Sales Report - {dateStr}</h2>");
            html.Append("<table>");
            html.Append("<tr><th>Total Vouchers</th><th>Total Revenue</th><th>Total Minutes</th></tr>");

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                long count = Convert.ToInt64(row["TotalCount"]);
                decimal price = row["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalPrice"]) : 0;
                long mins = row["TotalMinutes"] != DBNull.Value ? Convert.ToInt64(row["TotalMinutes"]) : 0;

                html.Append($"<tr><td>{count}</td><td>{price}</td><td>{mins}</td></tr>");
            }

            html.Append("</table></body></html>");

            webBrowser1.DocumentText = html.ToString();
            webBrowser1.DocumentCompleted += (s, args) => webBrowser1.ShowPrintPreviewDialog();
        }

        // === Employee Performance Report ===

        /// <summary>
        /// Handles the Click event of the Show Employee button.
        /// Retrieves and displays the employee performance report for the selected period.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnShowEmp_Click(object sender, EventArgs e)
        {
            DataTable dt = DatabaseManager.GetEmployeePerformanceReport(dtpFrom.Value, dtpTo.Value);
            dgvEmpReport.DataSource = dt;
        }

        /// <summary>
        /// Handles the Click event of the Print Employee button.
        /// Generates an HTML report of employee performance and opens the print preview dialog.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnPrintEmp_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvEmpReport.DataSource;
            if (dt == null || dt.Rows.Count == 0) { MessageBox.Show("No data to print."); return; }

            StringBuilder html = new StringBuilder();
            html.Append("<html><head><style>body{font-family:Arial;} table{width:100%; border-collapse:collapse;} th,td{border:1px solid #ccc; padding:10px;}</style></head><body>");
            html.Append($"<h2>Employee Performance Report</h2>");
            html.Append($"<p>Period: {dtpFrom.Value:yyyy-MM-dd} to {dtpTo.Value:yyyy-MM-dd}</p>");
            html.Append("<table>");
            html.Append("<tr><th>Employee</th><th>Vouchers Sold</th><th>Total Revenue</th></tr>");

            foreach (DataRow row in dt.Rows)
            {
                html.Append($"<tr><td>{row["Employee"]}</td><td>{row["VoucherCount"]}</td><td>{Convert.ToDecimal(row["TotalRevenue"])}</td></tr>");
            }

            html.Append("</table></body></html>");

            webBrowser1.DocumentText = html.ToString();
            webBrowser1.DocumentCompleted += (s, args) => webBrowser1.ShowPrintPreviewDialog();
        }
    }
}
