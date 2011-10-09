using System.Diagnostics;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Wrappers {
    class ProcessBase : IProcess {
        public virtual Process[] GetProcessesByName(string name) {
            return Process.GetProcessesByName(name);
        }
    }
}