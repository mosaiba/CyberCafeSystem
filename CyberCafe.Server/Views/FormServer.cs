using CyberCafe.Core.Data;
using CyberCafe.Core.Network;
using CyberCafe.Server.Views;

namespace CyberCafe.Server
{
    /// <summary>
    /// Represents the main server form that monitors connected clients and provides management tools.
    /// </summary>
    public partial class FormServer : Form
    {
        private ServerCore _server;
        
        /// <summary>
        /// Timer used to periodically update the connected devices grid.
        /// </summary>
        private System.Windows.Forms.Timer _updateGridTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormServer"/> class.
        /// </summary>
        public FormServer()
        {
            InitializeComponent();
            _server = new ServerCore();
        }

        /// <summary>
        /// Handles the Load event of the Form1 control.
        /// Configures user permissions, initializes data grids, and starts the server.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void Form1_Load(object sender, EventArgs e)
        {
            // === 1. User Settings and Permissions ===
            if (CurrentSession.FullName != null)
            {
                lblUserStatus.Text = $"User: {CurrentSession.FullName} ({(CurrentSession.IsAdmin() ? "Admin" : "Cashier")})";
            }

            if (!CurrentSession.IsAdmin())
            {
                btnManageEmployees.Visible = false;
                btnManageVouchers.Visible = false;
                // Note: Device control buttons can also be hidden for cashiers if needed.
                // btnRestartClient.Visible = false; 
            }

            // === 2. Data Grid Initialization ===
            // Configure logs grid
            if (dgvLogs.Columns.Count == 0)
            {
                dgvLogs.Columns.Add("Time", "Time");
                dgvLogs.Columns.Add("Message", "Message");
            }

            // Configure devices grid (includes a MAC address column)
            if (dgvDevices.Columns.Count == 0)
            {
                dgvDevices.Columns.Add("FriendlyName", "Device Name"); // Column 0
                dgvDevices.Columns.Add("Status", "Status");            // Column 1
                dgvDevices.Columns.Add("CurrentCode", "Code");         // Column 2
                dgvDevices.Columns.Add("LastSeen", "Last Seen");       // Column 3
                dgvDevices.Columns.Add("MacAddress", "MAC");           // Column 4 (Hidden)
            }

            // === 3. Bind Server Events ===
            _server.OnLog = (msg) =>
            {
                if (dgvLogs.InvokeRequired)
                {
                    dgvLogs.Invoke(new Action(() => AddLog(msg)));
                }
                else
                {
                    AddLog(msg);
                }
            };

            // === 4. Start Server ===
            _server.Start();
            AddLog("Server started listening on port 13000...");

            // === 5. Initialize Device Refresh Timer ===
            _updateGridTimer = new System.Windows.Forms.Timer();
            _updateGridTimer.Interval = 1000; // Update every second
            _updateGridTimer.Tick += (s, args) => RefreshDeviceGrid();
            _updateGridTimer.Start();
        }

        /// <summary>
        /// Refreshes the device grid with the current list of connected devices.
        /// </summary>
        private void RefreshDeviceGrid()
        {
            string selectedId = null;
            if (dgvDevices.SelectedRows.Count > 0)
                selectedId = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value?.ToString();

            dgvDevices.Rows.Clear();
            foreach (var device in _server.ConnectedDevices.Values)
            {
                int index = dgvDevices.Rows.Add(
                    device.FriendlyName,   // Column 0: Name
                    device.IsInSession ? "Busy" : "Idle",
                    device.IsInSession ? device.CurrentCode : "-",
                    device.LastHeartbeat.ToString("HH:mm:ss"),
                    device.DeviceId        // Column 4: MAC (Internal use)
                );

                dgvDevices.Rows[index].DefaultCellStyle.BackColor = device.IsInSession ? Color.LightSalmon : Color.LightGreen;

                if (device.DeviceId == selectedId) dgvDevices.Rows[index].Selected = true;
            }

            // Configure headers and visibility
            if (dgvDevices.Columns.Count > 0)
            {
                dgvDevices.Columns[0].HeaderText = "Device Name";
                dgvDevices.Columns[4].Visible = false; // Hide the MAC address column
            }
        }

        /// <summary>
        /// Adds a message to the server log grid.
        /// </summary>
        /// <param name="message">The message to log.</param>
        private void AddLog(string message)
        {
            dgvLogs.Rows.Add(DateTime.Now.ToString("HH:mm:ss"), message);
        }

        /// <summary>
        /// Handles the FormClosing event of the Form1 control.
        /// Stops the timer and shuts down the server.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _updateGridTimer?.Stop();
            _server.Stop();
        }

        // === Management Buttons ===

        /// <summary>
        /// Handles the Click event of the btnManageVouchers control.
        /// Opens the voucher management form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnManageVouchers_Click(object sender, EventArgs e)
        {
            FormVouchers frm = new FormVouchers();
            frm.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the btnSellVoucher control.
        /// Attempts to sell a voucher with the provided code.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSellVoucher_Click(object sender, EventArgs e)
        {
            string code = txtSellCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Enter the voucher code.");
                return;
            }

            int currentEmpId = CurrentSession.EmployeeID;

            if (DatabaseManager.SellVoucher(code, currentEmpId))
            {
                MessageBox.Show($"Voucher {code} sold successfully!", "Success");
                txtSellCode.Text = "";
            }
            else
            {
                MessageBox.Show("Failed to sell. Code might be wrong or already sold.", "Error");
            }
        }

        /// <summary>
        /// Handles the Click event of the btnManageEmployees control.
        /// Opens the employee management form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            FormEmployees frm = new FormEmployees();
            frm.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the btnLogout control.
        /// Shuts down the server and restarts the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            _updateGridTimer?.Stop();
            _server.Stop();
            Application.Restart();
        }

        // === Device Control Buttons ===

        /// <summary>
        /// Handles the Click event of the btnRestartClient control.
        /// Sends a restart command to the selected client device.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnRestartClient_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a device first.");
                return;
            }

            string macAddress = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value.ToString();
            string friendlyName = dgvDevices.SelectedRows[0].Cells["FriendlyName"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to RESTART device {friendlyName}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _server.SendCommand(macAddress, PacketType.CommandRestart);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnShutdownClient control.
        /// Sends a shutdown command to the selected client device.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnShutdownClient_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a device first.");
                return;
            }

            string macAddress = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value.ToString();
            string friendlyName = dgvDevices.SelectedRows[0].Cells["FriendlyName"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to SHUTDOWN device {friendlyName}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _server.SendCommand(macAddress, PacketType.CommandShutdown);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnForceLogout control.
        /// Sends a force logout command to the selected client device.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnForceLogout_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a device first.");
                return;
            }

            string macAddress = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value.ToString();
            string friendlyName = dgvDevices.SelectedRows[0].Cells["FriendlyName"].Value.ToString();

            if (MessageBox.Show($"Force logout user on {friendlyName}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _server.SendCommand(macAddress, PacketType.CommandForceLogout);
            }
        }

        /// <summary>
        /// Handles the CellDoubleClick event of the DgvDevices control.
        /// Allows renaming a device when its name cell is double-clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void DgvDevices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure a valid row is clicked (not the header)
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // Clicked the Name column
            {
                string currentName = dgvDevices.Rows[e.RowIndex].Cells[0].Value.ToString();
                string macAddress = dgvDevices.Rows[e.RowIndex].Cells[4].Value.ToString(); // Hidden MAC

                // Show an input box to get the new name
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new name for the device:", "Rename Device", currentName, -1, -1);

                if (!string.IsNullOrEmpty(newName) && newName != currentName)
                {
                    _server.RenameDevice(macAddress, newName);
                    RefreshDeviceGrid(); // Immediately refresh the display
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSendMessage control.
        /// Sends a text message to the selected client device.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a device first.");
                return;
            }

            // Column 4 is MAC (hidden), Column 0 is Name
            string macAddress = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value.ToString();
            string friendlyName = dgvDevices.SelectedRows[0].Cells["FriendlyName"].Value.ToString();

            // Request message text from the administrator
            string message = Microsoft.VisualBasic.Interaction.InputBox("Enter message to send:", "Send Message", "", -1, -1);

            if (!string.IsNullOrEmpty(message))
            {
                _server.SendCommand(macAddress, PacketType.CommandMessage, message);
                MessageBox.Show($"Message sent to {friendlyName}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnReports control.
        /// Opens the reports form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnReports_Click(object sender, EventArgs e)
        {
            FormReports frm = new FormReports();
            frm.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// Closes the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnMinimize control.
        /// Minimizes the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
