using System.Windows.Forms;
namespace MsAccessRestrictor.Interfaces {
    public interface IPasswordForm {
        string Password { get; set; }
        DialogResult ShowDialog();
        void SetTopLevel();
    }
}
