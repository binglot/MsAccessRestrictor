using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Access;
using MsAccessRestrictor.Interfaces;
using System.Runtime.InteropServices;
using MsAccessRestrictor.Properties;

namespace MsAccessRestrictor.Features {
    public class MsAccessView : IFeature, IDisposable {
        const string ToolbarName = "Ribbon";
        readonly Settings _settings;
        _Application _accessApp;

        public MsAccessView() : this(Settings.Default) { }

        internal MsAccessView(Settings applicationSettings) {
            _settings = applicationSettings;

            try
            {
                if (_settings.OpenMsAccess) {
                    _accessApp = new Application();
                }
                else {
                    _accessApp = (Application)Marshal.GetActiveObject("Access.Application");
                }
            }
            catch (COMException cex)
            {
                Debug.WriteLine(cex);
            }
        }

        public void Run() {
            try {
                if (_settings.OpenDbFile) {
                    _accessApp.OpenCurrentDatabase(_settings.DbFilePath);
                }
                if (_settings.DisableRibbon) {
                    ShowToolbar(false);
                }
                if (_settings.DisableNavigationPane) {
                    ShowNavigationPane(false);
                }

                _accessApp.Visible = true;
            }
            catch (COMException cex) {
                Debug.WriteLine(cex);
            }
        }

        public void Clear() {
            if (_accessApp == null) {
                return;
            }

            try {
                ShowToolbar(true);
                ShowNavigationPane(true);
            }
            catch (COMException cex) {
                Debug.WriteLine(cex);
            }
        }

        public void Dispose() {
            if (_accessApp != null) {
                Marshal.ReleaseComObject(_accessApp);
                _accessApp = null;
            }
        }

        void ShowToolbar(bool show) {
            _accessApp.DoCmd.ShowToolbar(ToolbarName, show ? AcShowToolbar.acToolbarYes : AcShowToolbar.acToolbarNo);
        }

        void ShowNavigationPane(bool show) {
            if (show)
            {
                _accessApp.DoCmd.SelectObject(AcObjectType.acTable, InDatabaseWindow: true);
            }
            else
            {
                _accessApp.DoCmd.NavigateTo("acNavigationCategoryObjectType");
                _accessApp.DoCmd.RunCommand(AcCommand.acCmdWindowHide);
            }
        }
    }
}