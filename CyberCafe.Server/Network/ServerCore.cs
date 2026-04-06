using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using CyberCafe.Core.Network;
using CyberCafe.Core.Data;
using System.Collections.Concurrent;

namespace CyberCafe.Server
{
    /// <summary>
    /// Manages core network operations for the server, including listening for client connections and processing packets.
    /// </summary>
    public class ServerCore
    {
        private TcpListener _listener;
        private UdpClient _discoveryListener; // جديد: مستخدم لاكتشاف السيرفر
        private int _port = 13000;
        private int _discoveryPort = 13001; // جديد: بورت خاص لاكتشاف السيرفر
        private bool _isRunning;

        /// <summary>
        /// A thread-safe collection of all currently connected devices, keyed by their unique device ID.
        /// </summary>
        public ConcurrentDictionary<string, ConnectedDevice> ConnectedDevices = new ConcurrentDictionary<string, ConnectedDevice>();

        /// <summary>
        /// An action invoked to log server events or messages to the UI.
        /// </summary>
        public Action<string> OnLog;

        /// <summary>
        /// Starts the server to listen for incoming client connections and monitor heartbeats.
        /// </summary>
        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            _isRunning = true;

            // Start thread to accept incoming client connections
            Thread listenThread = new Thread(ListenForClients);
            listenThread.IsBackground = true;
            listenThread.Start();

            // Start thread to monitor client heartbeats and detect disconnections
            Thread monitorThread = new Thread(MonitorHeartbeats);
            monitorThread.IsBackground = true;
            monitorThread.Start();

            // جديد: تشغيل خدمة اكتشاف السيرفر (UDP Broadcast)
            StartDiscoveryListener();

            OnLog?.Invoke($"Server Started on TCP Port: {_port}");
        }

        /// <summary>
        /// جديد: يبدأ الاستماع لطلبات البث (UDP) للسماح للعملاء بالعثور على السيرفر تلقائياً.
        /// </summary>
        private void StartDiscoveryListener()
        {
            try
            {
                _discoveryListener = new UdpClient(_discoveryPort);
                Thread discoveryThread = new Thread(() =>
                {
                    while (_isRunning)
                    {
                        try
                        {
                            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                            // استقبال أي رسالة تأتي على بورت الاكتشاف
                            byte[] data = _discoveryListener.Receive(ref remoteEP);
                            string message = Encoding.UTF8.GetString(data);

                            // التحقق مما إذا كانت الرسالة هي طلب بحث عن السيرفر
                            if (message == "CYBERCAFE_DISCOVERY_REQUEST")
                            {
                                OnLog?.Invoke($"Discovery request received from {remoteEP.Address}");

                                // إرسال الرد للعميل يحتوي على IP السيرفر وبورت الاتصال
                                string myIP = GetLocalIPAddress();
                                string response = $"CYBERCAFE_SERVER:{myIP}:{_port}";
                                byte[] responseData = Encoding.UTF8.GetBytes(response);

                                // إرسال الرد لنفس العنوان الذي أرسل الطلب
                                _discoveryListener.Send(responseData, responseData.Length, remoteEP);
                            }
                        }
                        catch (Exception ex)
                        {
                            // تجاهل الأخطاء الناتجة عن إغلاق السوكت
                            if (_isRunning)
                            {
                                OnLog?.Invoke($"Discovery listener error: {ex.Message}");
                            }
                        }
                    }
                });
                discoveryThread.IsBackground = true;
                discoveryThread.Start();
                OnLog?.Invoke($"Discovery Service Started on UDP Port: {_discoveryPort}");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Error starting Discovery Service: {ex.Message}");
            }
        }

        /// <summary>
        /// جديد: دالة مساعدة لجلب عنوان الـ IP الخاص بالجهاز (IPv4).
        /// </summary>
        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                // نختار فقط عناوين IPv4
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1"; // القيمة الافتراضية
        }

        /// <summary>
        /// Periodically checks the heartbeat of all connected devices and handles timed-out connections.
        /// </summary>
        private void MonitorHeartbeats()
        {
            while (_isRunning)
            {
                Thread.Sleep(5000); // Check every 5 seconds

                // Collect IDs of devices that have timed out (no heartbeat for 15+ seconds)
                var devicesToRemove = new List<string>();

                foreach (var device in ConnectedDevices.Values)
                {
                    if (DateTime.Now.Subtract(device.LastHeartbeat).TotalSeconds > 15)
                    {
                        devicesToRemove.Add(device.DeviceId);
                    }
                }

                // Disconnect each timed-out device
                foreach (var id in devicesToRemove)
                {
                    HandleDisconnection(id);
                }
            }
        }

        /// <summary>
        /// Continuously listens for and accepts incoming TcpClient connections.
        /// </summary>
        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch { }
            }
        }

        /// <summary>
        /// Processes the data stream for an individual connected client.
        /// </summary>
        /// <param name="client">The TcpClient object for the connection.</param>
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = null;
            string deviceId = "UNKNOWN";
            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int byteCount;

                while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedJson = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    NetworkPacket packet = null;
                    try { packet = JsonConvert.DeserializeObject<NetworkPacket>(receivedJson); }
                    catch { continue; }

                    if (packet == null) continue;

                    deviceId = packet.SenderId;

                    // Register or update device connection status
                    if (!ConnectedDevices.ContainsKey(deviceId))
                    {
                        // Retrieve friendly name from the database
                        string friendlyName = DatabaseManager.GetDeviceName(deviceId);

                        // If new device, assign a default friendly name
                        if (friendlyName == deviceId)
                        {
                            int count = ConnectedDevices.Count + 1;
                            friendlyName = $"PC-{count:D2}";
                            DatabaseManager.SaveDeviceName(deviceId, friendlyName);
                        }

                        ConnectedDevices.TryAdd(deviceId, new ConnectedDevice
                        {
                            Client = client,
                            DeviceId = deviceId,
                            FriendlyName = friendlyName,
                            LastHeartbeat = DateTime.Now
                        });
                        OnLog?.Invoke($"Device Connected: {friendlyName} ({deviceId})");
                    }
                    else
                    {
                        // Update heartbeat and client reference for existing connection
                        ConnectedDevices[deviceId].LastHeartbeat = DateTime.Now;
                        ConnectedDevices[deviceId].Client = client;
                    }

                    ProcessPacket(packet, stream, deviceId);
                }
            }
            catch
            {
                HandleDisconnection(deviceId);
            }
        }

        /// <summary>
        /// Updates the friendly name of a specific device in the system.
        /// </summary>
        /// <param name="macAddress">The MAC address of the device to rename.</param>
        /// <param name="newName">The new friendly name.</param>
        public void RenameDevice(string macAddress, string newName)
        {
            if (ConnectedDevices.ContainsKey(macAddress))
            {
                ConnectedDevices[macAddress].FriendlyName = newName;
                DatabaseManager.SaveDeviceName(macAddress, newName);
                OnLog?.Invoke($"Device renamed to: {newName}");
            }
        }

        /// <summary>
        /// Handles the disconnection of a device, pausing active user sessions if necessary.
        /// </summary>
        /// <param name="deviceId">The identifier of the disconnected device.</param>
        private void HandleDisconnection(string deviceId)
        {
            if (ConnectedDevices.TryRemove(deviceId, out ConnectedDevice device))
            {
                if (!string.IsNullOrEmpty(device.CurrentCode))
                {
                    // If an active session was running, pause the voucher to preserve time
                    DatabaseManager.PauseVoucher(device.CurrentCode);
                    OnLog?.Invoke($"Device {device.FriendlyName} disconnected. Voucher {device.CurrentCode} paused.");
                }
                else
                {
                    OnLog?.Invoke($"Device {device.FriendlyName} disconnected.");
                }
            }
        }

        /// <summary>
        /// Processes an individual network packet received from a client.
        /// </summary>
        /// <param name="packet">The received network packet.</param>
        /// <param name="stream">The network stream for responding.</param>
        /// <param name="deviceId">The identifier of the sender device.</param>
        private void ProcessPacket(NetworkPacket packet, NetworkStream stream, string deviceId)
        {
            // Ensure the heartbeat timestamp is updated upon receiving any data
            if (ConnectedDevices.ContainsKey(deviceId))
            {
                ConnectedDevices[deviceId].LastHeartbeat = DateTime.Now;
            }

            switch (packet.Type)
            {
                case PacketType.Heartbeat:
                    // Only heartbeat timestamp needs update (already handled above)
                    break;

                case PacketType.LoginRequest:
                    string validationResult = DatabaseManager.ValidateVoucher(packet.Payload);
                    if (validationResult.StartsWith("SUCCESS"))
                    {
                        if (ConnectedDevices.ContainsKey(deviceId))
                            ConnectedDevices[deviceId].CurrentCode = packet.Payload;

                        SendResponse(stream, PacketType.LoginResponse, validationResult);
                        OnLog?.Invoke($"Login Success: {packet.Payload} on {ConnectedDevices[deviceId].FriendlyName}");
                    }
                    else
                    {
                        SendResponse(stream, PacketType.LoginResponse, validationResult);
                    }
                    break;

                case PacketType.Logout:
                    try
                    {
                        string[] data = packet.Payload.Split(':');
                        if (data.Length >= 3)
                        {
                            string code = data[1];
                            int remainingSecs = int.Parse(data[2]);
                            DatabaseManager.SetRemainingMinutes(code, remainingSecs / 60);

                            if (ConnectedDevices.ContainsKey(deviceId))
                                ConnectedDevices[deviceId].CurrentCode = null;

                            OnLog?.Invoke($"Logout received for voucher: {code}");
                        }
                    }
                    catch (Exception ex) { OnLog?.Invoke($"Logout processing error: {ex.Message}"); }
                    break;
            }
        }

        /// <summary>
        /// Sends a remote control command to a specific device.
        /// </summary>
        /// <param name="deviceId">The identifier of the target device.</param>
        /// <param name="command">The command type to send.</param>
        /// <param name="payload">Optional data associated with the command.</param>
        public void SendCommand(string deviceId, PacketType command, string payload = "")
        {
            if (ConnectedDevices.TryGetValue(deviceId, out ConnectedDevice device))
            {
                try
                {
                    NetworkPacket packet = new NetworkPacket { Type = command, Payload = payload };
                    string json = JsonConvert.SerializeObject(packet);
                    byte[] buffer = Encoding.UTF8.GetBytes(json);

                    NetworkStream stream = device.Client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();

                    OnLog?.Invoke($"Command {command} sent to {device.FriendlyName}");
                }
                catch (Exception ex)
                {
                    OnLog?.Invoke($"Failed to send command to {device.FriendlyName}: {ex.Message}");
                    HandleDisconnection(deviceId);
                }
            }
            else
            {
                OnLog?.Invoke($"Device {deviceId} not found for sending command.");
            }
        }

        /// <summary>
        /// Sends a simple network response packet to a client stream.
        /// </summary>
        /// <param name="stream">The target network stream.</param>
        /// <param name="type">The type of packet to send.</param>
        /// <param name="payload">The response data payload.</param>
        private void SendResponse(NetworkStream stream, PacketType type, string payload)
        {
            try
            {
                NetworkPacket response = new NetworkPacket { Type = type, Payload = payload };
                string json = JsonConvert.SerializeObject(response);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
            catch { }
        }

        /// <summary>
        /// Stops the server listener and terminates network activities.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
            _discoveryListener?.Close(); // جديد: إغلاق مستمع الاكتشاف
        }
    }
}