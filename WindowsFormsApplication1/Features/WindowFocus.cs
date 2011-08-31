using System;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Features {
    public class WindowFocus : IFeature {
        private const int SW_MAXIMIZE = 3;
        private const int SW_RESTORE = 9;
        private readonly IntPtr _windowHandle;

        public WindowFocus() {
            _windowHandle = Utils.GetMsAccessWindowHandle();
        }

        public void Run() {
            MakeTopMost(_windowHandle);
        }

        public void Clear() {
            MakeNormal(_windowHandle);
        }

        public static void MakeTopMost(IntPtr window) {
            RestoreAndMaximize(window);
            Utils.SetTopMostWindow(window, true);
        }

        private static void RestoreAndMaximize(IntPtr window) {
            // If the window is minimized then won't become always top without this.
            if (WinApi.IsIconic(window)) {
                WinApi.ShowWindow(window, SW_RESTORE);
            }
            
            WinApi.ShowWindow(window, SW_MAXIMIZE);
        }

        public static void MakeNormal(IntPtr window) {
            Utils.SetTopMostWindow(window, false);
        }
    }
}