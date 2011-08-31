using System;
using System.Windows.Forms;

namespace MsAccessRestrictor {
    public partial class PasswordForm : Form {
        
        public PasswordForm() {
            InitializeComponent();
        }

        public string Password {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}
