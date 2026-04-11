using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using CyberCafe.Core.Network;
using System.Net.NetworkInformation; // Added for network interface scanning

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

        private string _serverIp = "";
        private int _port = 13000;
        private int _discoveryPort = 13001;

        private System.Threading.Timer _heartbeatTimer;
        private string _myDeviceId = "CLIENT-01";
        private readonly object _lock = new object();
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
        /// Uses both Global Broadcast and Directed Broadcast for better reliability.
        /// </summary>
        /// <returns>True if a server responded and a valid IP was captured, otherwise False.</returns>
        public bool DiscoverServer()
        {
            using (UdpClient udp = new UdpClient())
            {
                try
                {
                    udp.EnableBroadcast = true;
                    udp.Client.ReceiveTimeout = 3000; // 3 seconds timeout

                    string request = "CYBERCAFE_DISCOVERY_REQUEST";
                    byte[] data = Encoding.UTF8.GetBytes(request);

                    // STRATEGY 1: Global Broadcast (Standard)
                    // Sends to 255.255.255.255
                    udp.Send(data, data.Length, "255.255.255.255", _discoveryPort);

                    // STRATEGY 2: Directed Broadcast (Robust)
                    // Calculates the broadcast address for the specific subnet (e.g., 192.168.1.255)
                    // This is often necessary as some routers block global broadcasts between devices.
                    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (ni.OperationalStatus == OperationalStatus.Up &&
                            ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    try
                                    {
                                        byte[] ipBytes = ip.Address.GetAddressBytes();
                                        byte[] maskBytes = ip.IPv4Mask.GetAddressBytes();

                                        if (maskBytes != null && ipBytes.Length == maskBytes.Length)
                                        {
                                            byte[] broadcastBytes = new byte[4];
                                            for (int i = 0; i < 4; i++)
                                            {
                                                // Broadcast = IP OR (NOT Mask)
                                                broadcastBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
                                            }

                                            IPAddress broadcastAddress = new IPAddress(broadcastBytes);
                                            // Send to the calculated subnet broadcast address
                                            udp.Send(data, data.Length, broadcastAddress.ToString(), _discoveryPort);
                                        }
                                    }
                                    catch { /* Ignore calculation errors */ }
                                }
                            }
                        }
                    }

                    // Block thread pending UDP response envelope
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] response = udp.Receive(ref remoteEP);
                    string respString = Encoding.UTF8.GetString(response);

                    if (respString.StartsWith("CYBERCAFE_SERVER:"))
                    {
                        string[] parts = respString.Split(':');
                        if (parts.Length >= 3)
                        {
                            _serverIp = parts[1];
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
            if (string.IsNullOrEmpty(_serverIp)) return false;
            if (_isConnected) return true;

            try
            {
                _client = new TcpClient();
                _client.Connect(_serverIp, _port);
                _stream = _client.GetStream();

                _isConnected = true;

                OnConnected?.Invoke();

                Thread listenThread = new Thread(ListenForServer);
                listenThread.IsBackground = true;
                listenThread.Start();

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

        private void HeartbeatCallback(object state)
        {
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

                if (!SendPacketInternal(heartbeat))
                {
                    TriggerServerLost();
                }
            }
            catch
            {
                TriggerServerLost();
            }
        }

        public void SendPacket(NetworkPacket packet)
        {
            if (!_isConnected) return;

            packet.SenderId = _myDeviceId;
            if (!SendPacketInternal(packet))
            {
                TriggerServerLost();
            }
        }

        private bool SendPacketInternal(NetworkPacket packet)
        {
            if (_stream == null || !_stream.CanWrite) return false;

            try
            {
                string json = JsonConvert.SerializeObject(packet);
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                lock (_lock)
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

        private void ListenForServer()
        {
            byte[] buffer = new byte[1024];
            int byteCount;

            try
            {
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
                TriggerServerLost();
            }
        }

        private void TriggerServerLost()
        {
            if (!_isConnected) return;

            _isConnected = false;
            _heartbeatTimer?.Change(Timeout.Infinite, Timeout.Infinite);

            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _stream = null;
            _client = null;

            OnServerLost?.Invoke();
        }
    }
}