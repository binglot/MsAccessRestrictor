using System.Collections.Generic;
using System.Linq;
using WindowsFormsApplication1.Interfaces;

namespace WindowsFormsApplication1 {
    public class FeaturesManager {
        private readonly List<IFeature> _features;

        public FeaturesManager() : this(new FeaturesProvider()) {}

        public FeaturesManager(IFeaturesProvider provider) {
            _features = provider.GetFeatures().ToList();
        }

        public void SetAll() {
            _features.ForEach(f => f.Run());
        }
        
        public void ClearAll() {
            _features.ForEach(f => f.Clear());
        }
    }
}
