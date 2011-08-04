using System;
using System.Runtime.InteropServices;
using WindowsFormsApplication1.Interfaces;

namespace WindowsFormsApplication1.Features {
    public class Test : IFeature {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private readonly IntPtr _windowHandle;

        public Test() {
            _windowHandle = FindWindow("OMAIN", null);
        }

        public void Run() {
            WindowFocusSetter.MakeTopMost(_windowHandle);
            WindowSettingsSetter.DisableButtons(_windowHandle);
        }

        public void Clear() {
            WindowFocusSetter.MakeNormal(_windowHandle);
        }

        static class WindowSettingsSetter {
            [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
            private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);

            //[DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
            //private static extern int GetMenuItemCount(IntPtr hmenu);

            [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
            private static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);

            [DllImport("user32.dll", EntryPoint = "DrawMenuBar")]
            private static extern int DrawMenuBar(IntPtr hwnd);

            private const int MF_BYPOSITION = 0x0400;
            private const int MF_DISABLED = 0x0002;

            public static void DisableButtons(IntPtr window) {
                IntPtr hmenu = GetSystemMenu(window, 0);
                //int cnt = GetMenuItemCount(hmenu);

                for (int i = 8; i >= 0; i--)
                {
                    RemoveMenu(hmenu, i, 1024);//MF_DISABLED | MF_BYPOSITION);
                }

                DrawMenuBar(window);
            }
        }


        static class WindowFocusSetter {
            [DllImport("user32.dll")]
            static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,int Y, int cx, int cy, uint uFlags);
            
            [DllImport("user32.dll")]
            static extern IntPtr SetFocus(IntPtr hWnd);

            [DllImport("User32.dll")]
            static extern Int32 SetForegroundWindow(IntPtr hWnd);
            
            static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
            static readonly IntPtr HWND_TOP = new IntPtr(0);
            const UInt32 SWP_NOSIZE = 0x0001;
            const UInt32 SWP_NOMOVE = 0x0002;
            const UInt32 SWP_NOZORDER = 0x0004;
            const UInt32 SWP_NOREDRAW = 0x0008;
            const UInt32 SWP_NOACTIVATE = 0x0010;
            const UInt32 SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */
            const UInt32 SWP_SHOWWINDOW = 0x0040;
            const UInt32 SWP_HIDEWINDOW = 0x0080;
            const UInt32 SWP_NOCOPYBITS = 0x0100;
            const UInt32 SWP_NOOWNERZORDER = 0x0200; /* Don't do owner Z ordering */
            const UInt32 SWP_NOSENDCHANGING = 0x0400; /* Don't send WM_WINDOWPOSCHANGING */

            const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

            public static void MakeTopMost(IntPtr window) {
                //SetFocus(window);
                SetForegroundWindow(window);
                SetWindowPos(window, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }
            
            public static void MakeNormal(IntPtr window) {
                SetWindowPos(window, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }
        }
    }
}