using Microsoft.VisualStudio.TestTools.UnitTesting;
using vbo2dp3.GPSLogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbo2dp3.GPSLogLib.Tests
{
    [TestClass()]
    public class dp3ReaderTests
    {
        [TestMethod()]
        public void ConvertDateTest()
        {
            var testData = dp3Reader.ConvertDate(new byte[] { 0x00, 0x0D, 0xF3, 0x10 });
            Assert.IsTrue(testData == new DateTime(1, 1, 1, 9, 14, 19, 200));
        }

        [TestMethod()]
        public void ConvertLongitudeAndLatitudeTest()
        {
            var testData = dp3Reader.ConvertLongitudeAndLatitude(new byte[] { 0x03, 0xCD, 0x98, 0xD3 });
            Assert.IsTrue(testData == (63805651.0 / 460800.0));
        }

        [TestMethod()]
        public void ConvertSpeedTest()
        {
            var testData = dp3Reader.ConvertSpeedAndHeight(new byte[] { 0x00, 0x32 });
            Assert.IsTrue(testData == (63805651.0 / 460800.0));
        }
    }
}