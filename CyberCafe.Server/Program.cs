using System;
using System.Windows.Forms;
using CyberCafe.Core.Data;
using CyberCafe.Server.Views;

namespace CyberCafe.Server
{
    /// <summary>
    /// Provides the main entry point for the CyberCafe Server application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Initialize the database and test data on startup
                DatabaseManager.InitializeDatabase();
                DatabaseManager.SeedTestData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing Database: " + ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Important: Launch the main server form directly.
            // FormServer will spin up the networking listener and enforce the login screen (FormLogin)
            Application.Run(new FormServer());
        }
    }
}