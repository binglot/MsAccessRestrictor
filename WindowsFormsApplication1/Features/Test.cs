using System;
using System.Runtime.InteropServices;
using System.Text;
using WindowsFormsApplication1.Interfaces;

namespace WindowsFormsApplication1.Features {
    public class Test : IFeature {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private const string MsAccessClassName = "OMAIN";

        public void Run() {
            var windowHandle = FindMsAccess();
            WindowFocusSetter.MakeTopMost(windowHandle);
            WindowSettingsSetter.DisableButtons(windowHandle);
        }

        public void Clear() {
            var windowHandle = FindMsAccess();
            WindowFocusSetter.MakeNormal(FindMsAccess());
            WindowSettingsSetter.EnableButtons(windowHandle);
        }

        private static IntPtr FindMsAccess() {
            return FindWindow(MsAccessClassName, null);
        }

        private static class WindowSettingsSetter {
            [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
            private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);
            [DllImport("user32.dll")]
            static extern bool ModifyMenu(IntPtr hMnu, uint uPosition, uint uFlags, UIntPtr uIdNewItem, string lpNewItem);
            [DllImport("user32.dll")]
            static extern UIntPtr GetMenuItemID(IntPtr hMenu, int nPos);
            [DllImport("user32.dll")]
            static extern int GetMenuString(IntPtr hMenu, uint uIdItem, 
                [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpString, int nMaxCount, uint uFlag);
            [DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
            private static extern int GetMenuItemCount(IntPtr hmenu);

            const UInt32 MF_ENABLED = 0x00000000;
            const UInt32 MF_GRAYED = 0x00000001;
            const int MF_BYPOSITION = 0x00000400;
            const int MF_BYCOMMAND = 0x00000000;
            //const UInt32 MF_DISABLED = 0x00000002;

            // needs changing! can't be hard-coded
            private static readonly UIntPtr[] OriginalButtonIds = new UIntPtr[8];

            public static void DisableButtons(IntPtr window) {
                EnableMenuButtons(window, false);
            }

            public static void EnableButtons(IntPtr window) {
                EnableMenuButtons(window, true);
            }



            static void EnableMenuButtons(IntPtr window, bool enabled) {
                var label = new StringBuilder();
                var systemMenu = GetSystemMenu(window, 0);
                var flags = MF_BYCOMMAND | (enabled ? MF_ENABLED : MF_GRAYED);
                var itemsCount = GetMenuItemCount(systemMenu);

                if (itemsCount > 0) {
                    for (int i = itemsCount - 1; i >= 0; i--) {
                        GetMenuString(systemMenu, (uint)i, label, label.Capacity, MF_BYPOSITION);

                        // To skip the menu separator
                        if (label.ToString() == "") {
                            continue;
                        }

                        uint sourceId;
                        UIntPtr destId;

                        if (enabled) {
                            sourceId = UInt32.MaxValue - (uint)i;
                            destId = OriginalButtonIds[i];
                        }
                        else {
                            var id = GetMenuItemID(systemMenu, i);
                            OriginalButtonIds[i] = id;
                            sourceId = id.ToUInt32();
                            destId = new UIntPtr(UInt32.MaxValue - (uint)i);
                        }

                        ModifyMenu(systemMenu, sourceId, flags, destId, label.ToString());
                    }
                }

            }
        }


        static class WindowFocusSetter {
            // Make top most window
            [DllImport("user32.dll")]
            static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

            // Set focus
            [DllImport("User32.dll")]
            static extern Int32 SetForegroundWindow(IntPtr hWnd);

            static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
            //static readonly IntPtr HWND_TOP = new IntPtr(0);
            const UInt32 SWP_NOSIZE = 0x0001;
            const UInt32 SWP_NOMOVE = 0x0002;
            //const UInt32 SWP_NOZORDER = 0x0004;
            //const UInt32 SWP_NOREDRAW = 0x0008;
            //const UInt32 SWP_NOACTIVATE = 0x0010;
            //const UInt32 SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */
            //const UInt32 SWP_SHOWWINDOW = 0x0040;
            //const UInt32 SWP_HIDEWINDOW = 0x0080;
            //const UInt32 SWP_NOCOPYBITS = 0x0100;
            //const UInt32 SWP_NOOWNERZORDER = 0x0200; /* Don't do owner Z ordering */
            //const UInt32 SWP_NOSENDCHANGING = 0x0400; /* Don't send WM_WINDOWPOSCHANGING */

            const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

            public static void MakeTopMost(IntPtr window) {
                SetForegroundWindow(window);
                SetWindowPos(window, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }

            public static void MakeNormal(IntPtr window) {
                SetWindowPos(window, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }
        }
    }
}