namespace MsAccessRestrictor.Interfaces {
    public interface IProcess {
        System.Diagnostics.Process[] GetProcessesByName(string name);
    }
}