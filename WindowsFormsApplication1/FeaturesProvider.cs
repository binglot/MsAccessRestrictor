using System.Collections.Generic;
using MsAccessRestrictor.Features;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor {
    public class FeaturesProvider : IFeaturesProvider {
        public IEnumerable<IFeature> GetFeatures() {
            //yield return new CtrlAltDelete();
            yield return new KeyboardHooking();
            //yield return new Taskbar();
            yield return new WindowFocus();
            yield return new WindowButtons();
        }
    }
}