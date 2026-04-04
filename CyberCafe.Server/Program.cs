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
            // Configure application visual styles and high DPI settings
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Initialize the database and seed it with default data
                DatabaseManager.InitializeDatabase();
                DatabaseManager.SeedTestData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing Database: " + ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 1. Create and display the login screen
            FormLogin loginForm = new FormLogin();

            // 2. Check the login result
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // If login is successful, open the main management window
                Application.Run(new FormServer());
            }
            else
            {
                // If the user cancels or closes the login screen, exit the application
                Application.Exit();
            }
        }
    }
}