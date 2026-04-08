using System.Data;
using CyberCafe.Core.Data;

namespace CyberCafe.Server.Views
{
    /// <summary>
    /// Represents the login form for employee authentication.
    /// Provides security gates for server initialization and exiting.
    /// </summary>
    public partial class FormLogin : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormLogin"/> class.
        /// </summary>
        public FormLogin()
        {
            InitializeComponent();
            // Manually bind the closing event to handle server shutdown logic
            this.FormClosing += FormLogin_FormClosing;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            DataTable result = DatabaseManager.ValidateEmployeeLogin(txtUsername.Text, txtPassword.Text);

            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                CurrentSession.EmployeeID = Convert.ToInt32(row["EmployeeID"]);
                CurrentSession.FullName = row["FullName"].ToString();
                CurrentSession.Role = Convert.ToInt32(row["Role"]);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password, or account is disabled.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.SelectAll();
            }
        }

        /// <summary>
        /// Initiates the form closing process. The actual exit logic is handled by FormLogin_FormClosing.
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Centralized event handler for deciding whether to close the application.
        /// Warns the user of server shutdown if they attempt to close the application without logging in.
        /// </summary>
        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Allow seamless closing if the user successfully authenticated
            if (this.DialogResult == DialogResult.OK) return;

            // If the user explicitly clicked X or the Exit button (bypassing code-driven closes)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Temporarily cancel the close operation to prompt for confirmation
                e.Cancel = true;

                DialogResult dr = MessageBox.Show(
                    "WARNING: This will shut down the server and disconnect all clients.\nAre you sure you want to exit?",
                    "Confirm Server Shutdown",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // If confirmed, proceed to forcefully shut down the entire application
                    e.Cancel = false;
                    Application.Exit();
                }
                // If the user clicked No, e.Cancel remains true preventing the window from closing
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
            }
        }
    }
}