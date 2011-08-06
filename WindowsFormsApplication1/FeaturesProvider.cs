using System.Collections.Generic;
using WindowsFormsApplication1.Features;
using WindowsFormsApplication1.Interfaces;

namespace WindowsFormsApplication1 {
    public class FeaturesProvider : IFeaturesProvider {
        public IEnumerable<IFeature> GetFeatures() {
            yield return new CtrlAltDelete();
            yield return new KeyboardHooking();
            yield return new Taskbar();
            yield return new WindowFocus();
            yield return new WindowButtons();
        }
    }
}