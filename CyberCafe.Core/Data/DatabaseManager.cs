using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace CyberCafe.Core.Data
{
    /// <summary>
    /// Manages all database operations for the CyberCafe system, including initialization, 
    /// employee management, voucher handling, and reporting.
    /// </summary>
    public class DatabaseManager
    {
        private const string DbFileName = "CyberCafeDB.db";
        private const string ConnectionString = "Data Source=" + DbFileName + ";";

        /// <summary>
        /// Initializes the database by creating the SQLite file and necessary tables if they do not exist.
        /// Also sets the journal mode to WAL for better concurrency.
        /// </summary>
        public static void InitializeDatabase()
        {
            // Ensure the database file exists
            if (!File.Exists(DbFileName))
            {
                using (var fs = File.Create(DbFileName)) { }
            }

            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();

                // Enable Write-Ahead Logging for better performance
                using (var cmd = new SqliteCommand("PRAGMA journal_mode=WAL;", conn))
                    cmd.ExecuteNonQuery();

                // Define SQL for creating tables
                string sqlEmployees = @"
                    CREATE TABLE IF NOT EXISTS Employees (
                        EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName NVARCHAR(100) NOT NULL,
                        Username VARCHAR(50) UNIQUE NOT NULL,
                        PasswordHash VARCHAR(256) NOT NULL,
                        Role TINYINT DEFAULT 1,
                        IsActive BOOLEAN DEFAULT 1
                    );";

                string sqlDevices = @"
                    CREATE TABLE IF NOT EXISTS Devices (
                        DeviceID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FriendlyName NVARCHAR(50),
                        MACAddress VARCHAR(17) UNIQUE NOT NULL,
                        LastIP VARCHAR(15),
                        Status TINYINT DEFAULT 0
                    );";

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
                        SaleDate DATETIME,
                        ExpiryDate DATETIME,
                        FOREIGN KEY(SoldByEmpID) REFERENCES Employees(EmployeeID)
                    );";

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

                string sqlTransactions = @"
                    CREATE TABLE IF NOT EXISTS Transactions (
                        TransID INTEGER PRIMARY KEY AUTOINCREMENT,
                        EmpID INT,
                        TransType VARCHAR(20),
                        Amount DECIMAL,
                        Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY(EmpID) REFERENCES Employees(EmployeeID)
                    );";

                // Execute table creation scripts
                ExecuteNonQuery(conn, sqlEmployees);
                ExecuteNonQuery(conn, sqlDevices);
                ExecuteNonQuery(conn, sqlVouchers);
                ExecuteNonQuery(conn, sqlSessions);
                ExecuteNonQuery(conn, sqlTransactions);
            }
        }

        /// <summary>
        /// Seeds the database with default test data if it doesn't already exist.
        /// Includes a default admin account and a test voucher.
        /// </summary>
        public static void SeedTestData()
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();

                // Seed default admin user
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

                // Seed default test voucher
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
        /// Executes a non-query SQL command using the provided connection.
        /// </summary>
        /// <param name="conn">The active SQLite connection.</param>
        /// <param name="sql">The SQL query string to execute.</param>
        private static void ExecuteNonQuery(SqliteConnection conn, string sql)
        {
            using (var cmd = new SqliteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Computes a SHA256 hash of the provided password string.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>A hexadecimal string representation of the hashed password.</returns>
        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // === Voucher Methods ===

        /// <summary>
        /// Adds a new voucher record to the database.
        /// </summary>
        /// <param name="code">The unique voucher code.</param>
        /// <param name="minutes">The total minutes allocated to the voucher.</param>
        /// <param name="price">The sale price of the voucher.</param>
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
        /// Retrieves a filtered list of vouchers based on status, date range, and price range.
        /// </summary>
        /// <param name="status">The status filter (e.g., "0" for Available, "1" for Active, etc.).</param>
        /// <param name="from">The start date for the creation date filter.</param>
        /// <param name="to">The end date for the creation date filter.</param>
        /// <param name="priceFrom">The minimum price filter.</param>
        /// <param name="priceTo">The maximum price filter.</param>
        /// <returns>A DataTable containing the filtered voucher records.</returns>
        public static DataTable GetFilteredVouchers(string status, DateTime? from, DateTime? to, decimal priceFrom, decimal priceTo)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT VoucherCode, TotalMinutes, Price, ValidityDays, Status, CreatedDate FROM Vouchers WHERE 1=1";

                // Apply status filter
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    if (status.Contains("0")) sql += " AND Status = 0";
                    else if (status.Contains("2")) sql += " AND Status = 2";
                    else if (status.Contains("3")) sql += " AND Status = 3";
                    else if (status.Contains("1")) sql += " AND Status = 1";
                }

                // Apply date filters
                if (from.HasValue) sql += " AND CreatedDate >= @from";
                if (to.HasValue) sql += " AND CreatedDate < @toNextDay";
                
                // Apply price filters
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
        /// Marks a voucher as sold and records the selling employee and sale date.
        /// </summary>
        /// <param name="code">The voucher code to sell.</param>
        /// <param name="empId">The ID of the employee performing the sale.</param>
        /// <returns>True if the voucher was successfully updated; otherwise, false.</returns>
        public static bool SellVoucher(string code, int empId)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                // Status 1 indicates sold/active. SaleDate is recorded in local time.
                string sql = "UPDATE Vouchers SET Status = 1, SoldByEmpID = @empId, SaleDate = @saleDate WHERE VoucherCode = @code AND Status = 0";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@empId", empId);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@saleDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Deletes an available (unsold) voucher from the database.
        /// </summary>
        /// <param name="code">The voucher code to delete.</param>
        /// <returns>True if the voucher was deleted; otherwise, false.</returns>
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
        /// Validates a voucher code for login. Updates the status and expiry date if it's the first use.
        /// </summary>
        /// <param name="code">The voucher code to validate.</param>
        /// <returns>A string starting with "SUCCESS:" followed by remaining minutes, or an error message.</returns>
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

                            // Status 2 means voucher is exhausted
                            if (status == 2) return "Error: Voucher already used.";
                            
                            // Status 3 (Paused) or 1 (Active/Sold) are considered valid for login
                            if (status == 3 || status == 1) return $"SUCCESS:{minutes}";

                            // Status 0 means created but not yet sold/activated
                            if (status == 0)
                            {
                                // Ensure the voucher has been paid for
                                if (reader["SoldByEmpID"] == DBNull.Value || reader["SoldByEmpID"] == null)
                                    return "Error: This voucher is not paid. Please see the cashier.";

                                // Calculate expiry date based on validity days
                                int validityDays = 30;
                                if (reader["ValidityDays"] != DBNull.Value) validityDays = Convert.ToInt32(reader["ValidityDays"]);
                                DateTime expiryDate = DateTime.Now.AddDays(validityDays);

                                // Update status to 1 (Active) and set expiry date
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
        /// Updates the remaining minutes for a voucher and updates its status if exhausted.
        /// </summary>
        /// <param name="code">The voucher code to update.</param>
        /// <param name="minutes">The new remaining minutes value.</param>
        public static void SetRemainingMinutes(string code, int minutes)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                // If minutes reach 0, set status to 2 (Exhausted), otherwise keep/set to 3 (Paused/Active)
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
        /// Pauses an active voucher session by updating its status.
        /// </summary>
        /// <param name="code">The voucher code to pause.</param>
        public static void PauseVoucher(string code)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                // Status 3 indicates a paused session
                string sql = "UPDATE Vouchers SET Status = 3 WHERE VoucherCode = @code AND Status = 1";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // === Employee Methods ===

        /// <summary>
        /// Validates employee login credentials.
        /// </summary>
        /// <param name="username">The employee's username.</param>
        /// <param name="password">The employee's plain-text password.</param>
        /// <returns>A DataTable containing the employee's ID, FullName, and Role if successful.</returns>
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
        /// Retrieves all employee records, optionally filtered by a search term.
        /// </summary>
        /// <param name="searchTerm">The term to search for in FullName or Username.</param>
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
        /// Adds a new employee to the database.
        /// </summary>
        /// <param name="name">Full name of the employee.</param>
        /// <param name="username">Unique username for the employee.</param>
        /// <param name="password">Plain-text password to be hashed.</param>
        /// <param name="role">Integer representing the employee's role.</param>
        /// <returns>True if the employee was added; false if the username already exists.</returns>
        public static bool AddEmployee(string name, string username, string password, int role)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                // Check if username is already taken
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
        /// Updates an existing employee's details.
        /// </summary>
        /// <param name="id">The unique ID of the employee.</param>
        /// <param name="name">The new full name.</param>
        /// <param name="role">The new role integer.</param>
        /// <param name="password">The new plain-text password (optional).</param>
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
        /// <param name="id">The unique ID of the employee.</param>
        /// <param name="isActive">The desired active status.</param>
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

        // === Device Methods ===

        /// <summary>
        /// Retrieves the friendly name of a device based on its MAC address.
        /// Returns the MAC address if no friendly name is found.
        /// </summary>
        /// <param name="macAddress">The MAC address of the device.</param>
        /// <returns>The friendly name or the MAC address.</returns>
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
        /// Saves or updates the friendly name associated with a device's MAC address.
        /// </summary>
        /// <param name="macAddress">The device's MAC address.</param>
        /// <param name="friendlyName">The friendly name to assign.</param>
        public static void SaveDeviceName(string macAddress, string friendlyName)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sqlUpdate = "UPDATE Devices SET FriendlyName = @name WHERE MACAddress = @mac";
                using (var cmd = new SqliteCommand(sqlUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@name", friendlyName);
                    cmd.Parameters.AddWithValue("@mac", macAddress);
                    int rows = cmd.ExecuteNonQuery();

                    // If device doesn't exist, insert it
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
        /// Generates a daily sales report for a specific date.
        /// </summary>
        /// <param name="date">The date for which to generate the report.</param>
        /// <returns>A DataTable containing sales summary (TotalCount, TotalPrice, TotalMinutes).</returns>
        public static DataTable GetDailySalesReport(DateTime date)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                // Filter by SaleDate to include only vouchers sold on the target day
                string sql = @"
            SELECT 
                COUNT(*) as TotalCount, 
                SUM(Price) as TotalPrice, 
                SUM(TotalMinutes) as TotalMinutes 
            FROM Vouchers 
            WHERE SoldByEmpID IS NOT NULL 
            AND date(SaleDate) = date(@date)";

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
        /// Generates a report showing the performance of each employee over a date range.
        /// </summary>
        /// <param name="from">The start date of the range.</param>
        /// <param name="to">The end date of the range.</param>
        /// <returns>A DataTable containing employee performance data (Employee, VoucherCount, TotalRevenue).</returns>
        public static DataTable GetEmployeePerformanceReport(DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                // Join Vouchers with Employees to calculate sales per employee
                string sql = @"
                    SELECT 
                        E.FullName as Employee, 
                        COUNT(V.VoucherCode) as VoucherCount, 
                        SUM(V.Price) as TotalRevenue 
                    FROM Vouchers V
                    JOIN Employees E ON V.SoldByEmpID = E.EmployeeID
                    WHERE V.SoldByEmpID IS NOT NULL
                    AND date(V.SaleDate) >= date(@from)
                    AND date(V.SaleDate) <= date(@to)
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