namespace CyberCafe.Server
{
    /// <summary>
    /// Static class used to store and manage the current user session data for the application.
    /// </summary>
    public static class CurrentSession
    {
        /// <summary>
        /// Gets or sets the ID of the currently logged-in employee.
        /// </summary>
        public static int EmployeeID { get; set; }

        /// <summary>
        /// Gets or sets the full name of the currently logged-in employee.
        /// </summary>
        public static string FullName { get; set; }

        /// <summary>
        /// Gets or sets the role of the current user (e.g., 0 for Admin, 1 for Cashier).
        /// </summary>
        public static int Role { get; set; }

        /// <summary>
        /// Determines whether the current user has administrative privileges.
        /// </summary>
        /// <returns>True if the user is an administrator, otherwise false.</returns>
        public static bool IsAdmin()
        {
            return Role == 0;
        }
    }
}