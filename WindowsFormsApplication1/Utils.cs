using System;

namespace WindowsFormsApplication1 {
    static class Utils {
        private const string MsAccessClassName = "OMAIN";

        public static IntPtr GetMsAccessWindowHandle() {
            return WinApi.FindWindow(MsAccessClassName, null);
        }
    }
}
