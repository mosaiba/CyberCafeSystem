# Cyber Cafe Management System

A robust, full-featured Client-Server Cyber Cafe Management System built in C# using .NET 8.0 and Windows Forms (WinForms).

## 📖 Overview

The Cyber Cafe Management System is designed to help cafe administrators manage their computers, employees, and customers efficiently. It features a complete server dashboard for managing connections, generating vouchers, running reports, and a client application installed on the cafe's workstations to control usage access.

## 📚 User Manual & Documentation

For detailed instructions on launching the system, managing user roles, and operating the cashier desk, please refer to the comprehensive [Cyber Cafe User Manual](./CyberCafe_UserManual.md).

## ✨ Key Features

### 🖥️ Server Application
- **Server Dashboard**: Real-time monitoring and control of connected client computers. Control client sessions, lock/unlock screens, and send remote commands.
- **Authentication & Security**: Secure role-based login system for Cashiers and Admins.
- **Employee Management**: Manage staff accounts with distinct access privileges.
- **Voucher System**: Generate, track, and sell pre-paid access vouchers.
- **Reports & Analytics**: Obtain insights using detailed usage and financial reports.
- **Built-in Database**: Uses a local, lightweight `SQLite` database for zero-config data persistence.

### 💻 Client Application
- **Access Control (Kiosk Mode)**: Locks the workstation securely until a valid session/voucher is activated. Built-in Task Manager termination.
- **Real-Time Communication**: Custom asynchronous TCP networking protocol for live instructions and heartbeat.
- **Time Tracking**: Floating desk-overlay displays the remaining time to the user gracefully.

## 🏗️ Project Structure

The project is structured as a single Visual Studio solution `CyberCafeSystem.sln` containing three main projects:

- **`CyberCafe.Core`**: Shared namespace library encapsulating data models, network abstractions, and data layer.
- **`CyberCafe.Server`**: The Server Windows Forms application handling administrative views and primary backend orchestration.
- **`CyberCafe.Client`**: The Client Windows Forms application intended for customer workstations.

## 🛠️ Tech Stack & Dependencies

- **Framework**: [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **UI Framework**: Windows Forms (WinForms)
- **Database**: SQLite (`Microsoft.Data.Sqlite` ver 10.0.3)
- **Security**: Native `.NET SHA256` Hashing
- **Networking**: Raw TCP Sockets

## 🚀 Getting Started

### Prerequisites

To build and run this project, you need:
- **Windows OS**
- **.NET 8.0 SDK**
- **Visual Studio 2022** (Recommended) or JetBrains Rider.

### Installation & Execution

1. **Open the Solution**: Load `CyberCafeSystem.sln` in Visual Studio.
2. **Restore Packages**: The IDE should automatically restore NuGet packages. If not, open the Package Manager Console and run `dotnet restore`.
3. **Compile & Run**:
   - Set **`CyberCafe.Server`** as the Startup Project and run it. *(Default Admin pass: `1234`)*
   - Set **`CyberCafe.Client`** as the Startup Project on secondary instances to simulate physical kiosk workstations. 

## 📝 License

This project is licensed under the terms of the `LICENSE.txt` file included in the root repository.
