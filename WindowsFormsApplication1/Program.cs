using System;
using System.Windows.Forms;
using MsAccessRestrictor.Forms;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Main;
using MsAccessRestrictor.Properties;

namespace MsAccessRestrictor {
    static class Program {
        static Settings _settings = Settings.Default;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            if (_settings.OpenDbFile) {
                // check that the file exists
            }

            using (var features = new FeaturesManager()) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(features));
            }
        }
    }
}
