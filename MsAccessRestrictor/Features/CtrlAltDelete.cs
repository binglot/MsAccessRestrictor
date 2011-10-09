using System;
using System.Diagnostics;
using Microsoft.Win32;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Features {
    class CtrlAltDelete : IFeature {
        const bool WritableRegistry = true;
        const string RegistryBranch = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
        const string Key = "DisableTaskMgr";
        readonly RegistryKey _registry = Registry.CurrentUser;
        
        public void Run() {
            DisableCtrlAltDelete(true);
        }

        public void Clear() {
            DisableCtrlAltDelete(false);
        }

        private void DisableCtrlAltDelete(bool disable) {
            var keyValue = disable ? 1 : 0;

            try {
                var registryKey = GetRegistryKey() ?? CreateRegistryKey();

                if (registryKey != null) {
                    registryKey.SetValue(Key, keyValue);
                }
                else {
                    Debug.WriteLine("There was a problem writing to the registry.");
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(@"Exception: " + ex.Message);
            }
        }

        private RegistryKey CreateRegistryKey() {
            _registry.CreateSubKey(RegistryBranch);
            return GetRegistryKey();
        }

        private RegistryKey GetRegistryKey() {
            return _registry.OpenSubKey(RegistryBranch, WritableRegistry);
        }
    }
}
