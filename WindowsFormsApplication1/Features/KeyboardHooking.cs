using System;
using System.Runtime.InteropServices;
using WindowsFormsApplication1.Interfaces;

namespace WindowsFormsApplication1.Features {
    public class KeyboardHooking : IFeature {
        [DllImport("user32", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern int SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, int hMod, int dwThreadId);
        
        [DllImport("user32", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern int UnhookWindowsHookEx(int hHook);
        delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref HookStruct lParam);
        
        [DllImport("user32", EntryPoint = "CallNextHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref HookStruct lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(int ZeroOnly, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


#pragma warning disable 649, 169
        private struct HookStruct {
            public int VkCode;
            private int _scanCode;
            public int Flags;
            private int _time;
            private int _dwExtraInfo;
        }
#pragma warning restore 649, 169

        const int KeyboardWindowHandler = 13;
        readonly int _instance;
        int _intLlKey;


        public KeyboardHooking() {
            uint processHandle;
            IntPtr windowHandle = FindWindowByCaption(0, "Untitled - Notepad");
            uint threadID = GetWindowThreadProcessId(windowHandle, out processHandle);
            _instance = Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32();

        }

        private static int LowLevelKeyboardProc(int nCode, int wParam, ref HookStruct lParam) {
            bool blnEat = false;

            switch (wParam) {
                case 256:
                case 257:
                case 260:
                case 261:
                    //Alt+Tab, Alt+Esc, Ctrl+Esc, Windows Key, Alt+F4
                    blnEat = ((lParam.VkCode == 9) && (lParam.Flags == 32)) |  // Alt+Tab
                             ((lParam.VkCode == 27) && (lParam.Flags == 32)) | // Alt+Esc
                             ((lParam.VkCode == 27) && (lParam.Flags == 0)) |  // Ctrl+Esc
                             ((lParam.VkCode == 91) && (lParam.Flags == 1)) |  // Windows Key
                             //((lParam.VkCode == 92) && (lParam.Flags == 1)) |  // ?
                             //((lParam.VkCode == 73) && (lParam.Flags == 0)) |  // ?
                             (lParam.VkCode == 115);                           // Alt+F4
                    break;
            }

            return blnEat ? 1 : CallNextHookEx(0, nCode, wParam, ref lParam);
        }

        private static int HookWindowsHookEx(LowLevelKeyboardProcDelegate lpfn, int hMod) {
            return SetWindowsHookEx(KeyboardWindowHandler, lpfn, hMod, 0);
        }

        public void Run() {
            _intLlKey = HookWindowsHookEx(LowLevelKeyboardProc, _instance);
        }

        public void Clear() {
            UnhookWindowsHookEx(_intLlKey);
        }
    }
}
