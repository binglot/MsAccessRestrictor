using System;
using System.Runtime.InteropServices;
using System.Text;
using MsAccessRestrictor.Features;

namespace MsAccessRestrictor {
    class WinApi {
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
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        // Return if an application is minimized
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);
        //
        //public static extern IntPtr CreateJobObject(object a, string lpName);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateJobObject([In] ref SecurityAttributes lpJobAttributes, string lpName);
        //
        [DllImport("kernel32.dll")]
        public static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);
        //
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);
        //
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

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
