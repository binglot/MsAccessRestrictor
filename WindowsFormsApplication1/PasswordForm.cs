using System;
using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor {
    public partial class PasswordForm : Form, IPasswordForm {

        public PasswordForm() {
            InitializeComponent();
            Shown += delegate { SetTopMostWindow(true); };
            Closing += delegate { SetTopMostWindow(false); };
        }

        public string Password {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        public new DialogResult ShowDialog() {
            Password = String.Empty;
            return base.ShowDialog();
        }

        private void SetTopMostWindow(bool enable) {
            WinApi.SetWindowPos(this.Handle, enable ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            WinApi.SetForegroundWindow(this.Handle);
        }

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
    }
}
