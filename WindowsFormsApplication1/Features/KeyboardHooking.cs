using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MsAccessRestrictor.Forms;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Properties;

namespace MsAccessRestrictor.Features {
    class KeyboardHooking : IFeature {
        const int KeyboardWindowHandler = 13;
        const int ThreadId = 0;

        readonly IPasswordForm _passwordForm;
        readonly object _locker = new object();
        readonly string _password;
        readonly int _instance;
        static bool _passwordDialogIsOpen;
        int _hookId;


        public KeyboardHooking() : this(new PasswordForm(), Settings.Default) { }

        public KeyboardHooking(IPasswordForm passwordForm, Settings settings) {
            _passwordForm = passwordForm;
            _password = settings.Password;
            _instance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32();
        }

        public void Run() {
            _hookId = HookWindowsKeyboard(KeyboardProcessor, _instance);
            //PasswordDialog();
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
                    if (wParam == 260 && (lParam.Flags == 32) && (lParam.VkCode == 0x7B)) {  // Alt+F12
                        PasswordDialog();
                    }

                    shortcutPressed = ((lParam.Flags == 32) && (lParam.VkCode == 0x09)) ||   // Alt+Tab
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x1B)) ||                 // Alt+Esc
                        //((lParam.Flags == 0)  && (lParam.VkCode == 0x1B)) ||                 // Ctrl+Esc
                        ((lParam.Flags == 1)  && (lParam.VkCode == 0x5B)) ||                 // Left Windows Key
                        ((lParam.Flags == 1)  && (lParam.VkCode == 0x5C)) ||                 // Right Windows Key
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x73)) ||                 // Alt+F4              
                        ((lParam.Flags == 32) && (lParam.VkCode == 0x20)) ||                 // Alt+Space
                        ((lParam.Flags == 128) && (lParam.VkCode == 0x4F));                   // Ctrl+O

                    if (lParam.VkCode == 0x4F)
                    {
                        MessageBox.Show(lParam._scanCode.ToString());
                    }

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
    }
}
