using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Wrappers {
    class MessageWriter : IMessageWriter {
        public void ShowError(string message) {
            MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfo(string message) {
            MessageBox.Show(message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}