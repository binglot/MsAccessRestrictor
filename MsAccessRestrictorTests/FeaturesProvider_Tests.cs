using System;
using System.Collections.Generic;
using System.Linq;
using MsAccessRestrictor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsAccessRestrictor.Features;
using MsAccessRestrictor.Main;

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

            Assert.AreEqual(featuresTypeList.Count(), featuresList.Count(), "The number of features doesn't match.");

            foreach (var feature in featuresList.Select(feature => feature.GetType())) {
                var shortName = GetFeaturesShortName(feature);
                Assert.IsTrue(featuresTypeList.Contains(feature), String.Format("Missing the feature: {0}", shortName));
            }
        }

        private static string GetFeaturesShortName(Type feature) {
            var featuresName = feature.ToString();
            var lastDot = featuresName.LastIndexOf('.');
            var shortName = featuresName.Substring(lastDot + 1);
            
            return shortName;
        }

        static IEnumerable<Type> GetFeaturesTypeNames() {
            yield return typeof(CtrlAltDelete);
            yield return typeof(KeyboardHooking);
            yield return typeof(HideTaskbar);
            yield return typeof(WindowFocus);
            yield return typeof(WindowButtons);
        }
    }
}