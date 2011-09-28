using System.IO;
using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Properties;
using MsAccessRestrictor.Wrappers;

namespace MsAccessRestrictor.Main {
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
            CheckNumberOfInstances();
            CheckOpenMsAccess();
            CheckOpenDbFile();
        }

        private void CheckNumberOfInstances() {
            const string alreadyRunningError = "There's already one instance of the application running!";
            
            var processes = _process.GetProcessesByName("MsAccessRestrictor");

            if (processes.Length != 1)
            {
                ShowErrorAndStopRunning(alreadyRunningError);
            }
        }

        private void CheckOpenDbFile() {
            const string openFileError = "Opening the database file failed. Check your settings.";

            if (!_settings.OpenMsAccess || !_settings.OpenDbFile) {
                return;
            }

            if (!File.Exists(_settings.DbFilePath)) {
                ShowErrorAndStopRunning(openFileError);
            }
        }

        private void CheckOpenMsAccess() {
            // TODO: put these constants in the Resources
            const string closeError = "Close Microsoft Access before running the application or change your settings.";
            const string openError = "Open Microsoft Access before running the application or change your settings.";
            const string moreThanOneError = "Leave only one Microsoft Access window open before running the application or change your settings.";

            var processes = _process.GetProcessesByName("MSACCESS");

            if (_settings.OpenMsAccess) {
                // Show error if there's a running MsAccess
                if (processes.Length != 0) {
                    ShowErrorAndStopRunning(closeError);
                }
            }
            else {
                if (processes.Length == 0) {
                    ShowErrorAndStopRunning(openError);
                }
                else if (processes.Length > 1) {
                    ShowErrorAndStopRunning(moreThanOneError);
                }
            }
        }

        private void ShowErrorAndStopRunning(string message) {
            _messageWriter.ShowError(message);
            Application.Exit();
        }
    }
}