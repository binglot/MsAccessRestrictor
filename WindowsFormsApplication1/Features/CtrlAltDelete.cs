using System;
using System.Windows.Forms;
using Microsoft.Win32;
using WindowsFormsApplication1.Interfaces;

namespace WindowsFormsApplication1.Features {
    class CtrlAltDelete : IFeature {
        readonly RegistryKey _currentUser = Registry.CurrentUser;
        const string SubKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
        const string Value = "DisableTaskMgr";

        public void Run() {
            DisableCtrlAltDelete(true);
        }

        public void Clear() {
            DisableCtrlAltDelete(false);
        }

        private void DisableCtrlAltDelete(bool value) {
            int keyValue = value ? 1 : 0;

            try {
                RegistryKey sk1 = _currentUser.OpenSubKey(SubKey, true);

                if (sk1 == null) {
                    _currentUser.CreateSubKey(SubKey);
                    sk1 = _currentUser.OpenSubKey(SubKey, true);
                }

                if (sk1 != null) {
                    sk1.SetValue(Value, keyValue);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(@"Exception: " + ex.Message);
            }
        }
    }
}
