using System;

namespace MsAccessRestrictor {
    static class Utils {
        private const string MsAccessClassName = "OMAIN";

        public static IntPtr GetMsAccessWindowHandle() {
            return WinApi.FindWindow(MsAccessClassName, null);
        }

        // Sets focus on a window and makes it always on top
        public static void SetTopMostWindow(IntPtr window, bool enable) {
            WinApi.SetWindowPos(window, enable ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            WinApi.SetForegroundWindow(window);
        }

        //
        // Constants
        //
        // ReSharper disable InconsistentNaming
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        // ReSharper restore InconsistentNaming
    }
}
