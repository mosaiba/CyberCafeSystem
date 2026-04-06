using System.Data;
using CyberCafe.Core.Data;

namespace CyberCafe.Server.Views
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            // ربط حدث الإغلاق يدوياً للتأكد من عمله
            this.FormClosing += FormLogin_FormClosing;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            DataTable result = DatabaseManager.ValidateEmployeeLogin(txtUsername.Text, txtPassword.Text);

            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                CurrentSession.EmployeeID = Convert.ToInt32(row["EmployeeID"]);
                CurrentSession.FullName = row["FullName"].ToString();
                CurrentSession.Role = Convert.ToInt32(row["Role"]);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password, or account is disabled.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.SelectAll();
            }
        }

        // الزر الآن فقط يطلب إغلاق النافذة، والحدث FormClosing يتولى الباقي
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // هذا الحدث هو المسؤول الوحيد عن قرار الإغلاق أو الإلغاء
        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // إذا تم تسجيل الدخول بنجاح، اسمح بالإغلاق دون أسئلة
            if (this.DialogResult == DialogResult.OK) return;

            // إذا كان السبب هو ضغط المستخدم على X أو زر الخروج (وليس إغلاق من الكود)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // إلغاء أمر الإغلاق مؤقتاً لنسأل المستخدم
                e.Cancel = true;

                DialogResult dr = MessageBox.Show(
                    "WARNING: This will shut down the server and disconnect all clients.\nAre you sure you want to exit?",
                    "Confirm Server Shutdown",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // إذا وافق، نلغي خاصية Cancel ونخرج من البرنامج
                    e.Cancel = false;
                    Application.Exit();
                }
                // إذا ضغط No، الـ e.Cancel يظل true، وبالتالي النافذة لن تغلق
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
            }
        }
    }
}