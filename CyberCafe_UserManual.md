# Cyber Cafe Management System - User Manual

Welcome to the Cyber Cafe Management System. This guide provides detailed instructions on how to set up, launch, and operate the system components.

## Table of Contents
1. [Getting Started (Launch Process)](#1-getting-started)
2. [Administrator Capabilities](#2-administrator-capabilities)
3. [Cashier Capabilities](#3-cashier-capabilities)
4. [Using the Client Kiosk](#4-using-the-client-kiosk)

---

## 1. Getting Started

The Cyber Cafe system comprises two standalone applications: **Server** and **Client**. The Server MUST be running before any Client application initializes.

### Launching the Server
1. Start the `CyberCafe.Server.exe`.
2. Upon startup, the Server will automatically initialize the local SQLite database.
3. A login screen will immediately prompt you for credentials. 
   - **Default Admin Account:**
     - **Username:** `admin`
     - **Password:** `1234`
4. After logging in, the server background listener will begin accepting connections from clients on the network.

### Launching the Client
1. Install and start `CyberCafe.Client.exe` on each workstation.
2. The initial launch will capture the machine's MAC address and begin broadcasting to find the Server.
3. Upon discovering the Server, it will lock the screen, entering Kiosk Mode, which prevents standard user access to the desktop.
4. **Emergency Exit:** Press `Ctrl+Shift+Alt+X` to prompt the emergency exit dialog. Use the default admin password (`1234`) to close the Client and unlock the workstation.

---

## 2. Administrator Capabilities

Logging into the server with an `Admin` account unlocks full system capabilities.

### Employee Management
- Access the `Employees` section from the dashboard.
- Create new cashier accounts with restricted privileges.
- Reset passwords, modify display names, and dynamically activate or deactivate employee accounts.

### Voucher Management
- Navigate to the `Vouchers` section.
- **Generate:** Select the quantity, minutes, validity pool, and price to spawn prepaid voucher codes.
- **Search & Filter:** Find active, used, or expired vouchers visually in the database.
- **Delete:** Remove un-sold vouchers from the database.

### Reports & Analytics
- The `Reports` interface allows you to view daily earnings and employee performance metrics.
- Click **Print Daily Sales** or **Print Employee Report** to generate a printable HTML breakdown of the system statistics.

---

## 3. Cashier Capabilities

An employee logged in with a `Cashier` role has stripped-down limits designed strictly for operations.

- **Voucher Sales:** The cashier can enter a voucher code from the available pool and mark it as sold using the Quick Sell field.
- **Device Management:** Cashiers can monitor active clients through the devices grid.
- **Remote Operations:** Cashiers can remotely Lock, Restart, Shut Down, or Signal individual connected clients by right-clicking on their grid rows.

---

## 4. Using the Client Kiosk

When a user sits at a workstation, they will be greeted with the locked Kiosk UI.

1. **Purchasing Time:** The user must buy a prepaid voucher code from the front desk (Cashier).
2. **Logging In:** The user inputs the voucher code into the input field and presses **Login**.
3. **Session Active:** The application shrinks into a floating overlay widget. The overlay prominently displays the remaining time. 
4. **Time Warning:** The timer turns red when the session drops below 5 minutes.
5. **Logout:** The user can click **Logout** on the floating widget to pause their voucher time, immediately returning the machine to Kiosk mode.
