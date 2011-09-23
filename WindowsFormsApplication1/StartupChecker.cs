using System.Diagnostics;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Properties;
using MsAccessRestrictor.Wrappers;

namespace MsAccessRestrictor {
    public class StartUpChecker {
        private readonly Settings _settings;
        private readonly IMessageWriter _messageWriter;
        private readonly IProcess _process;

        public StartUpChecker()
            : this(Settings.Default, new MessageWriter(), new ProcessBase()) {
            //
        }

        internal StartUpChecker(Settings settings, IMessageWriter messageWriter, IProcess process) {
            _settings = settings;
            _messageWriter = messageWriter;
            _process = process;
        }

        public void Run() {
            CheckOpenMsAccess();

        }

        private void CheckOpenMsAccess() {
            const string closeError = "Close MsAccess before running the application or change your settings.";
            const string openError = "Open MsAccess before running the application or change your settings.";
            const string moreThanOneError = "Leave only one MsAccess window open before running the application or change your settings.";

            var processes = _process.GetProcessesByName("MSACCESS");

            if (_settings.OpenMsAccess)
            {
                // Show error if there's a running MsAccess
                if (processes.Length != 0)
                {
                    _messageWriter.ShowError(closeError);
                }
            }
            else
            {
                if (processes.Length == 0)
                {
                    _messageWriter.ShowError(openError);
                }
                else if (processes.Length > 1)
                {
                    _messageWriter.ShowError(moreThanOneError);
                }
            }
        }
    }
}