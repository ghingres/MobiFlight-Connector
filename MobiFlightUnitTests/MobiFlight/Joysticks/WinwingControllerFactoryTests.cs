using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobiFlight.Joysticks.Winwing;

namespace MobiFlight.Joysticks.Tests
{
    [TestClass()]
    public class WinwingControllerFactoryTests
    {
        private const int WINWING_VENDOR_ID = 0x4098;
        private const int OTHER_VENDOR_ID = 0x1234;

        [TestMethod()]
        public void IsWinwingDevice_WithWinwingFCU_ReturnsTrue()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(WINWING_VENDOR_ID, WinwingConstants.PRODUCT_ID_FCU_ONLY);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithWinwingCDU_ReturnsTrue()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(WINWING_VENDOR_ID, WinwingConstants.PRODUCT_ID_MCDU_CPT);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithWinwingPAP3_ReturnsTrue()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(WINWING_VENDOR_ID, WinwingConstants.PRODUCT_ID_PAP3_ONLY);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithWinwingAirbusThrottle_ReturnsTrue()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(WINWING_VENDOR_ID, WinwingConstants.PRODUCT_ID_AIRBUS_THROTTLE_L);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithWinwingAirbusStick_ReturnsTrue()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(WINWING_VENDOR_ID, WinwingConstants.PRODUCT_ID_AIRBUS_STICK_L);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithWinwingPDC3_ReturnsTrue()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(WINWING_VENDOR_ID, WinwingConstants.PRODUCT_ID_3NPDCL);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithWinwingECAM_ReturnsTrue()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(WINWING_VENDOR_ID, WinwingConstants.PRODUCT_ID_ECAM);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithWrongVendorId_ReturnsFalse()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(OTHER_VENDOR_ID, WinwingConstants.PRODUCT_ID_FCU_ONLY);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithUnknownProductId_ReturnsFalse()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(WINWING_VENDOR_ID, 0x9999);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsWinwingDevice_WithWrongVendorAndProductId_ReturnsFalse()
        {
            var result = WinwingControllerFactory.IsWinwingDevice(OTHER_VENDOR_ID, 0x9999);
            Assert.IsFalse(result);
        }
    }
}
