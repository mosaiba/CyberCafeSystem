using System.Diagnostics;
using System.Net.NetworkInformation;
using CyberCafe.Client.Helpers;
using CyberCafe.Client.Network;
using CyberCafe.Core.Network;

namespace CyberCafe.Client
{
    /// <summary>
    /// Represents the main form of the client application, handling kiosk mode, session management, and server communication.
    /// </summary>
    public partial class FormClient : Form
    {
        // Global variables for session and device identification
        private string _myDeviceId = "";
        private ClientNetwork _network;
        private System.Windows.Forms.Timer sessionTimer;
        private int remainingSeconds;
        private string _currentCode = "";
        private KeyboardHook _keyboardHook;

        // Task Manager protection variables
        private Thread _taskManagerKillerThread;
        private bool _isRunning = true;

        /// <summary>
        /// Initializes a new instance of the form1 class.
        /// </summary>
        public FormClient()
        {
            InitializeComponent();
            _network = new ClientNetwork();
        }

        /// <summary>
        /// Handles the Form Load event, initializing security measures and network connectivity.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Activate keyboard hook for security
            _keyboardHook = new KeyboardHook();
            _keyboardHook.IsBlockingActive = true;

            // 2. Start the Task Manager killer thread
            StartTaskManagerKiller();

            // 3. Enter kiosk mode to lock the UI
            EnterKioskMode();

            // 4. Configure network connectivity (order is critical)

            // A. Retrieve the MAC Address for device identification
            _myDeviceId = GetMacAddress();

            // B. Set the device ID in the network class before connecting
            _network.SetDeviceId(_myDeviceId);

            // C. Subscribe to network events
            _network.OnLoginResult = (msg) =>
            {
                if (this.InvokeRequired) this.Invoke(new Action(() => HandleLoginResponse(msg)));
                else HandleLoginResponse(msg);
            };

            _network.OnServerLost = () =>
            {
                if (this.InvokeRequired) this.Invoke(new Action(ServerDisconnected));
                else ServerDisconnected();
            };

            _network.OnConnected = () =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        lblStatus.Text = "Connected to Server";
                        lblStatus.ForeColor = Color.Silver;
                    }));
                }
                else
                {
                    lblStatus.Text = "Connected to Server";
                    lblStatus.ForeColor = Color.Silver;
                }
            };

            _network.OnCommandReceived = (cmdType, payload) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => ExecuteCommand(cmdType, payload)));
                }
                else
                {
                    ExecuteCommand(cmdType, payload);
                }
            };

            // D. Attempt to establish connection to the server
            TryConnect();

            // Bind button events
            btnLogin.Click += btnLogin_Click;
            btnLogout.Click += btnLogout_Click;
            txtCode.MouseClick += (s, args) => { if (txtCode.Text == "Enter Code") txtCode.Text = ""; };

            // Enable secret admin shortcut key
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        // === Connection Methods ===

        /// <summary>
        /// Attempts to discover the server and connect in a background task.
        /// </summary>
        private void TryConnect()
        {
            lblStatus.Text = "Discovering Server...";
            lblStatus.ForeColor = Color.Silver;

            System.Threading.Tasks.Task.Run(() =>
            {
                // Step 1: Initiate network discovery to locate the backend server
                bool found = _network.DiscoverServer();

                if (found)
                {
                    // Target acquired, initiating TCP payload connection
                    this.Invoke(new Action(() => lblStatus.Text = "Connecting..."));

                    bool connected = _network.Connect();

                    if (!connected)
                    {
                        this.Invoke(new Action(() =>
                        {
                            lblStatus.Text = "Connection Failed";
                            lblStatus.ForeColor = Color.Red;
                        }));
                    }
                }
                else
                {
                    // Broadcase timed out with no response
                    this.Invoke(new Action(() =>
                    {
                        lblStatus.Text = "Server Not Found";
                        lblStatus.ForeColor = Color.Red;
                    }));
                }
            });
        }

        // === Display Modes ===

        /// <summary>
        /// Transitions the UI to kiosk mode, locking it for unauthorized use.
        /// </summary>
        private void EnterKioskMode()
        {
            if (_keyboardHook != null) _keyboardHook.IsBlockingActive = true;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.TopMost = true;

            pnlLogin.Visible = true;
            pnlLogin.Dock = DockStyle.Fill;

            // Center the login container
            pnlLoginContainer.Left = (this.ClientSize.Width - pnlLoginContainer.Width) / 2;
            pnlLoginContainer.Top = (this.ClientSize.Height - pnlLoginContainer.Height) / 2;

            pnlSession.Visible = false;
        }

        /// <summary>
        /// Transitions the UI to session mode, allowing authorized user access.
        /// </summary>
        private void EnterSessionMode()
        {
            if (_keyboardHook != null) _keyboardHook.IsBlockingActive = false;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;

            int formWidth = 130;
            int formHeight = 45;
            this.Size = new Size(formWidth, formHeight);

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int x = (screenWidth - formWidth) / 2;
            int y = 0;

            this.Location = new Point(x, y);

            pnlLogin.Visible = false;
            pnlSession.Visible = true;
            pnlSession.Dock = DockStyle.Fill;
        }

        // === Events and Logic ===

        /// <summary>
        /// Handles the login button click event, sending a login request to the server.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text;
            if (string.IsNullOrEmpty(code) || code == "Enter Code") return;

            _currentCode = code;
            lblStatus.Text = "Verifying...";

            if (!_network.Connect())
            {
                lblStatus.Text = "Server Offline";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            NetworkPacket loginPacket = new NetworkPacket
            {
                Type = PacketType.LoginRequest,
                Payload = code
            };
            _network.SendPacket(loginPacket);
        }

        /// <summary>
        /// Processes the login response received from the server.
        /// </summary>
        /// <param name="message">The response message from the server.</param>
        private void HandleLoginResponse(string message)
        {
            if (message.StartsWith("SUCCESS"))
            {
                string[] parts = message.Split(':');
                int minutes = int.Parse(parts[1]);
                remainingSeconds = minutes * 60;

                EnterSessionMode();

                if (sessionTimer == null) sessionTimer = new System.Windows.Forms.Timer();
                sessionTimer.Interval = 1000;
                sessionTimer.Tick -= Timer_Tick;
                sessionTimer.Tick += Timer_Tick;
                sessionTimer.Start();

                UpdateTimeDisplay();
            }
            else
            {
                lblStatus.Text = "Error: " + message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Handles the session timer tick, updating the remaining time.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                UpdateTimeDisplay();
            }
            else
            {
                sessionTimer.Stop();
                LockWorkstation();
            }
        }

        /// <summary>
        /// Updates the time display label on the UI.
        /// </summary>
        private void UpdateTimeDisplay()
        {
            int mins = (remainingSeconds % 3600) / 60;
            int secs = remainingSeconds % 60;
            lblTime.Text = $"{mins:D2}:{secs:D2}";
            lblTime.ForeColor = remainingSeconds < 300 ? Color.Red : Color.LimeGreen;
        }

        /// <summary>
        /// Handles the logout button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            sessionTimer.Stop();
            LockWorkstation();
        }

        /// <summary>
        /// Locks the workstation and returns to kiosk mode.
        /// </summary>
        private void LockWorkstation()
        {
            // Send session end data to the server
            if (!string.IsNullOrEmpty(_currentCode))
            {
                string payload = $"VOUCHER:{_currentCode}:{remainingSeconds}";
                NetworkPacket logoutPacket = new NetworkPacket
                {
                    Type = PacketType.Logout,
                    Payload = payload
                };
                _network.SendPacket(logoutPacket);
            }

            _currentCode = "";
            lblTime.ForeColor = Color.LimeGreen;

            // Update connection status display
            if (_network.IsConnected)
            {
                lblStatus.Text = "Connected to Server";
                lblStatus.ForeColor = Color.Silver;
            }
            else
            {
                lblStatus.Text = "Server Offline";
                lblStatus.ForeColor = Color.Red;
            }

            txtCode.Text = "Enter Code";
            EnterKioskMode();
        }

        /// <summary>
        /// Handles server disconnection events.
        /// </summary>
        private void ServerDisconnected()
        {
            if (sessionTimer != null) sessionTimer.Stop();
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => LockWorkstation()));
            }
            else
            {
                LockWorkstation();
            }
        }

        // === Remote Commands from Server ===

        /// <summary>
        /// Executes commands received from the server.
        /// </summary>
        /// <param name="cmd">The type of command received.</param>
        /// <param name="payload">Optional data associated with the command.</param>
        private void ExecuteCommand(PacketType cmd, string payload)
        {
            switch (cmd)
            {
                case PacketType.CommandShutdown:
                    Process.Start("shutdown", "/s /t 0");
                    break;

                case PacketType.CommandRestart:
                    Process.Start("shutdown", "/r /t 0");
                    break;

                case PacketType.CommandForceLogout:
                    LockWorkstation();
                    break;

                case PacketType.CommandMessage:
                    // Temporarily disable TopMost to ensure the message box is visible to the user
                    this.TopMost = false;
                    MessageBox.Show(payload, "Message from Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.TopMost = true;
                    break;
            }
        }

        // === Security and Exit Methods ===

        /// <summary>
        /// Handles key down events, specifically checking for the admin exit shortcut.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A KeyEventArgs that contains the event data.</param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.Alt && e.KeyCode == Keys.X)
            {
                PromptAdminExit();
            }
        }

        /// <summary>
        /// Prompts the administrator for a password to exit the application.
        /// </summary>
        private void PromptAdminExit()
        {
            _keyboardHook.IsBlockingActive = false;
            this.TopMost = false;

            string password = Microsoft.VisualBasic.Interaction.InputBox("Enter Admin Password:", "Exit Application", "", -1, -1);

            this.TopMost = true;

            if (password == "1234")
            {
                ExitApplication();
            }
            else if (!string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Wrong Password.");
                _keyboardHook.IsBlockingActive = true;
            }
            else
            {
                _keyboardHook.IsBlockingActive = true;
            }
        }

        /// <summary>
        /// Safely terminates the application, releasing resources and restoring system settings.
        /// </summary>
        private void ExitApplication()
        {
            _isRunning = false;
            _keyboardHook?.Dispose();
            sessionTimer?.Stop();

            try
            {
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                regKey.DeleteValue("DisableTaskMgr", false);
                regKey.Close();
            }
            catch { }

            Application.Exit();
        }

        /// <summary>
        /// Starts a background thread that continuously attempts to terminate the Windows Task Manager process.
        /// </summary>
        private void StartTaskManagerKiller()
        {
            _taskManagerKillerThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    try
                    {
                        foreach (var p in Process.GetProcessesByName("taskmgr"))
                        {
                            try { p.Kill(); } catch { }
                        }
                    }
                    catch { }
                    Thread.Sleep(500);
                }
            });
            _taskManagerKillerThread.IsBackground = true;
            _taskManagerKillerThread.Start();
        }

        /// <summary>
        /// Retrieves the MAC address of the first available active network interface.
        /// </summary>
        /// <returns>The MAC address as a string, or a generated fallback ID if none is found.</returns>
        private string GetMacAddress()
        {
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up
                        && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        return nic.GetPhysicalAddress().ToString();
                    }
                }
            }
            catch { }
            return "UNKNOWN-" + new Random().Next(1000, 9999);
        }

        /// <summary>
        /// Handles the Form Closing event to prevent unauthorized closing of the client.
        /// </summary>
        /// <param name="e">A FormClosingEventArgs that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
            else
            {
                _isRunning = false;
                _keyboardHook?.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}