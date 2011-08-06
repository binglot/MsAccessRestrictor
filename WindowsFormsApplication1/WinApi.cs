using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsFormsApplication1 {
    static class WinApi {
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
    }
}
