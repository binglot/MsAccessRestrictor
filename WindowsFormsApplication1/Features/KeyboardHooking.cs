using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Features {
    public class KeyboardHooking : IFeature {
        const string PasswordString = "dupa";
        const int KeyboardWindowHandler = 13;
        readonly IPasswordForm _passwordForm;
        readonly int _instance;
        static bool _passwordDialogIsOpen;
        int _hookId;


        public KeyboardHooking() : this(new PasswordForm()) { }

        public KeyboardHooking(IPasswordForm passwordForm) {
            _passwordForm = passwordForm;
            _instance = Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32();
        }

        public void Run() {
            _hookId = HookWindowsKeyboard(KeyboardProcessor, _instance);
        }

        public void Clear() {
            UnhookWindowsKeyboard(_hookId);
        }

        private static int HookWindowsKeyboard(WinApi.LowLevelKeyboardProcDelegate handler, int instance) {
            return WinApi.SetWindowsHookEx(KeyboardWindowHandler, handler, instance, 0);
        }

        private static void UnhookWindowsKeyboard(int hookId) {
            WinApi.UnhookWindowsHookEx(hookId);
        }

        private int KeyboardProcessor(int nCode, int wParam, ref WinApi.HookStruct lParam) {
            var foundShortcut = false;

            switch (wParam) {
                case 256:
                case 257:
                case 260:
                case 261:
                    if (wParam == 260 && (lParam.Flags == 32) && (lParam.VkCode == 0x7B)) { // Alt+F12
                        PasswordDialog();
                    }

                    foundShortcut = ((lParam.Flags == 32) && (lParam.VkCode == 0x09)) ||    // Alt+Tab
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x1B)) ||                // Alt+Esc
                        ((lParam.Flags == 0) && (lParam.VkCode == 0x1B)) ||                 // Ctrl+Esc
                        ((lParam.Flags == 1) && (lParam.VkCode == 0x5B)) ||                 // Left Windows Key
                        ((lParam.Flags == 1) && (lParam.VkCode == 0x5C)) ||                 // Right Windows Key
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x73)) ||                // Alt+F4              
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x20));                  // Alt+Space

                    break;
            }

            return foundShortcut ? 1 : WinApi.CallNextHookEx(0, nCode, wParam, ref lParam);
        }

        private void PasswordDialog() {
            if (_passwordDialogIsOpen) {
                return;
            }

            _passwordDialogIsOpen = true;
            Task.Factory.StartNew(ShowPasswordDialog); // to avoid thread's hiccups
        }

        private void ShowPasswordDialog() {
            if (_passwordForm.ShowDialog() == DialogResult.OK) {
                if (_passwordForm.Password == PasswordString) {
                    Application.Exit();
                }
            }

            _passwordDialogIsOpen = false;
        }
    }
}
