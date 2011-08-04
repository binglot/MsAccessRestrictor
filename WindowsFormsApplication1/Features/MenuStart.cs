using System.Runtime.InteropServices;
using WindowsFormsApplication1.Interfaces;

namespace WindowsFormsApplication1.Features {
    class MenuStart : IFeature {
        [DllImport("user32.dll")]
        static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        static extern int ShowWindow(int hwnd, int command);

        public void Run() {
            HideStartMenuAccess(true);
        }

        public void Clear() {
            HideStartMenuAccess(false);
        }

        private static void HideStartMenuAccess(bool value) {
            int hwnd = FindWindow("Shell_TrayWnd", "");
            int showValue = value ? 0 : 1;
            ShowWindow(hwnd, showValue);
        }
    }
}
