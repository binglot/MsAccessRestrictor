using System;
using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Forms {
    public partial class PasswordForm : Form, IPasswordForm {
        public PasswordForm() {
            InitializeComponent();
            Shown += delegate { SetTopMostWindow(true);  Refresh(); };
            Closing += delegate { SetTopMostWindow(false); };
        }

        public string Password {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        private void btnOk_Click(object sender, EventArgs e) {
            Close();
        }

        public new DialogResult ShowDialog() {
            Password = String.Empty;
            return base.ShowDialog();
        }

        private void SetTopMostWindow(bool enable) {
            Utils.SetTopMostWindow(Handle, enable);
        }
    }
}
