using System;
using System.Collections.Generic;
using System.Linq;
using MsAccessRestrictor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsAccessRestrictor.Features;

namespace MsAccessRestrictorTests {
    [TestClass]
    public class FeaturesProvider_Tests {
        FeaturesProvider _featuresProvider;

        [TestInitialize]
        public void Setup() {
            _featuresProvider = new FeaturesProvider();
        }

        [TestCleanup]
        public void Cleanup() {
            //
        }

        [TestMethod]
        public void Getting_features_returns_all_of_them() {
            var featuresTypeList = GetFeaturesTypeNames();
            var featuresList = _featuresProvider.GetFeatures();

            foreach (var feature in featuresList.Select(feature => feature.GetType()))
            {
                Assert.IsTrue(featuresTypeList.Contains(feature));
            }
        }

        static IEnumerable<Type> GetFeaturesTypeNames() {
            yield return typeof(CtrlAltDelete);
            yield return typeof(KeyboardHooking);
            yield return typeof(Taskbar);
            yield return typeof(WindowFocus);
            yield return typeof(WindowButtons);
        }
    }
}