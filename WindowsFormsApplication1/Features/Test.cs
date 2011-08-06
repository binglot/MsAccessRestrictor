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

            [DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
            private static extern int GetMenuItemCount(IntPtr hmenu);

            [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
            private static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);

            [DllImport("user32.dll", EntryPoint = "DrawMenuBar")]
            private static extern int DrawMenuBar(IntPtr hwnd);

            private const int MF_BYPOSITION = 0x0400;
            //private const int MF_BYCOMMAND = 0x0000;

            public static void DisableButtons(IntPtr window) {
                //IntPtr hmenu = GetSystemMenu(window, 0);
                //int itemCount = GetMenuItemCount(hmenu);

                //for (int i = itemCount; i >= 0; i--) {
                //    RemoveMenu(hmenu, i, MF_BYPOSITION);
                //}

                //// seems to be working without it?!
                //DrawMenuBar(window);


                // new way!
                //EnableCloseButton(window, false);


                // better way
                //EnableCloseButton(window, SystemCommands.SC_CLOSE, false);
                //EnableCloseButton(window, SystemCommands.SC_SEPARATOR, false);
                //EnableCloseButton(window, SystemCommands.SC_ARRANGE, false);
                //EnableCloseButton(window, SystemCommands.SC_RESTORE, false);
                //EnableCloseButton(window, SystemCommands.SC_RESTORE2, false);
                //EnableCloseButton(window, SystemCommands.SC_MAXIMIZE, false);
                //EnableCloseButton(window, SystemCommands.SC_MAXIMIZE2, false);
                //EnableCloseButton(window, SystemCommands.SC_MINIMIZE, false);


                // one more go
                IntPtr hMenu = GetSystemMenu(window, 0);
                if (hMenu.ToInt32() != 0) {
                    for (int i = GetMenuItemCount(hMenu) - 1; i >= 0; i--) {
                        StringBuilder menuName = new StringBuilder(0x20);
                        GetMenuString(hMenu.ToInt32(), (uint)i, menuName, 0x20, MF_BYPOSITION);
                        ModifyMenu(hMenu, (uint) i, MF_BYCOMMAND | MF_GRAYED, UIntPtr.Zero, menuName.ToString());
                    }
                }
            }

            public static void EnableButtons(IntPtr window) {
                //IntPtr hmenu = GetSystemMenu(window, 0);
                //for (int i = 8; i >= 0; i--) {
                //    RemoveMenu(hmenu, i, MF_BYCOMMAND);
                //}

                //DrawMenuBar(window);

                // new way!
                EnableCloseButton(window, SystemCommands.SC_CLOSE, true);
            }

            [DllImport("user32.dll")]
            static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
            const UInt32 SC_CLOSE = 0xF060;
            const UInt32 MF_ENABLED = 0x00000000;
            const UInt32 MF_GRAYED = 0x00000001;
            const UInt32 MF_DISABLED = 0x00000002;
            const uint MF_BYCOMMAND = 0x00000000;


            [DllImport("User32.dll")]
            static extern int GetMenuString(int hMenu, uint uIDItem, StringBuilder lpString, int nMaxCount, uint uFlag);
            [DllImport("user32.dll")]
            static extern bool ModifyMenu(IntPtr hMnu, uint uPosition, uint uFlags, UIntPtr uIDNewItem, string lpNewItem);

            static void EnableCloseButton(IntPtr window, SystemCommands button, bool bEnabled) {
                var hSystemMenu = GetSystemMenu(window, 0);
                //EnableMenuItem(hSystemMenu, (uint)button , (uint)(MF_ENABLED | (bEnabled ? MF_ENABLED : MF_GRAYED)));
                EnableMenuItem(hSystemMenu, (uint)button, (uint)(bEnabled ? MF_ENABLED : MF_GRAYED | MF_DISABLED));
            }

            public enum SystemCommands {
                SC_SIZE = 0xF000,
                SC_MOVE = 0xF010,
                SC_MINIMIZE = 0xF020,

                ///<summary>
                /// Sent when form maximizes
                ///</summary>
                SC_MAXIMIZE = 0xF030,

                ///<summary>
                /// Sent when form maximizes because of doubcle click on caption
                /// JTE: Don't use this constant. As per the documentation, you
                ///      must mask off the last 4 bits of wParam by AND'ing it
                ///      with 0xFFF0. You can't assume the last 4 bits. 
                ///</summary>
                SC_MAXIMIZE2 = 0xF032,
                SC_NEXTWINDOW = 0xF040,
                SC_PREVWINDOW = 0xF050,
                ///<summary>
                /// Closes the form
                ///</summary>
                SC_CLOSE = 0xF060,
                SC_VSCROLL = 0xF070,
                SC_HSCROLL = 0xF080,
                SC_MOUSEMENU = 0xF090,
                SC_KEYMENU = 0xF100,
                SC_ARRANGE = 0xF110,

                ///<summary>
                /// Sent when form is maximized from the taskbar
                ///</summary>
                SC_RESTORE = 0xF120,
                ///<summary>
                /// Sent when form maximizes because of doubcle click on caption
                /// JTE: Don't use this constant. As per the documentation, you
                ///      must mask off the last 4 bits of wParam by AND'ing it
                ///      with 0xFFF0. You can't assume the last 4 bits. 
                ///</summary>
                SC_RESTORE2 = 0xF122,
                SC_TASKLIST = 0xF130,
                SC_SCREENSAVE = 0xF140,
                SC_HOTKEY = 0xF150,
                SC_DEFAULT = 0xF160,
                SC_MONITORPOWER = 0xF170,
                SC_CONTEXTHELP = 0xF180,
                SC_SEPARATOR = 0xF00F
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