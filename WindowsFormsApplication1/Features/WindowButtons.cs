using System;
using System.Text;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Utils;

namespace MsAccessRestrictor.Features {
    internal class WindowButtons : IFeature {
        readonly IntPtr _msAccessWindow;
        UIntPtr[] _originalButtonIds;

        enum ButtonOption : uint {
            Enabled = 0,
            Grayed = 1
        }

        enum MenuOption {
            GetByCommand = 0,
            GetByPosition = 1024,
        }

        public WindowButtons() {
            _msAccessWindow = WinApi.GetMsAccessWindowHandle();
        }

        public void Run() {
            SetWindowMenuButtons(_msAccessWindow, ButtonOption.Grayed);
        }

        public void Clear() {
            SetWindowMenuButtons(_msAccessWindow, ButtonOption.Enabled);
        }

        void SetWindowMenuButtons(IntPtr window, ButtonOption buttonFlag) {
            // Find all the buttons on the window's menu
            var windowMenu = GetSystemMenu(window);
            var itemsCount = WinApi.GetMenuItemCount(windowMenu);

            if (itemsCount == 0) {
                return;
            }

            if (_originalButtonIds == null) {
                _originalButtonIds = new UIntPtr[itemsCount];
            }

            // Modify each button
            for (var position = 0; position < itemsCount; position++) {
                uint sourceId;
                UIntPtr destId;

                // Swap source and destination IDs to turn it on/off
                switch (buttonFlag) {
                    case ButtonOption.Enabled:
                        sourceId = UInt32.MaxValue - (uint)position;
                        destId = _originalButtonIds[position];
                        break;
                    case ButtonOption.Grayed:
                        sourceId = (uint)WinApi.GetMenuItemID(windowMenu, position);
                        destId = new UIntPtr(UInt32.MaxValue - (uint)position);
                        RetainOriginalButtonId(position, sourceId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("buttonFlag");
                }

                ModifyMenu(position, sourceId, destId, windowMenu, buttonFlag);
            }
        }

        static IntPtr GetSystemMenu(IntPtr window) {
            return WinApi.GetSystemMenu(window, 0);
        }

        private void RetainOriginalButtonId(int position, uint buttonId) {
            _originalButtonIds[position] = new UIntPtr(buttonId);
        }

        private static string GetButtonLabel(int itemId, IntPtr systemMenu) {
            var label = new StringBuilder();
            WinApi.GetMenuString(systemMenu, (uint)itemId, label, label.Capacity, (uint)MenuOption.GetByPosition);
            return label.ToString();
        }

        private static void ModifyMenu(int position, uint sourceId, UIntPtr destId, IntPtr windowMenu,
            ButtonOption buttonFlag) {
            var buttonLabel = GetButtonLabel(position, windowMenu);

            // Skip the menu separator
            if (buttonLabel == "") {
                return;
            }

            var flags = (uint)MenuOption.GetByCommand | (uint)buttonFlag;
            WinApi.ModifyMenu(windowMenu, sourceId, flags, destId, buttonLabel);
        }
    }
}