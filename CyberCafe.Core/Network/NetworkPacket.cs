namespace CyberCafe.Core.Network
{
    /// <summary>
    /// Defines the types of network packets that can be exchanged between the client and the server.
    /// This enumeration identifies the purpose of each communication message.
    /// </summary>
    public enum PacketType
    {
        /// <summary>
        /// A request sent from a client to the server to initiate a session using a voucher code.
        /// Payload typically contains the voucher code.
        /// </summary>
        LoginRequest,

        /// <summary>
        /// A response sent from the server to the client with the result of a login request.
        /// Payload typically contains "SUCCESS:Minutes" or an error message.
        /// </summary>
        LoginResponse,

        /// <summary>
        /// A notification sent from the client to the server indicating that a session has ended or the user logged out.
        /// </summary>
        Logout,

        /// <summary>
        /// A periodic signal sent between client and server to confirm the connection is still active.
        /// </summary>
        Heartbeat,

        // === Remote Control Commands ===

        /// <summary>
        /// An administrative command sent from the server to a client to shut down the operating system.
        /// </summary>
        CommandShutdown,

        /// <summary>
        /// An administrative command sent from the server to a client to restart the operating system.
        /// </summary>
        CommandRestart,

        /// <summary>
        /// An administrative command sent from the server to a client to immediately terminate the current session.
        /// </summary>
        CommandForceLogout,

        /// <summary>
        /// An administrative command sent from the server to a client to display a pop-up message to the user.
        /// Payload contains the message text.
        /// </summary>
        CommandMessage
    }

    /// <summary>
    /// Represents a structured data packet used for network communication within the CyberCafe ecosystem.
    /// Encapsulates the packet type, payload data, and sender identification.
    /// </summary>
    public class NetworkPacket
    {
        /// <summary>
        /// Gets or sets the type of the network packet, determining how the payload should be interpreted.
        /// </summary>
        public PacketType Type { get; set; }

        /// <summary>
        /// Gets or sets the data payload of the packet. The content format depends on the <see cref="Type"/>.
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the sender. 
        /// For clients, this is usually the hardware MAC address.
        /// </summary>
        public string SenderId { get; set; }
    }
}