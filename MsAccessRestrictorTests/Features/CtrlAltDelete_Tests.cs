using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;
using MsAccessRestrictor.Features;

namespace MsAccessRestrictorTests.Features {
    [TestClass]
    public class CtrlAltDelete_Tests {
        CtrlAltDelete _ctrlAltDelete;
        
        [TestInitialize]
        public void Setup() {
            _ctrlAltDelete = new CtrlAltDelete();
        }

        [TestCleanup]
        public void Cleanup() {
            //
        }

        //[TestMethod]
        //public void Disabling_sets_a_registry_key_to_one() {
        //    var mockRegistry = new Mock<RegistryKey>();
        //    _ctrlAltDelete.Run();
        
        //    mockRegistry.Verify();
        //}
    }
}