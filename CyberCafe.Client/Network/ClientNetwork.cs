using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using CyberCafe.Core.Network;

namespace CyberCafe.Client.Network
{
    /// <summary>
    /// Manages the network connection and communication between the client and the server.
    /// Handles connection establishment, heartbeat pulses, and data transmission.
    /// </summary>
    public class ClientNetwork
    {
        /// <summary>
        /// Occurs when a login result is received from the server.
        /// </summary>
        public Action<string> OnLoginResult;

        /// <summary>
        /// Occurs when the connection to the server is lost.
        /// </summary>
        public Action OnServerLost;

        /// <summary>
        /// Occurs when the client successfully connects to the server.
        /// </summary>
        public Action OnConnected;

        /// <summary>
        /// Occurs when a command is received from the server.
        /// </summary>
        public Action<PacketType, string> OnCommandReceived;

        private TcpClient _client;
        private NetworkStream _stream;

        // Hardcoded endpoint dependencies removed in favor of dynamic discovery.
        private string _serverIp = "";
        private int _port = 13000;
        private int _discoveryPort = 13001; // The targeted UDP broadcast port

        // System.Threading.Timer is used for stability as it runs in the background thread pool.
        private System.Threading.Timer _heartbeatTimer;
        private string _myDeviceId = "CLIENT-01";
        private readonly object _lock = new object();

        // === Critical Fix: Flag to track connection state and prevent infinite loops ===
        private volatile bool _isConnected = false;

        /// <summary>
        /// Gets a value indicating whether the client is currently connected to the server.
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// Sets the unique device identifier for this client.
        /// </summary>
        /// <param name="id">The device identifier (typically MAC Address).</param>
        public void SetDeviceId(string id)
        {
            _myDeviceId = id;
        }

        /// <summary>
        /// Invokes a network-wide UDP broadcast looking for an active CyberCafe server.
        /// </summary>
        /// <returns>True if a server responded and a valid IP was captured, otherwise False.</returns>
        public bool DiscoverServer()
        {
            using (UdpClient udp = new UdpClient())
            {
                try
                {
                    udp.EnableBroadcast = true;
                    udp.Client.ReceiveTimeout = 3000; // Constrain the blocking wait to 3 seconds

                    string request = "CYBERCAFE_DISCOVERY_REQUEST";
                    byte[] data = Encoding.UTF8.GetBytes(request);

                    // Transmit to the IPv4 broadcast mask
                    udp.Send(data, data.Length, "255.255.255.255", _discoveryPort);

                    // Block thread pending UDP response envelope
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] response = udp.Receive(ref remoteEP);
                    string respString = Encoding.UTF8.GetString(response);

                    // Decode server reply payload configuration
                    if (respString.StartsWith("CYBERCAFE_SERVER:"))
                    {
                        string[] parts = respString.Split(':');
                        if (parts.Length >= 3)
                        {
                            _serverIp = parts[1]; // Store the resolved backend IP destination
                            // _port = int.Parse(parts[2]); // Implementable if dynamic port allocation is needed
                            return true;
                        }
                    }
                }
                catch (SocketException)
                {
                    // Triggered when ReceiveTimeout is eclipsed; exit gracefully
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Establishes a connection to the server.
        /// </summary>
        /// <returns>True if the connection was successful, otherwise false.</returns>
        public bool Connect()
        {
            // Abort routine early if no dynamic IP was successfully discovered
            if (string.IsNullOrEmpty(_serverIp)) return false;
            if (_isConnected) return true;

            try
            {
                _client = new TcpClient();
                _client.Connect(_serverIp, _port);
                _stream = _client.GetStream();

                // Mark as connected immediately after successful handshake
                _isConnected = true;

                // Notify the UI or other subscribers about the connection
                OnConnected?.Invoke();

                // Start the listening thread for incoming server messages
                Thread listenThread = new Thread(ListenForServer);
                listenThread.IsBackground = true;
                listenThread.Start();

                // Start sending heartbeat pulses every 3 seconds
                _heartbeatTimer = new System.Threading.Timer(HeartbeatCallback, null, 3000, 3000);

                return true;
            }
            catch (Exception)
            {
                _isConnected = false;
                _stream = null;
                _client = null;
                return false;
            }
        }

        /// <summary>
        /// Sends heartbeat pulses to the server to maintain the connection.
        /// Runs in a background thread pool.
        /// </summary>
        /// <param name="state">The timer state object.</param>
        private void HeartbeatCallback(object state)
        {
            // Do not attempt to send if already disconnected
            if (!_isConnected) return;

            try
            {
                if (_client == null || !_client.Connected)
                {
                    TriggerServerLost();
                    return;
                }

                NetworkPacket heartbeat = new NetworkPacket
                {
                    Type = PacketType.Heartbeat,
                    SenderId = _myDeviceId,
                    Payload = "PING"
                };

                // Attempt to send the heartbeat packet
                if (!SendPacketInternal(heartbeat))
                {
                    // If sending fails, consider the connection lost
                    TriggerServerLost();
                }
            }
            catch
            {
                TriggerServerLost();
            }
        }

        /// <summary>
        /// Sends a network packet to the server.
        /// </summary>
        /// <param name="packet">The packet to send.</param>
        public void SendPacket(NetworkPacket packet)
        {
            // Prevent sending on a closed connection to avoid IOException loops
            if (!_isConnected) return;

            packet.SenderId = _myDeviceId;
            if (!SendPacketInternal(packet))
            {
                TriggerServerLost();
            }
        }

        /// <summary>
        /// Internal method to send a packet, ensuring thread safety.
        /// </summary>
        /// <param name="packet">The network packet to send.</param>
        /// <returns>True if the packet was sent successfully, otherwise false.</returns>
        private bool SendPacketInternal(NetworkPacket packet)
        {
            if (_stream == null || !_stream.CanWrite) return false;

            try
            {
                string json = JsonConvert.SerializeObject(packet);
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                lock (_lock) // Lock to prevent concurrent send operations
                {
                    _stream.Write(buffer, 0, buffer.Length);
                    _stream.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Continuously listens for incoming data from the server.
        /// </summary>
        private void ListenForServer()
        {
            byte[] buffer = new byte[1024];
            int byteCount;

            try
            {
                // Keep reading while connected and data is available
                while (_isConnected && (byteCount = _stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedJson = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    NetworkPacket packet = JsonConvert.DeserializeObject<NetworkPacket>(receivedJson);

                    if (packet.Type == PacketType.LoginResponse)
                    {
                        OnLoginResult?.Invoke(packet.Payload);
                    }
                    else if (packet.Type >= PacketType.CommandShutdown)
                    {
                        OnCommandReceived?.Invoke(packet.Type, packet.Payload);
                    }
                }
            }
            catch
            {
                // Any error during reading implies disconnection
                TriggerServerLost();
            }
        }

        /// <summary>
        /// Triggers the server connection loss sequence.
        /// Ensures cleanup logic runs only once to prevent loops.
        /// </summary>
        private void TriggerServerLost()
        {
            // === CRITICAL FIX: Return if already disconnected to prevent infinite recursion ===
            if (!_isConnected) return;

            // 1. Update state immediately
            _isConnected = false;

            // 2. Stop the heartbeat timer
            _heartbeatTimer?.Change(Timeout.Infinite, Timeout.Infinite);

            // 3. Clean up resources safely
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _stream = null;
            _client = null;

            // 4. Notify the application
            OnServerLost?.Invoke();
        }
    }
}