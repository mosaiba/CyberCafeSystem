using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace CyberCafe.Core.Data
{
    /// <summary>
    /// Manages all database operations for the CyberCafe system using SQLite.
    /// </summary>
    public class DatabaseManager
    {
        private const string DbFileName = "CyberCafeDB.db";
        private const string ConnectionString = "Data Source=" + DbFileName + ";";

        /// <summary>
        /// Initializes the database by creating the file and required tables if they do not exist.
        /// </summary>
        public static void InitializeDatabase()
        {
            if (!File.Exists(DbFileName))
            {
                using (var fs = File.Create(DbFileName)) { }
            }

            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();

                // Enable Write-Ahead Logging (WAL) mode to prevent "Database is locked" errors
                using (var cmd = new SqliteCommand("PRAGMA journal_mode=WAL;", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // 1. Employees Table
                string sqlEmployees = @"
                    CREATE TABLE IF NOT EXISTS Employees (
                        EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName NVARCHAR(100) NOT NULL,
                        Username VARCHAR(50) UNIQUE NOT NULL,
                        PasswordHash VARCHAR(256) NOT NULL,
                        Role TINYINT DEFAULT 1,
                        IsActive BOOLEAN DEFAULT 1
                    );";

                // 2. Devices Table
                string sqlDevices = @"
                    CREATE TABLE IF NOT EXISTS Devices (
                        DeviceID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FriendlyName NVARCHAR(50),
                        MACAddress VARCHAR(17) UNIQUE NOT NULL,
                        LastIP VARCHAR(15),
                        Status TINYINT DEFAULT 0
                    );";

                // 3. Vouchers Table
                string sqlVouchers = @"
                    CREATE TABLE IF NOT EXISTS Vouchers (
                        VoucherCode VARCHAR(10) PRIMARY KEY,
                        TotalMinutes INT NOT NULL,
                        RemainingMinutes INT NOT NULL,
                        Status TINYINT DEFAULT 0,
                        Price DECIMAL(10,2) DEFAULT 0,
                        ValidityDays INT DEFAULT 30,
                        SoldByEmpID INT,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        ExpiryDate DATETIME,
                        FOREIGN KEY(SoldByEmpID) REFERENCES Employees(EmployeeID)
                    );";

                // 4. Sessions Table (for records)
                string sqlSessions = @"
                    CREATE TABLE IF NOT EXISTS Sessions (
                        SessionID INTEGER PRIMARY KEY AUTOINCREMENT,
                        DeviceID INT,
                        LoginType TINYINT,
                        Identifier VARCHAR(50),
                        StartTime DATETIME,
                        EndTime DATETIME,
                        DurationSec INT,
                        Cost DECIMAL,
                        FOREIGN KEY(DeviceID) REFERENCES Devices(DeviceID)
                    );";

                // 5. Financial Transactions Table
                string sqlTransactions = @"
                    CREATE TABLE IF NOT EXISTS Transactions (
                        TransID INTEGER PRIMARY KEY AUTOINCREMENT,
                        EmpID INT,
                        TransType VARCHAR(20),
                        Amount DECIMAL,
                        Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY(EmpID) REFERENCES Employees(EmployeeID)
                    );";

                ExecuteNonQuery(conn, sqlEmployees);
                ExecuteNonQuery(conn, sqlDevices);
                ExecuteNonQuery(conn, sqlVouchers);
                ExecuteNonQuery(conn, sqlSessions);
                ExecuteNonQuery(conn, sqlTransactions);
            }
        }

        /// <summary>
        /// Seeds the database with default test data if it's not already present.
        /// </summary>
        public static void SeedTestData()
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();

                // 1. Add default administrator (admin / 1234)
                string checkEmp = "SELECT COUNT(*) FROM Employees WHERE Username = 'admin'";
                using (var checkCmd = new SqliteCommand(checkEmp, conn))
                {
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                    {
                        string insertEmp = "INSERT INTO Employees (FullName, Username, PasswordHash, Role) VALUES ('System Admin', 'admin', @pass, 0)";
                        using (var cmdIns = new SqliteCommand(insertEmp, conn))
                        {
                            cmdIns.Parameters.AddWithValue("@pass", HashPassword("1234"));
                            cmdIns.ExecuteNonQuery();
                        }
                    }
                }

                // 2. Add test voucher
                string checkSql = "SELECT count(*) FROM Vouchers WHERE VoucherCode = 'TEST123'";
                using (var checkCmd = new SqliteCommand(checkSql, conn))
                {
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                    {
                        string insertSql = "INSERT INTO Vouchers (VoucherCode, TotalMinutes, RemainingMinutes, Price, ValidityDays, Status) VALUES ('TEST123', 60, 60, 5, 30, 0)";
                        using (var insertCmd = new SqliteCommand(insertSql, conn))
                        {
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes a non-query SQL command using an existing connection.
        /// </summary>
        /// <param name="conn">The SQLite connection to use.</param>
        /// <param name="sql">The SQL command string.</param>
        private static void ExecuteNonQuery(SqliteConnection conn, string sql)
        {
            using (var cmd = new SqliteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Computes a SHA-256 hash of a string.
        /// </summary>
        /// <param name="password">The string to hash.</param>
        /// <returns>A hexadecimal string representation of the hash.</returns>
        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // === Voucher Management Methods ===

        /// <summary>
        /// Adds a new voucher to the database.
        /// </summary>
        /// <param name="code">The unique voucher code.</param>
        /// <param name="minutes">The total minutes allowed by the voucher.</param>
        /// <param name="price">The price of the voucher.</param>
        /// <param name="validityDays">The number of days the voucher remains valid after activation.</param>
        public static void AddVoucher(string code, int minutes, decimal price, int validityDays)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Vouchers (VoucherCode, TotalMinutes, RemainingMinutes, Price, ValidityDays, Status) VALUES (@code, @min, @min, @price, @validity, 0)";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@min", minutes);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@validity", validityDays);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Retrieves vouchers filtered by various criteria.
        /// </summary>
        /// <param name="status">The status filter string.</param>
        /// <param name="from">The start date for filtering creation date.</param>
        /// <param name="to">The end date for filtering creation date.</param>
        /// <param name="priceFrom">The minimum price filter.</param>
        /// <param name="priceTo">The maximum price filter.</param>
        /// <returns>A DataTable containing the filtered vouchers.</returns>
        public static DataTable GetFilteredVouchers(string status, DateTime? from, DateTime? to, decimal priceFrom, decimal priceTo)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT VoucherCode, TotalMinutes, Price, ValidityDays, Status, CreatedDate FROM Vouchers WHERE 1=1";

                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    if (status.Contains("0")) sql += " AND Status = 0";
                    else if (status.Contains("2")) sql += " AND Status = 2";
                    else if (status.Contains("3")) sql += " AND Status = 3";
                    else if (status.Contains("1")) sql += " AND Status = 1";
                }

                if (from.HasValue) sql += " AND CreatedDate >= @from";
                if (to.HasValue) sql += " AND CreatedDate < @toNextDay";
                sql += " AND Price >= @pFrom AND Price <= @pTo";
                sql += " ORDER BY CreatedDate DESC";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    if (from.HasValue) cmd.Parameters.AddWithValue("@from", from.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (to.HasValue)
                    {
                        DateTime nextDay = to.Value.AddDays(1);
                        cmd.Parameters.AddWithValue("@toNextDay", nextDay.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    cmd.Parameters.AddWithValue("@pFrom", priceFrom);
                    cmd.Parameters.AddWithValue("@pTo", priceTo);

                    using (var reader = cmd.ExecuteReader()) dt.Load(reader);
                }
            }
            return dt;
        }

        /// <summary>
        /// Marks a voucher as sold by an employee.
        /// </summary>
        /// <param name="code">The voucher code to sell.</param>
        /// <param name="empId">The ID of the employee selling the voucher.</param>
        /// <returns>True if the voucher was successfully sold, otherwise false.</returns>
        public static bool SellVoucher(string code, int empId)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "UPDATE Vouchers SET SoldByEmpID = @empId WHERE VoucherCode = @code AND Status = 0 AND SoldByEmpID IS NULL";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@empId", empId);
                    cmd.Parameters.AddWithValue("@code", code);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Deletes a voucher from the database if it hasn't been used or sold.
        /// </summary>
        /// <param name="code">The voucher code to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public static bool DeleteVoucher(string code)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Vouchers WHERE VoucherCode = @code AND Status = 0";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@code", code);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Validates a voucher code and prepares it for activation.
        /// </summary>
        /// <param name="code">The voucher code to validate.</param>
        /// <returns>A status string starting with SUCCESS if valid, or an error message.</returns>
        public static string ValidateVoucher(string code)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Vouchers WHERE VoucherCode = @code";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@code", code);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int status = Convert.ToInt32(reader["Status"]);
                            int minutes = Convert.ToInt32(reader["RemainingMinutes"]);

                            if (status == 2) return "Error: Voucher already used.";
                            if (status == 3 || status == 1) return $"SUCCESS:{minutes}";

                            if (status == 0)
                            {
                                if (reader["SoldByEmpID"] == DBNull.Value || reader["SoldByEmpID"] == null)
                                    return "Error: This voucher is not paid. Please see the cashier.";

                                int validityDays = 30;
                                if (reader["ValidityDays"] != DBNull.Value) validityDays = Convert.ToInt32(reader["ValidityDays"]);
                                DateTime expiryDate = DateTime.Now.AddDays(validityDays);

                                string updateSql = "UPDATE Vouchers SET Status = 1, ExpiryDate = @exp WHERE VoucherCode = @code";
                                using (var updateCmd = new SqliteCommand(updateSql, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@exp", expiryDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    updateCmd.Parameters.AddWithValue("@code", code);
                                    updateCmd.ExecuteNonQuery();
                                }
                                return $"SUCCESS:{minutes}";
                            }
                        }
                    }
                }
            }
            return "Error: Invalid Voucher Code.";
        }

        /// <summary>
        /// Updates the remaining minutes for a specific voucher.
        /// </summary>
        /// <param name="code">The voucher code to update.</param>
        /// <param name="minutes">The new remaining minutes.</param>
        public static void SetRemainingMinutes(string code, int minutes)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = minutes > 0
                    ? "UPDATE Vouchers SET RemainingMinutes = @min, Status = 3 WHERE VoucherCode = @code"
                    : "UPDATE Vouchers SET RemainingMinutes = 0, Status = 2 WHERE VoucherCode = @code";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@min", minutes);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Pauses an active voucher session, typically due to network disconnection.
        /// </summary>
        /// <param name="code">The voucher code to pause.</param>
        public static void PauseVoucher(string code)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                // Set status to Paused (3) if it was Active (1)
                string sql = "UPDATE Vouchers SET Status = 3 WHERE VoucherCode = @code AND Status = 1";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // === Employee Management Methods ===

        /// <summary>
        /// Validates employee credentials for login.
        /// </summary>
        /// <param name="username">The employee's username.</param>
        /// <param name="password">The employee's password.</param>
        /// <returns>A DataTable containing employee details if login is successful.</returns>
        public static DataTable ValidateEmployeeLogin(string username, string password)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT EmployeeID, FullName, Role FROM Employees WHERE Username = @user AND PasswordHash = @pass AND IsActive = 1";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", HashPassword(password));
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all employees, optionally filtered by a search term.
        /// </summary>
        /// <param name="searchTerm">An optional search term for filtering by name or username.</param>
        /// <returns>A DataTable containing employee information.</returns>
        public static DataTable GetAllEmployees(string searchTerm = null)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT EmployeeID, FullName, Username, Role, IsActive FROM Employees WHERE 1=1";
                if (!string.IsNullOrEmpty(searchTerm)) sql += " AND (FullName LIKE @term OR Username LIKE @term)";
                sql += " ORDER BY FullName";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(searchTerm)) cmd.Parameters.AddWithValue("@term", $"%{searchTerm}%");
                    using (var reader = cmd.ExecuteReader()) dt.Load(reader);
                }
            }
            return dt;
        }

        /// <summary>
        /// Adds a new employee to the system.
        /// </summary>
        /// <param name="name">The full name of the employee.</param>
        /// <param name="username">The employee's unique username.</param>
        /// <param name="password">The employee's password.</param>
        /// <param name="role">The employee's role level.</param>
        /// <returns>True if the employee was added successfully, false if the username already exists.</returns>
        public static bool AddEmployee(string name, string username, string password, int role)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string checkSql = "SELECT COUNT(*) FROM Employees WHERE Username = @user";
                using (var checkCmd = new SqliteCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@user", username);
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0) return false;
                }

                string sql = "INSERT INTO Employees (FullName, Username, PasswordHash, Role, IsActive) VALUES (@name, @user, @pass, @role, 1)";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", HashPassword(password));
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        /// <summary>
        /// Updates the details of an existing employee.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="name">The new full name.</param>
        /// <param name="role">The new role level.</param>
        /// <param name="password">The new password (optional).</param>
        public static void UpdateEmployee(int id, string name, int role, string password = null)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = !string.IsNullOrEmpty(password) ?
                    "UPDATE Employees SET FullName = @name, Role = @role, PasswordHash = @pass WHERE EmployeeID = @id" :
                    "UPDATE Employees SET FullName = @name, Role = @role WHERE EmployeeID = @id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@role", role);
                    if (!string.IsNullOrEmpty(password)) cmd.Parameters.AddWithValue("@pass", HashPassword(password));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Toggles the active status of an employee.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="isActive">The new active status.</param>
        public static void ToggleEmployeeStatus(int id, bool isActive)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "UPDATE Employees SET IsActive = @status WHERE EmployeeID = @id";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@status", isActive ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // === Device Management Methods ===

        /// <summary>
        /// Retrieves the friendly name of a device based on its MAC address.
        /// </summary>
        /// <param name="macAddress">The MAC address of the device.</param>
        /// <returns>The friendly name of the device, or the MAC address if no name is assigned.</returns>
        public static string GetDeviceName(string macAddress)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT FriendlyName FROM Devices WHERE MACAddress = @mac";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@mac", macAddress);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value && !string.IsNullOrEmpty(result.ToString()))
                    {
                        return result.ToString();
                    }
                }
            }
            return macAddress;
        }

        /// <summary>
        /// Saves or updates the friendly name of a device.
        /// </summary>
        /// <param name="macAddress">The MAC address of the device.</param>
        /// <param name="friendlyName">The friendly name to assign to the device.</param>
        public static void SaveDeviceName(string macAddress, string friendlyName)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                // Attempt to update existing device
                string sqlUpdate = "UPDATE Devices SET FriendlyName = @name WHERE MACAddress = @mac";
                using (var cmd = new SqliteCommand(sqlUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@name", friendlyName);
                    cmd.Parameters.AddWithValue("@mac", macAddress);
                    int rows = cmd.ExecuteNonQuery();

                    // If no rows updated, insert as a new device
                    if (rows == 0)
                    {
                        string sqlInsert = "INSERT INTO Devices (MACAddress, FriendlyName, Status) VALUES (@mac, @name, 1)";
                        using (var cmdIns = new SqliteCommand(sqlInsert, conn))
                        {
                            cmdIns.Parameters.AddWithValue("@mac", macAddress);
                            cmdIns.Parameters.AddWithValue("@name", friendlyName);
                            cmdIns.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        // === Reporting Methods ===

        /// <summary>
        /// Generates a report of daily sales for a specific date.
        /// </summary>
        /// <param name="date">The date for the report.</param>
        /// <returns>A DataTable containing sales summary data.</returns>
        public static DataTable GetDailySalesReport(DateTime date)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        COUNT(*) as TotalCount, 
                        SUM(Price) as TotalPrice, 
                        SUM(TotalMinutes) as TotalMinutes 
                    FROM Vouchers 
                    WHERE SoldByEmpID IS NOT NULL 
                    AND date(CreatedDate) = date(@date)";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Generates an employee performance report for a specific date range.
        /// </summary>
        /// <param name="from">The start date for the range.</param>
        /// <param name="to">The end date for the range.</param>
        /// <returns>A DataTable containing employee performance statistics.</returns>
        public static DataTable GetEmployeePerformanceReport(DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        E.FullName as Employee, 
                        COUNT(V.VoucherCode) as VoucherCount, 
                        SUM(V.Price) as TotalRevenue 
                    FROM Vouchers V
                    JOIN Employees E ON V.SoldByEmpID = E.EmployeeID
                    WHERE V.SoldByEmpID IS NOT NULL
                    AND date(V.CreatedDate) >= date(@from)
                    AND date(V.CreatedDate) <= date(@to)
                    GROUP BY E.FullName";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-dd"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
    }
}