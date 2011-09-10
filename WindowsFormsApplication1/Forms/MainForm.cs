using System;
using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Forms {
    public partial class MainForm : Form {
        private readonly IFeaturesManager _features;

        public MainForm(IFeaturesManager features) {
            _features = features;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            _features.SetAll();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            _features.ClearAll();
            _features.Dispose();
        }
    }
}
