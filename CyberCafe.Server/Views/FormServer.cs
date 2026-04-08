using CyberCafe.Core.Data;
using CyberCafe.Core.Network;
using CyberCafe.Server.Views;

namespace CyberCafe.Server
{
    /// <summary>
    /// Represents the main server form that manages connected client devices, 
    /// employee sessions, administrative commands, and data logs.
    /// </summary>
    public partial class FormServer : Form
    {
        private ServerCore _server;
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
        /// Handles the Load event of the main server form.
        /// Initializes UI, starts the network server, and forces employee login.
        /// </summary>
        public void Form1_Load(object sender, EventArgs e)
        {
            // === 1. Initialize data grids primarily (before writing logs) ===
            InitializeGrids();

            // === 2. Bind the logging event hook ===
            _server.OnLog = (msg) =>
            {
                // Ensure thread safety via Invoke before accessing UI controls
                if (dgvLogs.InvokeRequired)
                    dgvLogs.Invoke(new Action(() => AddLog(msg)));
                else
                    AddLog(msg);
            };

            // === 3. Start the TCP server listener ===
            _server.Start();

            // === 4. Start the UI polling timer to refresh grid data ===
            _updateGridTimer = new System.Windows.Forms.Timer();
            _updateGridTimer.Interval = 1000;
            _updateGridTimer.Tick += (s, args) => RefreshDeviceGrid();
            _updateGridTimer.Start();

            // === 5. Request administrator or cashier login ===
            ShowLoginScreen();
        }

        /// <summary>
        /// Initializes the UI datagrids columns and configurations.
        /// </summary>
        private void InitializeGrids()
        {
            // Initialize Logs Grid
            if (dgvLogs.Columns.Count == 0)
            {
                dgvLogs.Columns.Add("Time", "Time");
                dgvLogs.Columns.Add("Message", "Message");
                
                // Adjust column sizing
                dgvLogs.Columns["Time"].Width = 80;
                dgvLogs.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Initialize Connected Devices Grid
            if (dgvDevices.Columns.Count == 0)
            {
                dgvDevices.Columns.Add("FriendlyName", "Device Name");
                dgvDevices.Columns.Add("Status", "Status");
                dgvDevices.Columns.Add("CurrentCode", "Code");
                dgvDevices.Columns.Add("LastSeen", "Last Seen");
                dgvDevices.Columns.Add("MacAddress", "MAC");

                // Keep the MAC Address column hidden from standard UI view
                dgvDevices.Columns["MacAddress"].Visible = false;
            }
        }

        /// <summary>
        /// Displays the login form and applies UI restrictions based on the signed-in role.
        /// </summary>
        private void ShowLoginScreen()
        {
            CurrentSession.EmployeeID = 0;
            CurrentSession.FullName = null;
            CurrentSession.Role = 0;

            this.Enabled = false;

            FormLogin loginForm = new FormLogin();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                ApplyUserPermissions();
                this.Enabled = true;
            }
            else
            {
                Application.Exit();
            }
        }

        private void ApplyUserPermissions()
        {
            if (CurrentSession.FullName != null)
            {
                lblUserStatus.Text = $"User: {CurrentSession.FullName} ({(CurrentSession.IsAdmin() ? "Admin" : "Cashier")})";
            }

            bool isAdmin = CurrentSession.IsAdmin();
            btnManageEmployees.Visible = isAdmin;
            btnManageVouchers.Visible = isAdmin;
        }

        private void RefreshDeviceGrid()
        {
            string selectedId = null;
            if (dgvDevices.SelectedRows.Count > 0)
                selectedId = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value?.ToString();

            dgvDevices.Rows.Clear();
            foreach (var device in _server.ConnectedDevices.Values)
            {
                int index = dgvDevices.Rows.Add(
                    device.FriendlyName,
                    device.IsInSession ? "Busy" : "Idle",
                    device.IsInSession ? device.CurrentCode : "-",
                    device.LastHeartbeat.ToString("HH:mm:ss"),
                    device.DeviceId
                );

                dgvDevices.Rows[index].DefaultCellStyle.BackColor = device.IsInSession ? Color.LightSalmon : Color.LightGreen;
                if (device.DeviceId == selectedId) dgvDevices.Rows[index].Selected = true;
            }
        }

        /// <summary>
        /// Adds a message entry to the system logs grid and auto-scrolls to the bottom.
        /// </summary>
        /// <param name="message">The text command or state message to log.</param>
        private void AddLog(string message)
        {
            // Ensure data grid columns exist
            if (dgvLogs.Columns.Count == 0) return;

            dgvLogs.Rows.Add(DateTime.Now.ToString("HH:mm:ss"), message);

            // Automatically scroll to the latest row
            if (dgvLogs.Rows.Count > 0)
                dgvLogs.FirstDisplayedScrollingRowIndex = dgvLogs.Rows.Count - 1;
        }

        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _updateGridTimer?.Stop();
            _server.Stop();
        }

        private void btnManageVouchers_Click(object sender, EventArgs e)
        {
            FormVouchers frm = new FormVouchers();
            frm.ShowDialog();
        }

        private void btnSellVoucher_Click(object sender, EventArgs e)
        {
            string code = txtSellCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Enter the voucher code.");
                return;
            }

            if (DatabaseManager.SellVoucher(code, CurrentSession.EmployeeID))
            {
                MessageBox.Show($"Voucher {code} sold successfully!", "Success");
                txtSellCode.Text = "";
            }
            else
            {
                MessageBox.Show("Failed to sell. Code might be wrong or already sold.", "Error");
            }
        }

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            FormEmployees frm = new FormEmployees();
            frm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            ShowLoginScreen();
        }

        private void btnRestartClient_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0) { MessageBox.Show("Select a device."); return; }
            string macAddress = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value.ToString();
            string name = dgvDevices.SelectedRows[0].Cells["FriendlyName"].Value.ToString();
            if (MessageBox.Show($"Restart {name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                _server.SendCommand(macAddress, PacketType.CommandRestart);
        }

        private void btnShutdownClient_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0) { MessageBox.Show("Select a device."); return; }
            string macAddress = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value.ToString();
            string name = dgvDevices.SelectedRows[0].Cells["FriendlyName"].Value.ToString();
            if (MessageBox.Show($"Shutdown {name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                _server.SendCommand(macAddress, PacketType.CommandShutdown);
        }

        private void btnForceLogout_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0) { MessageBox.Show("Select a device."); return; }
            string macAddress = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value.ToString();
            string name = dgvDevices.SelectedRows[0].Cells["FriendlyName"].Value.ToString();
            if (MessageBox.Show($"Force logout on {name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                _server.SendCommand(macAddress, PacketType.CommandForceLogout);
        }

        private void DgvDevices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                string currentName = dgvDevices.Rows[e.RowIndex].Cells[0].Value.ToString();
                string macAddress = dgvDevices.Rows[e.RowIndex].Cells[4].Value.ToString();
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new name:", "Rename", currentName, -1, -1);
                if (!string.IsNullOrEmpty(newName) && newName != currentName)
                {
                    _server.RenameDevice(macAddress, newName);
                    RefreshDeviceGrid();
                }
            }
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0) { MessageBox.Show("Select a device."); return; }
            string macAddress = dgvDevices.SelectedRows[0].Cells["MacAddress"].Value.ToString();
            string name = dgvDevices.SelectedRows[0].Cells["FriendlyName"].Value.ToString();
            string message = Microsoft.VisualBasic.Interaction.InputBox("Enter message:", "Send", "", -1, -1);
            if (!string.IsNullOrEmpty(message))
            {
                _server.SendCommand(macAddress, PacketType.CommandMessage, message);
                MessageBox.Show($"Message sent to {name}.", "Success");
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            FormReports frm = new FormReports();
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                   "WARNING: This will shut down the server and disconnect all clients.\nAre you sure you want to exit?",
                   "Confirm Server Shutdown",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
    }
}