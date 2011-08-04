using System.Collections.Generic;

namespace WindowsFormsApplication1.Interfaces {
    public interface IFeaturesProvider {
        IEnumerable<IFeature> GetFeatures();
    }
}