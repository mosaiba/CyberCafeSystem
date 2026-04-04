using System;
using System.Data;
using System.Windows.Forms;
using CyberCafe.Core.Data;

namespace CyberCafe.Server.Views
{
    /// <summary>
    /// Form for managing employees, including adding, updating, and toggling active status.
    /// </summary>
    public partial class FormEmployees : Form
    {
        private int _selectedEmpId = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormEmployees"/> class.
        /// </summary>
        public FormEmployees()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the FormEmployees control.
        /// Initializes the roles combo box, binds formatting events, and loads the employee data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FormEmployees_Load(object sender, EventArgs e)
        {
            // Set up Role ComboBox
            cboRole.Items.Clear();
            cboRole.Items.Add("Admin");
            cboRole.Items.Add("Cashier");
            cboRole.SelectedIndex = 1;

            // Bind cell formatting event for the grid
            dgvEmployees.CellFormatting += dgvEmployees_CellFormatting;

            // Initial data load
            RefreshGrid();
            ClearInputs();
        }

        /// <summary>
        /// Refreshes the employee grid with data from the database, optionally filtered by a search term.
        /// </summary>
        /// <param name="searchTerm">The term to search for in employee names or usernames.</param>
        private void RefreshGrid(string searchTerm = "")
        {
            // Pass search term to the database manager
            dgvEmployees.DataSource = DatabaseManager.GetAllEmployees(searchTerm);

            // Hide password hash column for security
            if (dgvEmployees.Columns["PasswordHash"] != null)
                dgvEmployees.Columns["PasswordHash"].Visible = false;
        }

        /// <summary>
        /// Handles the TextChanged event of the search text box to perform a live search.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Automatically refresh the grid as the user types
            RefreshGrid(txtSearch.Text);
        }

        /// <summary>
        /// Formats cells in the employee grid, including row coloring based on status and role name conversion.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> instance containing the event data.</param>
        private void dgvEmployees_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 1. Color rows based on active status
            if (dgvEmployees.Columns["IsActive"] != null && e.RowIndex >= 0)
            {
                var row = dgvEmployees.Rows[e.RowIndex];
                if (row.Cells["IsActive"].Value != null)
                {
                    bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value);
                    row.DefaultCellStyle.BackColor = isActive ? System.Drawing.Color.White : System.Drawing.Color.LightGray;
                }
            }

            // 2. Format role display text
            if (dgvEmployees.Columns["Role"] != null && e.ColumnIndex == dgvEmployees.Columns["Role"].Index)
            {
                if (e.Value != null)
                {
                    int roleVal;
                    if (int.TryParse(e.Value.ToString(), out roleVal))
                    {
                        // Convert numeric role value (0 or 1) to display text
                        e.Value = (roleVal == 0) ? "Admin" : "Cashier";
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        /// <summary>
        /// Clears all input fields and resets the selected employee state.
        /// </summary>
        private void ClearInputs()
        {
            _selectedEmpId = 0;
            txtName.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            cboRole.SelectedIndex = 1;
            btnAdd.Text = "Add New";
            btnToggle.Text = "Deactivate";
            txtUsername.Enabled = true;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the employee grid.
        /// Populates the input fields with the selected employee's data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvEmployees.SelectedRows[0];

                _selectedEmpId = Convert.ToInt32(row.Cells["EmployeeID"].Value);
                txtName.Text = row.Cells["FullName"].Value.ToString();
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtUsername.Enabled = false;
                txtPassword.Text = "";

                int roleVal = Convert.ToInt32(row.Cells["Role"].Value);
                cboRole.SelectedItem = (roleVal == 0) ? "Admin" : "Cashier";

                bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value);
                btnToggle.Text = isActive ? "Deactivate" : "Activate";

                btnAdd.Text = "New";
            }
        }

        /// <summary>
        /// Handles the Click event of the Add button.
        /// Validates input and adds a new employee to the database.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please fill all fields (Name, Username, Password).");
                return;
            }

            int role = (cboRole.SelectedItem.ToString() == "Admin") ? 0 : 1;

            if (DatabaseManager.AddEmployee(txtName.Text, txtUsername.Text, txtPassword.Text, role))
            {
                MessageBox.Show("Employee added successfully!");
                RefreshGrid(); 
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Username already exists!");
            }
        }

        /// <summary>
        /// Handles the Click event of the Update button.
        /// Updates the selected employee's details in the database.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedEmpId == 0)
            {
                MessageBox.Show("Please select an employee to update.");
                return;
            }

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Name is required.");
                return;
            }

            int role = (cboRole.SelectedItem.ToString() == "Admin") ? 0 : 1;
            string passToUpdate = string.IsNullOrEmpty(txtPassword.Text) ? null : txtPassword.Text;

            DatabaseManager.UpdateEmployee(_selectedEmpId, txtName.Text, role, passToUpdate);
            MessageBox.Show("Employee updated successfully!");

            // Refresh the grid while maintaining the current search text
            RefreshGrid(txtSearch.Text);

            ClearInputs();
        }

        /// <summary>
        /// Handles the Click event of the Toggle button.
        /// Activates or deactivates the selected employee.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnToggle_Click(object sender, EventArgs e)
        {
            if (_selectedEmpId == 0) return;

            bool currentStatus = Convert.ToBoolean(dgvEmployees.SelectedRows[0].Cells["IsActive"].Value);
            bool newStatus = !currentStatus;

            DatabaseManager.ToggleEmployeeStatus(_selectedEmpId, newStatus);

            // Refresh the grid while maintaining the search
            RefreshGrid(txtSearch.Text);
            ClearInputs();
        }

        /// <summary>
        /// Handles the Click event of the Clear button.
        /// Resets all input fields and clears the current selection and search.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
            dgvEmployees.ClearSelection();
            txtSearch.Text = "";
        }
    }
}
