using System;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Features {
    public class WindowFocus : IFeature {
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
            WinApi.SetTopMostWindow(window, true);
        }

        public static void MakeNormal(IntPtr window) {
            WinApi.SetTopMostWindow(window, false);
        }
    }
}