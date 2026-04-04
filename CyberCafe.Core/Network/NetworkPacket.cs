namespace CyberCafe.Core.Network
{
    /// <summary>
    /// Defines the types of network packets that can be sent between the client and the server.
    /// </summary>
    public enum PacketType
    {
        /// <summary>
        /// A request from a client to log in using a voucher code.
        /// </summary>
        LoginRequest,

        /// <summary>
        /// A response from the server indicating the result of a login request.
        /// </summary>
        LoginResponse,

        /// <summary>
        /// A notification from the client that a session has ended or been logged out.
        /// </summary>
        Logout,

        /// <summary>
        /// A periodic pulse sent to maintain and monitor the connection status.
        /// </summary>
        Heartbeat,

        // === Remote Control Commands ===

        /// <summary>
        /// Command sent to a client to shut down the computer.
        /// </summary>
        CommandShutdown,

        /// <summary>
        /// Command sent to a client to restart the computer.
        /// </summary>
        CommandRestart,

        /// <summary>
        /// Command sent to a client to force an immediate session logout.
        /// </summary>
        CommandForceLogout,

        /// <summary>
        /// Command sent to a client to display a message on the screen.
        /// </summary>
        CommandMessage
    }

    /// <summary>
    /// Represents a data packet for network communication within the CyberCafe system.
    /// </summary>
    public class NetworkPacket
    {
        /// <summary>
        /// Gets or sets the type of the network packet.
        /// </summary>
        public PacketType Type { get; set; }

        /// <summary>
        /// Gets or sets the data payload associated with the packet.
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the sender (e.g., Device MAC Address).
        /// </summary>
        public string SenderId { get; set; }
    }
}