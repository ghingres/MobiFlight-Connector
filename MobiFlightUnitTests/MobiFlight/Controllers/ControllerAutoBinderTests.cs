using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobiFlight.Base;
using MobiFlight.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace MobiFlight.Tests.Controllers
{
    [TestClass]
    public class ControllerAutoBinderTests
    {
        #region Scenario Tests

        [TestMethod]
        public void Scenario1_ExactMatch_ReturnsMatchAndNoChanges()
        {
            // Arrange
            var connectedControllers = new List<string> { "MyBoard # / SN-1234567890" };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("MyBoard # / SN-1234567890")
            };
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);
            var serialMappings = binder.ApplyAutoBinding(configItems, results);

            // Assert
            Assert.HasCount(1, results);
            var (status, boundController) = results["MyBoard # / SN-1234567890"];
            Assert.AreEqual(ControllerBindingStatus.Match, status);
            Assert.IsEmpty(serialMappings);
            Assert.AreEqual("MyBoard # / SN-1234567890", configItems[0].ModuleSerial);
            Assert.AreEqual("MyBoard # / SN-1234567890", boundController);
        }

        [TestMethod]
        public void Scenario2_SerialDiffers_ReturnsAutoBoundAndUpdatesSerial()
        {
            // Arrange
            var connectedControllers = new List<string> { "X1-Pro # / SN-NEW456" };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("X1-Pro # / SN-OLD123")
            };
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);
            var serialMappings = binder.ApplyAutoBinding(configItems, results);

            // Assert
            Assert.HasCount(1, results);
            var (status, boundController) = results["X1-Pro # / SN-OLD123"];
            Assert.AreEqual(ControllerBindingStatus.AutoBind, status);
            Assert.HasCount(1, serialMappings);
            Assert.AreEqual("X1-Pro # / SN-NEW456", serialMappings["X1-Pro # / SN-OLD123"]);
            Assert.AreEqual("X1-Pro # / SN-NEW456", configItems[0].ModuleSerial);
        }

        [TestMethod]
        public void Scenario3_NameDiffers_ReturnsAutoBoundAndUpdatesName()
        {
            // Arrange
            var connectedControllers = new List<string> { "NewBoardName # / SN-1234567890" };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("OldBoardName # / SN-1234567890")
            };
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);
            var serialMappings = binder.ApplyAutoBinding(configItems, results);

            // Assert
            Assert.HasCount(1, results);
            var (status, boundController) = results["OldBoardName # / SN-1234567890"];
            Assert.AreEqual(ControllerBindingStatus.AutoBind, status);
            Assert.HasCount(1, serialMappings);
            Assert.AreEqual("NewBoardName # / SN-1234567890", serialMappings["OldBoardName # / SN-1234567890"]);
            Assert.AreEqual("NewBoardName # / SN-1234567890", configItems[0].ModuleSerial);
        }

        [TestMethod]
        public void Scenario4_Missing_ReturnsMissingAndNoChanges()
        {
            // Arrange
            var connectedControllers = new List<string> { "DifferentBoard # / SN-9999" };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("X1-Pro # / SN-1234")
            };
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);
            var serialMappings = binder.ApplyAutoBinding(configItems, results);

            // Assert
            Assert.HasCount(1, results);
            var (status, boundController) = results["X1-Pro # / SN-1234"];
            Assert.AreEqual(ControllerBindingStatus.Missing, status);
            Assert.IsEmpty(serialMappings);
            Assert.AreEqual("X1-Pro # / SN-1234", configItems[0].ModuleSerial);
        }

        [TestMethod]
        public void Scenario5_MultipleMatches_RequiresManualBindAndNoChanges()
        {
            // Arrange
            var connectedControllers = new List<string>
            {
                "Joystick X #/JS-111111",
                "Joystick X #/JS-222222"
            };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("Joystick X #/JS-999999")
            };
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);
            var serialMappings = binder.ApplyAutoBinding(configItems, results);

            // Assert
            Assert.HasCount(1, results);
            var (status, boundController) = results["Joystick X #/JS-999999"];
            Assert.AreEqual(ControllerBindingStatus.RequiresManualBind, status);
            Assert.IsEmpty(serialMappings);
            Assert.AreEqual("Joystick X #/JS-999999", configItems[0].ModuleSerial);
        }

        #endregion

        #region Multiple Config Items Tests

        [TestMethod]
        public void AnalyzeBindings_MultipleConfigItems_AnalyzesAll()
        {
            // Arrange
            var connectedControllers = new List<string>
            {
                "Board1 # / SN-111",
                "Board2 # / SN-222"
            };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("Board1 # / SN-111"),
                CreateConfigItem("Board2 # / SN-OLD"),
                CreateConfigItem("Board3 # / SN-333")
            };
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);

            // Assert
            Assert.HasCount(3, results);
            var (status1, _) = results["Board1 # / SN-111"];
            var (status2, _) = results["Board2 # / SN-OLD"];
            var (status3, _) = results["Board3 # / SN-333"];

            Assert.AreEqual(ControllerBindingStatus.Match, status1);
            Assert.AreEqual(ControllerBindingStatus.AutoBind, status2);
            Assert.AreEqual(ControllerBindingStatus.Missing, status3);
        }

        [TestMethod]
        public void ApplyAutoBinding_MultipleConfigItems_UpdatesOnlyAutoBound()
        {
            // Arrange
            var connectedControllers = new List<string>
            {
                "Board1 # / SN-111",
                "Board2 # / SN-222"
            };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("Board1 # / SN-111"),
                CreateConfigItem("Board2 # / SN-OLD"),
                CreateConfigItem("Board3 # / SN-333")
            };
            var binder = new ControllerAutoBinder(connectedControllers);
            var bindingStatus = binder.AnalyzeBindings(configItems);

            // Act
            var serialMappings = binder.ApplyAutoBinding(configItems, bindingStatus);

            // Assert
            Assert.HasCount(1, serialMappings);
            Assert.AreEqual("Board1 # / SN-111", configItems[0].ModuleSerial, "Exact match unchanged");
            Assert.AreEqual("Board2 # / SN-222", configItems[1].ModuleSerial, "Auto-bound updated");
            Assert.AreEqual("Board3 # / SN-333", configItems[2].ModuleSerial, "Missing unchanged");
        }

        [TestMethod]
        public void ApplyAutoBinding_MultipleConfigItems_IgnoreMissingMatch()
        {
            // Arrange
            var connectedControllers = new List<string>
            {
                "Board1 # / SN-111",
            };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("Board1 # / SN-111"),
                CreateConfigItem("Board1 # / SN-OTHER")
            };
            var binder = new ControllerAutoBinder(connectedControllers);
            var bindingStatus = binder.AnalyzeBindings(configItems);

            // Act
            var serialMappings = binder.ApplyAutoBinding(configItems, bindingStatus);

            // Assert
            Assert.HasCount(0, serialMappings);
            Assert.AreEqual("Board1 # / SN-111", configItems[0].ModuleSerial, "Exact match unchanged");
            Assert.AreEqual("Board1 # / SN-OTHER", configItems[1].ModuleSerial, "Missing unchanged");
        }

        public void ApplyAutoBinding_MultipleConfigItems_IgnoreMissingMatchAndOrder()
        {
            // Arrange
            var connectedControllers = new List<string>
            {
                "Board1 # / SN-111",
            };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("Board1 # / SN-OTHER"),
                CreateConfigItem("Board1 # / SN-111")
            };
            var binder = new ControllerAutoBinder(connectedControllers);
            var bindingStatus = binder.AnalyzeBindings(configItems);

            // Act
            var serialMappings = binder.ApplyAutoBinding(configItems, bindingStatus);

            // Assert
            Assert.HasCount(1, serialMappings);
            Assert.AreEqual("Board1 # / SN-111", configItems[0].ModuleSerial, "Exact match unchanged");
            Assert.AreEqual("Board1 # / SN-OTHER", configItems[1].ModuleSerial, "Missing unchanged");
        }

        #endregion

        #region Duplicate Serials Tests

        [TestMethod]
        public void AnalyzeBindings_DuplicateSerials_ReturnsOnlyUnique()
        {
            // Arrange
            var connectedControllers = new List<string> { "Board # / SN-NEW" };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("Board # / SN-OLD"),
                CreateConfigItem("Board # / SN-OLD"),  // Duplicate
                CreateConfigItem("Board # / SN-OLD")   // Duplicate
            };
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);

            // Assert
            Assert.HasCount(1, results, "Should only analyze unique serials");
            var (status, _) = results["Board # / SN-OLD"];
            Assert.AreEqual(ControllerBindingStatus.AutoBind, status);
        }

        [TestMethod]
        public void ApplyAutoBinding_DuplicateSerials_UpdatesAllInstances()
        {
            // Arrange
            var connectedControllers = new List<string> { "Board # / SN-NEW" };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem("Board # / SN-OLD"),
                CreateConfigItem("Board # / SN-OLD"),
                CreateConfigItem("Board # / SN-OLD")
            };
            var binder = new ControllerAutoBinder(connectedControllers);
            var bindingStatus = binder.AnalyzeBindings(configItems);

            // Act
            binder.ApplyAutoBinding(configItems, bindingStatus);

            // Assert
            Assert.IsTrue(configItems.All(c => c.ModuleSerial == "Board # / SN-NEW"),
                "All duplicate serials should be updated");
        }

        #endregion

        #region Empty and Null Tests

        [TestMethod]
        public void AnalyzeBindings_EmptyConfigItems_ReturnsEmptyDictionary()
        {
            // Arrange
            var connectedControllers = new List<string> { "Board # / SN-123" };
            var configItems = new List<IConfigItem>();
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);

            // Assert
            Assert.IsEmpty(results);
        }

        [TestMethod]
        public void AnalyzeBindings_IgnoresEmptyAndDashSerials()
        {
            // Arrange
            var connectedControllers = new List<string> { "Board # / SN-123" };
            var configItems = new List<IConfigItem>
            {
                CreateConfigItem(""),
                CreateConfigItem("-"),
                CreateConfigItem(null),
                CreateConfigItem("Board # / SN-123")
            };
            var binder = new ControllerAutoBinder(connectedControllers);

            // Act
            var results = binder.AnalyzeBindings(configItems);

            // Assert
            Assert.HasCount(1, results);
            Assert.IsTrue(results.ContainsKey("Board # / SN-123"));
        }

        [TestMethod]
        public void Constructor_NullConnectedControllers_HandlesGracefully()
        {
            // Arrange & Act
            var binder = new ControllerAutoBinder(null);
            var configItems = new List<IConfigItem> { CreateConfigItem("Board # / SN-123") };

            // Act
            var results = binder.AnalyzeBindings(configItems);

            // Assert
            Assert.HasCount(1, results);
            var (status, _) = results["Board # / SN-123"];
            Assert.AreEqual(ControllerBindingStatus.Missing, status);
        }

        #endregion

        #region Helper Methods

        private IConfigItem CreateConfigItem(string moduleSerial)
        {
            return new OutputConfigItem
            {
                ModuleSerial = moduleSerial,
                Active = true,
                GUID = System.Guid.NewGuid().ToString()
            };
        }

        #endregion

        #region GetTypeAndName Tests

        [TestMethod]
        public void GetTypeAndName_StandardFormat_ReturnsTypeAndName()
        {
            // Arrange
            var serial = "Board #/ SN-1234567890";

            // Act
            var result = ControllerAutoBinder.GetTypeAndName(serial);

            // Assert
            Assert.AreEqual("Board #", result);
        }

        [TestMethod]
        public void GetTypeAndName_WithWhitespace_TrimsProperly()
        {
            // Arrange
            var serial = "  X1-Pro #  / SN-ABC123  ";

            // Act
            var result = ControllerAutoBinder.GetTypeAndName(serial);

            // Assert
            Assert.AreEqual("X1-Pro #", result);
        }

        [TestMethod]
        public void GetTypeAndName_NoSeparator_ReturnsFullString()
        {
            // Arrange
            var serial = "BoardWithoutSeparator";

            // Act
            var result = ControllerAutoBinder.GetTypeAndName(serial);

            // Assert
            Assert.AreEqual("BoardWithoutSeparator", result);
        }

        [TestMethod]
        public void GetTypeAndName_EmptyString_ReturnsEmptyString()
        {
            // Arrange
            var serial = "";

            // Act
            var result = ControllerAutoBinder.GetTypeAndName(serial);

            // Assert
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetTypeAndName_OnlySeparator_ReturnsEmptyString()
        {
            // Arrange
            var serial = "/ ";

            // Act
            var result = ControllerAutoBinder.GetTypeAndName(serial);

            // Assert
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetTypeAndName_MultipleSeparators_ReturnFirstPart()
        {
            // Arrange
            var serial = "Board #/ SN-123/ Extra";

            // Act
            var result = ControllerAutoBinder.GetTypeAndName(serial);

            // Assert
            Assert.AreEqual("Board #", result);
        }

        #endregion
    }
}