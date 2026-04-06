using System;
using System.Windows.Forms;
using CyberCafe.Core.Data;
using CyberCafe.Server.Views;

namespace CyberCafe.Server
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // تهيئة قاعدة البيانات
                DatabaseManager.InitializeDatabase();
                DatabaseManager.SeedTestData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing Database: " + ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // التعديل الأهم: تشغيل الفورم الرئيسي (FormServer) مباشرة
            // FormServer سيقوم بدوره بتشغيل السيرفر ثم استدعاء شاشة الدخول (FormLogin)
            Application.Run(new FormServer());
        }
    }
}