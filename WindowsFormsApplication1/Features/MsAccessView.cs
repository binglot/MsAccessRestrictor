using System;
using Microsoft.Office.Interop.Access;
using MsAccessRestrictor.Interfaces;
using System.Runtime.InteropServices;

namespace MsAccessRestrictor.Features {
    public class MsAccessView : IFeature, IDisposable {
        Application _accessApp;

        public void Run() {
            //_accessApp = (Application)Marshal.GetActiveObject("Access.Application");
            _accessApp = new Application();
            _accessApp.OpenCurrentDatabase(@"C:\Users\Bart\Documents\Database1.accdb");
            _accessApp.DoCmd.ShowToolbar("Ribbon", AcShowToolbar.acToolbarNo);
            _accessApp.RunCommand(AcCommand.acCmdWindowHide);
            _accessApp.Visible = true;
        }

        public void Clear() {
            if (_accessApp == null) {
                return;
            }

            _accessApp.DoCmd.ShowToolbar("Ribbon");
            _accessApp.RunCommand(AcCommand.acCmdWindowUnhide);
        }

        public void Dispose() {
            if (_accessApp == null) {
                return;
            }

            Marshal.ReleaseComObject(_accessApp);
        }
    }
}