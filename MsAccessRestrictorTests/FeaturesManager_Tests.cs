using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MsAccessRestrictor;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Main;

namespace MsAccessRestrictorTests {
    [TestClass]
    public class FeaturesManager_Tests {
        Mock<IFeaturesProvider> _mockFeaturesProvider;

        [TestInitialize]
        public void Setup() {
            _mockFeaturesProvider = new Mock<IFeaturesProvider>();
        }

        [TestCleanup]
        public void Cleanup() {
            _mockFeaturesProvider.Verify();
        }

        [TestMethod]
        public void Instantiating_gets_features_from_a_feature_provider() {
            var featuresList = new List<IFeature>();

            _mockFeaturesProvider.Setup(p => p.GetFeatures()).Returns(featuresList);

            new FeaturesManager(_mockFeaturesProvider.Object);
        }

        [TestMethod]
        public void Enabling_the_features_runs_the_Run_method_on_all_of_them() {
            RunOnAllFeatures(feature => feature.Run());
        }

        [TestMethod]
        public void Disabling_the_features_runs_the_Clear_method_on_all_of_them() {
            RunOnAllFeatures(feature => feature.Clear());
        }

        void RunOnAllFeatures(Expression<Action<IFeature>> action) {
            var mockFeature1 = new Mock<IFeature>();
            var mockFeature2 = new Mock<IFeature>();
            var mockFeature3 = new Mock<IFeature>();
            var featuresList = new List<IFeature> { mockFeature1.Object, mockFeature2.Object, mockFeature3.Object };

            _mockFeaturesProvider.Setup(p => p.GetFeatures()).Returns(featuresList);
            mockFeature1.Setup(action);
            mockFeature2.Setup(action);
            mockFeature3.Setup(action);

            var manager = new FeaturesManager(_mockFeaturesProvider.Object);
            manager.SetAll();

            mockFeature1.Verify();
            mockFeature2.Verify();
            mockFeature3.Verify();
        }
    }
}
