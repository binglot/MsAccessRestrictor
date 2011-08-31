using System.Runtime.InteropServices;
using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Features {
    public class KeyboardHooking : IFeature {
        const int KeyboardWindowHandler = 13;
        readonly int _instance;
        int _intLlKey;


        public KeyboardHooking() {
            //uint processHandle;
            //IntPtr windowHandle = FindWindowByCaption(0, "Untitled - Notepad");
            //uint threadId = GetWindowThreadProcessId(windowHandle, out processHandle);
            _instance = Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32();
        }

        private static int LowLevelKeyboardProc(int nCode, int wParam, ref WinApi.HookStruct lParam) {
            bool blnEat = false;

            switch (wParam) {
                case 256:
                case 257:
                case 260:
                case 261:
                    // Alt+F12
                    if ((lParam.Flags == 32) && (lParam.VkCode == 0x7B)) {
                        //_pressed = true;
                        //MessageBox.Show("You're good!");
                        ShowPasswordDialog();
                        return 1;
                    }

                    blnEat = ((lParam.Flags == 32) && (lParam.VkCode == 0x09)) ||       // Alt+Tab
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x1B)) ||            // Alt+Esc
                        ((lParam.Flags == 0) && (lParam.VkCode == 0x1B)) ||             // Ctrl+Esc
                        ((lParam.Flags == 1) && (lParam.VkCode == 0x5B)) ||             // Left Windows Key
                        ((lParam.Flags == 1) && (lParam.VkCode == 0x5C)) ||             // Right Windows Key
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x73)) ||            // Alt+F4              
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x20));              // Alt+Space

                    break;
            }

            return blnEat ? 1 : WinApi.CallNextHookEx(0, nCode, wParam, ref lParam);
        }

        private static void ShowPasswordDialog() {
            var passwordForm = new PasswordForm();

            if (passwordForm.ShowDialog() == DialogResult.OK) {
                if (passwordForm.Password == "dupa") {
                    MainForm.Instance.Close();
                }
            }
        }

        private static int HookWindowsHookEx(WinApi.LowLevelKeyboardProcDelegate lpfn, int hMod) {
            return WinApi.SetWindowsHookEx(KeyboardWindowHandler, lpfn, hMod, 0);
        }

        public void Run() {
            _intLlKey = HookWindowsHookEx(LowLevelKeyboardProc, _instance);
        }

        public void Clear() {
            WinApi.UnhookWindowsHookEx(_intLlKey);
        }
    }
}
