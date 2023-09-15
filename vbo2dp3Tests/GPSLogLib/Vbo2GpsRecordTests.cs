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
    public class Vbo2GpsRecordTests
    {
        [TestMethod()]
        public void ReadVboTest()
        {

            var result = Vbo2GpsRecord.Read("session_20230326_141947_test.vbo");

            Assert.IsTrue(result is not null);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Count() == 1);
            var record = result.First();
            Assert.IsTrue(record.Date.Year == 2023);
            Assert.IsTrue(record.Date.Month == 3);
            Assert.IsTrue(record.Date.Day == 26);
            Assert.IsTrue(record.Date.Hour == 14);
            Assert.IsTrue(record.Date.Minute == 19);
            Assert.IsTrue(record.Date.Second == 47);
            Assert.IsTrue(record.Date.Millisecond == 400);


            Assert.IsTrue(record.Latitude == 36.974474833333339);
            Assert.IsTrue(record.Longitude == 139.93824283333333);
            Assert.IsTrue(record.Speed == 11.254);

            result = Vbo2GpsRecord.Read("session_20230430_095050_test.vbo");

            Assert.IsTrue(result.Count() == 1);
            record = result.First();
            Assert.IsTrue(record.Date.Year == 2023);
            Assert.IsTrue(record.Date.Month == 4);
            Assert.IsTrue(record.Date.Day == 30);
            Assert.IsTrue(record.Date.Hour == 10);
            Assert.IsTrue(record.Date.Minute == 30);
            Assert.IsTrue(record.Date.Second == 27);
            Assert.IsTrue(record.Date.Millisecond == 100);

            result = Vbo2GpsRecord.Read("2023Rd3①.vbo");

            Assert.IsTrue(result.Count() == 1);
            record = result.First();
            Assert.IsTrue(record.Date.Year == 2023);
            Assert.IsTrue(record.Date.Month == 4);
            Assert.IsTrue(record.Date.Day == 30);
            Assert.IsTrue(record.Date.Hour == 10);
            Assert.IsTrue(record.Date.Minute == 30);
            Assert.IsTrue(record.Date.Second == 27);
            Assert.IsTrue(record.Date.Millisecond == 100);

        }

        [TestMethod()]
        public void ReadTest()
        {

            var result = RaceChronoCsv2GpsRecords.Read("session_20230806_132338_20230806_地区戦野沢_resume3_v2.csv");

            Assert.IsTrue(result is not null);
            Assert.IsTrue(result.Any());
            var record = result.First();
            Assert.IsTrue(record.Date.Year == 2023);
            Assert.IsTrue(record.Date.Month == 8);
            Assert.IsTrue(record.Date.Day == 6);
            Assert.IsTrue(record.Date.Hour == 13);
            Assert.IsTrue(record.Date.Minute == 23);
            Assert.IsTrue(record.Date.Second == 38);
            Assert.IsTrue(record.Date.Millisecond == 750);


        }
    }
}