using System.Collections.Generic;

namespace MsAccessRestrictor.Interfaces {
    public interface IFeaturesProvider {
        IEnumerable<IFeature> GetFeatures();
    }
}