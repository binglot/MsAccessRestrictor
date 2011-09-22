using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MsAccessRestrictor;
using MsAccessRestrictor.Interfaces;
using MsAccessRestrictor.Properties;
using Application = Microsoft.Office.Interop.Access.Application;

namespace MsAccessRestrictorTests {
    [TestClass]
    public class StartupChecker_Tests {
        const string ProcessName = "MSACCESS";
        
        StartupChecker _startupChecker;
        Mock<Settings> _mockSettings;
        Mock<IMessageWriter> _mockMessageWriter;
        Mock<IProcess> _mockProcess;
        

        [TestInitialize]
        public void Setup() {
            _mockSettings = new Mock<Settings>();
            _mockMessageWriter = new Mock<IMessageWriter>();
            _mockProcess = new Mock<IProcess>();
            _startupChecker = new StartupChecker(_mockSettings.Object, _mockMessageWriter.Object, _mockProcess.Object);
        }

        [TestCleanup]
        public void Cleanup() {
            _mockSettings.VerifyAll();
            _mockMessageWriter.VerifyAll();
            _mockProcess.VerifyAll();
        }

        [TestMethod]
        public void Having_OpenMsAccess_set_to_true_and_a_running_instance_of_MsAccess_throws_an_error() {
            const string errorMessage = "Close MsAccess before running the application or change your settings.";
            const int processesNumber = 1;
            const bool settingValue = false;

            SetTheOpenMsAccessMock(errorMessage, processesNumber, settingValue);
        }

        [TestMethod]
        public void Having_OpenMsAccess_set_to_false_and_no_instance_of_MsAccess_throws_an_error() {
            const string errorMessage = "Open MsAccess before running the application or change your settings.";
            const int processesNumber = 0;
            const bool settingValue = false;

            SetTheOpenMsAccessMock(errorMessage, processesNumber, settingValue);
        }

        [TestMethod]
        public void Having_OpenMsAccess_set_to_false_and_more_than_one_running_MsAccess_throws_an_error() {
            const string errorMessage = "Open MsAccess before running the application or change your settings.";
            var processesNumber = new Random().Next(2, 10);
            const bool settingValue = false;

            SetTheOpenMsAccessMock(errorMessage, processesNumber, settingValue);
        }

        private void SetTheOpenMsAccessMock(string errorMessage, int processesNumber, bool settingValue) {
            Expression<Func<Settings, bool>> settingsExpression = m => m.OpenMsAccess;

            SetMsAccessMocks(errorMessage, processesNumber, settingsExpression, settingValue);
        }

        [TestMethod]
        public void Having_OpenDbFile_set_to_true_and_the_file_to_open_not_exist_will_throw_an_error() {
            //
        }

        private void SetMsAccessMocks(string errorMessage, int processesNumber, Expression<Func<Settings, bool>> settingsExpression, bool settingValue) {
            var processes = new Process[processesNumber];

            _mockProcess.Setup(p => p.GetProcessesByName(ProcessName)).Returns(processes);
            _mockSettings.Setup(settingsExpression).Returns(settingValue);
            _mockMessageWriter.Setup(m => m.ShowError(errorMessage));

            _startupChecker.Run();
        }
    }
}