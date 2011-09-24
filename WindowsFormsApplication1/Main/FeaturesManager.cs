using System;
using System.Collections.Generic;
using System.Linq;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Utils;

namespace MsAccessRestrictor.Main {
    internal class FeaturesManager : DisposeBase, IFeaturesManager {
        readonly List<IFeature> _features;
        bool _disposed;

        public FeaturesManager() : this(new FeaturesProvider()) { }

        public FeaturesManager(IFeaturesProvider provider) {
            _features = provider.GetFeatures().ToList();
        }

        public void SetAll() {
            _features.ForEach(f => f.Run());
        }

        public void ClearAll() {
            _features.ForEach(f => f.Clear());
        }

        protected override void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    // Release managed resources
                    // ...
                }

                // Release unmanaged resources
                DisposeAll();
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        void DisposeAll() {
            _features.OfType<IDisposable>().ToList().ForEach(f => f.Dispose());
        }
    }
}
