using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MsAccessRestrictor {
    static class WinApi {
        //
        // API Calls
        //

        //
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        public static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);
        //
        [DllImport("user32.dll")]
        public static extern UIntPtr GetMenuItemID(IntPtr hMenu, int nPos);
        //
        [DllImport("user32.dll")]
        public static extern int GetMenuString(IntPtr hMenu, uint uIdItem, [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpString, int nMaxCount, uint uFlag);
        //
        [DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
        public static extern int GetMenuItemCount(IntPtr hmenu);
        //
        [DllImport("user32.dll")]
        public static extern bool ModifyMenu(IntPtr hMnu, uint uPosition, uint uFlags, UIntPtr uIdNewItem, string lpNewItem);
        // Set window top most
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        // Set focus
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(IntPtr hWnd);
        // Return window's handle
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //
        [DllImport("user32", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, int hMod, int dwThreadId);
        //
        [DllImport("user32", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UnhookWindowsHookEx(int hHook);
        public delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref HookStruct lParam);
        //
        [DllImport("user32", EntryPoint = "CallNextHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref HookStruct lParam);

        //
        // Overloads on WinAPI methods
        //

        // Sets focus on a window and makes it always on top
        public static void SetTopMostWindow(IntPtr window, bool enable) {
            SetWindowPos(window, enable ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            SetForegroundWindow(window);
        }

        //
        // Constants
        //

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        //
        // Structs
        //

        #pragma warning disable 649, 169
        public struct HookStruct {
            public int VkCode;
            private int _scanCode;
            public int Flags;
            private int _time;
            private int _dwExtraInfo;
        }
        #pragma warning restore 649, 169
    }
}
