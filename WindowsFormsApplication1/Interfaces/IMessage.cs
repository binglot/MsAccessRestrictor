namespace MsAccessRestrictor.Interfaces {
    public interface IMessageWriter {
        void ShowError(string message);
        void ShowInfo(string message);
    }
}