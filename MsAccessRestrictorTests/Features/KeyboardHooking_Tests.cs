//using Moq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MsAccessRestrictor;
//using MsAccessRestrictor.Features;
//using MsAccessRestrictor.Interfaces;

//namespace MsAccessRestrictorTests.Features {
//    [TestClass]
//    public class KeyboardHooking_Tests {
//        Mock<IPasswordForm> _mockPasswordForm;
//        KeyboardHooking _keyboardHooking;
//        Mock<WinApi> _mockWinApi;

//        [TestInitialize]
//        public void Setup() {
//            _mockPasswordForm = new Mock<IPasswordForm>();
//            //_keyboardHooking = new KeyboardHooking(_mockPasswordForm.Object);
//            _mockWinApi = new Mock<WinApi>();
//        }

//        [TestCleanup]
//        public void Cleanup() {
//            _mockPasswordForm.Verify();
//            _mockWinApi.Verify();
//        }

//        [TestMethod]
//        public void TestMethod() {
//            //_mockWinApi.
//        }
//    }
//}