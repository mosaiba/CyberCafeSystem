namespace CyberCafe.Client
{
    /// <summary>
    /// Provides the main entry point for the CyberCafe Client application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize application configuration, including high DPI and default font settings.
            ApplicationConfiguration.Initialize();
            Application.Run(new FormClient());
        }
    }
}