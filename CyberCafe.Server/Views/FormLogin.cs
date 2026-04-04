using System.Data;
using CyberCafe.Core.Data;

namespace CyberCafe.Server.Views
{
    /// <summary>
    /// Represents the login form for the server application.
    /// </summary>
    public partial class FormLogin : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormLogin"/> class.
        /// </summary>
        public FormLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the Login button.
        /// Validates user credentials against the database and sets the current session.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            // Call database validation method
            DataTable result = DatabaseManager.ValidateEmployeeLogin(txtUsername.Text, txtPassword.Text);

            if (result.Rows.Count > 0)
            {
                // Record user data in the session
                DataRow row = result.Rows[0];
                CurrentSession.EmployeeID = Convert.ToInt32(row["EmployeeID"]);
                CurrentSession.FullName = row["FullName"].ToString();
                CurrentSession.Role = Convert.ToInt32(row["Role"]);

                // Set dialog result and close the login screen
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
        /// Handles the Click event of the Exit button.
        /// Terminates the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the KeyDown event of the password text box.
        /// Allows the user to press Enter to trigger the login process.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the username text box.
        /// Allows the user to press Enter to go to the next text box
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
            }
        }
    }
}
