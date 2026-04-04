using System.Net.Sockets;

namespace CyberCafe.Server
{
    /// <summary>
    /// Represents a device connected to the server, containing session and connection details.
    /// </summary>
    public class ConnectedDevice
    {
        /// <summary>
        /// Gets or sets the TcpClient associated with the connection.
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Gets or sets the unique hardware identifier (MAC address) of the device.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets a human-readable friendly name for the device (e.g., PC-01).
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the voucher code currently active on the device, if any.
        /// </summary>
        public string CurrentCode { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last received heartbeat pulse from the device.
        /// </summary>
        public DateTime LastHeartbeat { get; set; }

        /// <summary>
        /// Gets a value indicating whether the device is currently in an active user session.
        /// </summary>
        public bool IsInSession => !string.IsNullOrEmpty(CurrentCode);
    }
}
