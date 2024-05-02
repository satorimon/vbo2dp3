using Microsoft.VisualStudio.TestTools.UnitTesting;
using dp3Alignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dp3Alignment.Tests
{
    [TestClass()]
    public class dp3AlignmentTests
    {
        [TestMethod()]
        public void DoAlignmentTest()
        {
            string[] paths = new string[] { "2024-0428_wada1.dp3", "20240428_hirakawa1.dp3" };

            dp3Alignment.DoAlignment(paths);
            Assert.IsTrue(File.Exists("2024-0428_wada1_offset.dp3"));
            Assert.IsTrue(File.Exists("20240428_hirakawa1_offset.dp3"));
            File.Delete("2024-0428_wada1_offset.dp3");
            File.Delete("2024-20240428_hirakawa1_offset.dp3");

        }
    }
}