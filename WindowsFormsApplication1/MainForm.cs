using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1 {
    public partial class MainForm : Form {
        private readonly FeaturesManager _features;
        public static MainForm Instance;

        public MainForm(FeaturesManager features) {
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
