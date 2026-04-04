using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CyberCafe.Client.Helpers
{
    /// <summary>
    /// Provides a global keyboard hook to monitor and optionally block specific key combinations for kiosk mode security.
    /// </summary>
    public class KeyboardHook : IDisposable
    {
        // Import necessary functions from user32.dll for Windows hooking
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // Import function to check the current state of a key (Async Key State)
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        /// <summary>
        /// Gets or sets a value indicating whether the keyboard blocking logic is active.
        /// </summary>
        public bool IsBlockingActive { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the KeyboardHook class and sets up the global hook.
        /// </summary>
        public KeyboardHook()
        {
            _proc = HookCallback;
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// Releases the resources used by the KeyboardHook class, specifically unhooking the keyboard.
        /// </summary>
        public void Dispose()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Callback function for the keyboard hook.
        /// </summary>
        /// <param name="nCode">A code the hook procedure uses to determine how to process the message.</param>
        /// <param name="wParam">The identifier of the keyboard message.</param>
        /// <param name="lParam">A pointer to a KBDLLHOOKSTRUCT structure containing details about the key press.</param>
        /// <returns>An IntPtr result based on the processing of the keyboard event.</returns>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // If blocking is not active, pass the key event to the next hook in the chain.
            if (!IsBlockingActive)
            {
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            // Blocking logic
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                bool isBlocked = false;

                // 1. Block Windows Key (Left and Right)
                if (key == Keys.LWin || key == Keys.RWin) isBlocked = true;

                // 2. Block Alt+Tab
                bool isAltDown = (GetAsyncKeyState(Keys.Menu) & 0x8000) != 0;
                if (key == Keys.Tab && isAltDown) isBlocked = true;

                // 3. Block Ctrl+Esc
                bool isCtrlDown = (GetAsyncKeyState(Keys.ControlKey) & 0x8000) != 0;
                if (key == Keys.Escape && isCtrlDown) isBlocked = true;

                // 4. Block Alt+F4
                if (key == Keys.F4 && isAltDown) isBlocked = true;

                // 5. Block Ctrl+Shift+Esc (Task Manager)
                bool isShiftDown = (GetAsyncKeyState(Keys.ShiftKey) & 0x8000) != 0;
                if (key == Keys.Escape && isCtrlDown && isShiftDown) isBlocked = true;

                if (isBlocked)
                {
                    // Block the key event from reaching the system.
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}