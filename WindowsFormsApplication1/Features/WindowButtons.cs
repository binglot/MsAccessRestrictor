using System;
using System.Text;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Features {
    class WindowButtons : IFeature {
        private readonly IntPtr _windowHandle;

        public WindowButtons() {
            _windowHandle = Utils.GetMsAccessWindowHandle();
        }

        public void Run() {
            DisableButtons(_windowHandle);
        }

        public void Clear() {
            EnableButtons(_windowHandle);
        }

        #region Implementation Details

        const UInt32 MF_ENABLED = 0x00000000;
        const UInt32 MF_GRAYED = 0x00000001;
        const int MF_BYPOSITION = 0x00000400;
        const int MF_BYCOMMAND = 0x00000000;
        //const UInt32 MF_DISABLED = 0x00000002;

        private UIntPtr[] _originalButtonIds;

        public void DisableButtons(IntPtr window) {
            EnableMenuButtons(window, false);
        }

        public void EnableButtons(IntPtr window) {
            EnableMenuButtons(window, true);
        }

        void EnableMenuButtons(IntPtr window, bool enabled) {
            var label = new StringBuilder();
            var systemMenu = WinApi.GetSystemMenu(window, 0);
            var flags = MF_BYCOMMAND | (enabled ? MF_ENABLED : MF_GRAYED);
            var itemsCount = WinApi.GetMenuItemCount(systemMenu);
            _originalButtonIds = new UIntPtr[itemsCount];

            if (itemsCount > 0) {
                for (int i = itemsCount - 1; i >= 0; i--) {
                    WinApi.GetMenuString(systemMenu, (uint)i, label, label.Capacity, MF_BYPOSITION);

                    // To skip the menu separator
                    if (label.ToString() == "") {
                        continue;
                    }

                    uint sourceId;
                    UIntPtr destId;

                    if (enabled) {
                        sourceId = UInt32.MaxValue - (uint)i;
                        destId = _originalButtonIds[i];
                    }
                    else {
                        var id = WinApi.GetMenuItemID(systemMenu, i);
                        _originalButtonIds[i] = id;
                        sourceId = id.ToUInt32();
                        destId = new UIntPtr(UInt32.MaxValue - (uint)i);
                    }

                    WinApi.ModifyMenu(systemMenu, sourceId, flags, destId, label.ToString());
                }
            }
        }

        #endregion
    }
}