using System.Collections.Generic;
using MsAccessRestrictor.Features;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Main {
    class FeaturesProvider : IFeaturesProvider {
        public IEnumerable<IFeature> GetFeatures() {
            yield return new CtrlAltDelete();
            yield return new HideTaskbar();
            yield return new KeyboardHooking();
            yield return new MsAccessView();
            yield return new RunInsideLimitedJob();
            yield return new WindowFocus();
            yield return new WindowButtons();
        }
    }
}