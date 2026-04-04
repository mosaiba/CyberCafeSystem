# Cyber Cafe Management System

A robust, full-featured Client-Server Cyber Cafe Management System built in C# using .NET 8.0 and Windows Forms (WinForms).

## 📖 Overview

The Cyber Cafe Management System is designed to help cafe administrators manage their computers, employees, and customers efficiently. It features a complete server dashboard for managing connections, generating vouchers, running reports, and a client application installed on the cafe's workstations to control usage access.

## ✨ Key Features

### 🖥️ Server Application
- **Server Dashboard**: Real-time monitoring and control of connected client computers. Control client sessions, lock/unlock screens, and send messages.
- **Authentication & Security**: Secure login system. Passwords are encrypted using `BCrypt.Net-Next`.
- **Employee Management**: Add, update, and remove staff accounts with different access privileges.
- **Voucher System**: Generate, track, and manage pre-paid access vouchers or time-based tickets for customers.
- **Reports & Analytics**: Generate detailed usage and financial reports to keep track of cafe earnings and statistics.
- **Built-in Database**: Uses a local, lightweight `SQLite` database for zero-config data persistence.

### 💻 Client Application
- **Access Control**: Locks the workstation screen until a valid session/voucher is activated.
- **Real-Time Communication**: Uses a custom asynchronous TCP networking protocol (`NetworkPacket`) to communicate seamlessly with the server.
- **Time Tracking**: Displays remaining time to the user.

## 🏗️ Project Structure

The project is structured as a single Visual Studio solution `CyberCafeSystem.sln` containing three main projects:

- **`CyberCafe.Core`**: A shared class library containing common data models (`DatabaseManager`) and network abstractions used by both Client and Server.
- **`CyberCafe.Server`**: The Server Windows Forms application handling administrative tasks, UI, and database connections.
- **`CyberCafe.Client`**: The Client Windows Forms application meant to run on customer workstations.

## 🛠️ Tech Stack & Dependencies

- **Framework**: [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **UI Framework**: Windows Forms (WinForms)
- **Database**: SQLite (`Microsoft.Data.Sqlite` ver 10.0.3)
- **Security**: `BCrypt.Net-Next` (ver 4.1.0)
- **Networking**: TCP Sockets

## 🚀 Getting Started

### Prerequisites

To build and run this project, you need:
- **Windows OS**
- **.NET 8.0 SDK**
- **Visual Studio 2022** (Recommended) or JetBrains Rider.

### Installation & Execution

1. **Get the source code**: Validate that you have the complete source for `CyberCafeSystem` locally.
2. **Open the Solution**:
   Open `CyberCafeSystem.sln` using Visual Studio or your preferred IDE.
3. **Restore Packages**:
   The IDE should automatically restore NuGet packages. If not, open the Package Manager Console and run:
   ```bash
   dotnet restore
   ```
4. **Compile & Run**:
   - Set **`CyberCafe.Server`** as the Startup Project and run it to launch the server dashboard.
   - Set **`CyberCafe.Client`** as the Startup Project (in another IDE instance or by configuring multiple startup projects) and run it to simulate client workstations. 

## 📝 License

This project is licensed under the terms of the `LICENSE.txt` file included in the root repository.
