using System;
using System.Windows.Forms;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor {
    public partial class MainForm : Form {
        private readonly IFeaturesManager _features;
        public static MainForm Instance;

        public MainForm(IFeaturesManager features) {
            _features = features;
            Instance = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            _features.SetAll();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            _features.ClearAll();
        }
    }
}
