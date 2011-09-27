using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MsAccessRestrictor.Forms;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Properties;
using MsAccessRestrictor.Utils;

namespace MsAccessRestrictor.Features {
    class KeyboardHooking : IFeature {
        const int KeyboardWindowHandler = 13;
        const int ThreadId = 0;
        const int F1Key = 0x70;
        const int F12Key = 0x7B;

        readonly IPasswordForm _passwordForm;
        readonly object _locker = new object();
        readonly string _password;
        readonly int _instance;
        static bool _passwordDialogIsOpen;
        int _hookId;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public KeyboardHooking() : this(new PasswordForm(), Settings.Default) { }

        public KeyboardHooking(IPasswordForm passwordForm, Settings settings) {
            _passwordForm = passwordForm;
            _password = settings.Password;
            _instance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32();
        }

        public void Run() {
            _hookId = HookWindowsKeyboard(KeyboardProcessor, _instance);
        }

        public void Clear() {
            UnhookWindowsKeyboard(_hookId);
        }

        private static int HookWindowsKeyboard(WinApi.LowLevelKeyboardProcDelegate handler, int instance) {
            return WinApi.SetWindowsHookEx(KeyboardWindowHandler, handler, instance, ThreadId);
        }

        private static void UnhookWindowsKeyboard(int hookId) {
            WinApi.UnhookWindowsHookEx(hookId);
        }

        private int KeyboardProcessor(int nCode, int wParam, ref WinApi.HookStruct lParam) {
            var shortcutPressed = false;

            switch (wParam) {
                case 256:
                case 257:
                case 260:
                case 261:
                    if (wParam == 260 && (lParam.Flags == 32) && (lParam.VkCode == F12Key)) {  // Alt+F12
                        PasswordDialog();
                        return 1;
                    }

                    shortcutPressed = PressedAlt(lParam) ||
                                      PressedLeftWindowsKey(lParam) ||
                                      PressedRightWindowsKey(lParam) ||
                                      PressedLeftControl(lParam) ||
                                      PressedRightControl(lParam) ||
                                      PressedAnyOfF1ToF12(lParam);

                    break;
            }

            return shortcutPressed ? 1 : WinApi.CallNextHookEx(0, nCode, wParam, ref lParam);
        }

        private void PasswordDialog() {
            lock (_locker) {
                if (_passwordDialogIsOpen) {
                    return;
                }

                _passwordDialogIsOpen = true;
            }

            Task.Factory.StartNew(ShowPasswordDialog); // to avoid thread's hiccups
        }

        private void ShowPasswordDialog() {
            if (_passwordForm.ShowDialog() == DialogResult.OK) {
                if (_passwordForm.Password == _password) {
                    Application.Exit();
                }
            }

            lock (_locker) {
                _passwordDialogIsOpen = false;
            }
        }

        private static bool IsBetween(int value, int left, int right) {
            return value >= left && value <= right;
        }

        #region Key Presses

        private static bool PressedAlt(WinApi.HookStruct lParam) {
            return (lParam.Flags == 32 && lParam.VkCode != F12Key);
        }

        private static bool PressedLeftWindowsKey(WinApi.HookStruct lParam) {
            return ((lParam.Flags == 1) && (lParam.VkCode == 0x5B));
        }

        private static bool PressedRightWindowsKey(WinApi.HookStruct lParam) {
            return ((lParam.Flags == 1) && (lParam.VkCode == 0x5C));
        }

        private static bool PressedLeftControl(WinApi.HookStruct lParam) {
            return ((lParam.Flags == 0) && (lParam.VkCode == 0xA2));
        }

        private static bool PressedRightControl(WinApi.HookStruct lParam) {
            return ((lParam.Flags == 1) && (lParam.VkCode == 0xA3));
        }

        private bool PressedAnyOfF1ToF12(WinApi.HookStruct lParam) {
            return ((lParam.Flags == 0) && (IsBetween(lParam.VkCode, F1Key, F12Key)));
        }
        
        #endregion
    }
}
