using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        private readonly FeaturesManager _features;

        public Form1(FeaturesManager features) {
            _features = features;
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
