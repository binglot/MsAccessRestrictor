using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MsAccessRestrictor;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictorTests {
    [TestClass]
    public class FeaturesManager_Tests {
        private Mock<IFeaturesProvider> _mockFeaturesProvider;

        [TestInitialize]
        public void Initialize() {
            _mockFeaturesProvider = new Mock<IFeaturesProvider>();
        }

        [TestCleanup]
        public void Cleanup() {
            _mockFeaturesProvider.Verify();
        }

        [TestMethod]
        public void Get_features_from_a_feature_provider() {
            var featuresList = new List<IFeature>();

            _mockFeaturesProvider.Setup(p => p.GetFeatures()).Returns(featuresList);

            new FeaturesManager(_mockFeaturesProvider.Object);
        }

        [TestMethod]
        public void Enable_all_the_features() {
            RunOnAllFeatures(feature => feature.Run());
        }

        [TestMethod]
        public void Disable_all_the_features() {
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
