using System;
using System.Collections.Generic;
using System.Linq;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Main {
    public class FeaturesManager : IFeaturesManager {
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
        
        ~FeaturesManager() {
            DisposeAll();
        }

        public virtual void Dispose() {
            DisposeAll();
            GC.SuppressFinalize(this);
        }

        void DisposeAll() {
            _features.OfType<IDisposable>().ToList().ForEach(f => f.Dispose());
        }
    }
}
