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
            RunOnAll(feature => feature.Run());
        }

        [TestMethod]
        public void Disable_all_the_features() {
            RunOnAll(feature => feature.Clear());
        }

        void RunOnAll(Expression<Action<IFeature>> action) {
            var mockFeaturesList = GetMockFeaturesList();
            var featuresList = mockFeaturesList.ConvertAll(f => f.Object);

            SetUpMockFeatures(mockFeaturesList, action);
            _mockFeaturesProvider.Setup(p => p.GetFeatures()).Returns(featuresList);

            var manager = new FeaturesManager(_mockFeaturesProvider.Object);
            manager.SetAll();

            VerifyMockFeatures(mockFeaturesList);
        }

        static List<Mock<IFeature>> GetMockFeaturesList() {
            var mockFeature1 = new Mock<IFeature>();
            var mockFeature2 = new Mock<IFeature>();
            var mockFeature3 = new Mock<IFeature>();
            var mocksList = new List<Mock<IFeature>> { mockFeature1, mockFeature2, mockFeature3 };

            return mocksList;
        }

        static void SetUpMockFeatures(IEnumerable<Mock<IFeature>> features, Expression<Action<IFeature>> action) {
            foreach (var feature in features) {
                feature.Setup(action);
            }
        }

        static void VerifyMockFeatures(IEnumerable<Mock<IFeature>> featuresList) {
            foreach (var feature in featuresList) {
                feature.Verify();
            }
        }
    }
}
