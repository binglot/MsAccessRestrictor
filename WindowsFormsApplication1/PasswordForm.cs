using System;
using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor {
    public partial class PasswordForm : Form, IPasswordForm {
        
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
            e.Handled = true;

            if (e.KeyData == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        new DialogResult ShowDialog() {
            Password = String.Empty;
            TopMost = true;
            TopLevel = true;
            
            return base.ShowDialog();
        }
    }
}
