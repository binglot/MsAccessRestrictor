using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MsAccessRestrictor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictorTests {
    [TestClass]
    public class FeaturesManager_Tests {
        private Mock<IFeaturesProvider> _mockProvider;

        [TestInitialize]
        public void Initialize() {
            var featuresList = new List<IFeature>();
            _mockProvider = new Mock<IFeaturesProvider>();

            _mockProvider.Setup(p => p.GetFeatures()).Returns(featuresList);
        }

        [TestCleanup]
        public void Cleanup() {
            _mockProvider.Verify();
        }

        [TestMethod]
        public void Gets_features_from_a_feature_provider() {
            new FeaturesManager(_mockProvider.Object);
        }

        [TestMethod]
        public void Enables_all_the_features() {
            var mockFeature1 = new Mock<IFeature>();
            var mockFeature2 = new Mock<IFeature>();
            var mockFeature3 = new Mock<IFeature>();
            var featuresList = new List<IFeature> { mockFeature1.Object, mockFeature2.Object, mockFeature3.Object };

            _mockProvider.Setup(p => p.GetFeatures()).Returns(featuresList);
            mockFeature1.Setup(f => f.Run());
            mockFeature2.Setup(f => f.Run());
            mockFeature3.Setup(f => f.Run());

            var manager = new FeaturesManager(_mockProvider.Object);
            manager.SetAll();

            mockFeature1.Verify();
            mockFeature2.Verify();
            mockFeature3.Verify();
        }

        [TestMethod]
        public void Disables_all_the_features() {
            var mockFeature1 = new Mock<IFeature>();
            var mockFeature2 = new Mock<IFeature>();
            var mockFeature3 = new Mock<IFeature>();
            var featuresList = new List<IFeature> { mockFeature1.Object, mockFeature2.Object, mockFeature3.Object };

            _mockProvider.Setup(p => p.GetFeatures()).Returns(featuresList);
            mockFeature1.Setup(f => f.Clear());
            mockFeature2.Setup(f => f.Clear());
            mockFeature3.Setup(f => f.Clear());

            var manager = new FeaturesManager(_mockProvider.Object);
            manager.ClearAll();

            mockFeature1.Verify();
            mockFeature2.Verify();
            mockFeature3.Verify();
        }
    }
}
