﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using vbo2dp3.GPSLogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbo2dp3.GPSLogLib.Tests
{
    [TestClass()]
    public class dp3ConverterTests
    {
        [TestMethod()]
        public void Vbo2dp3Test()
        {
           var record = new GpsRecord();
            record.Longitude = (double)0x03D7EFA7 / 460800.0;
            record.Latitude = (double)0x0103FADF / 460800.0;
            record.Date = new DateTime(2023, 04, 30, 10, 30, 26, 400);
            record.Speed = 0.9;

            var bytes = dp3Converter.Vbo2dp3(new[] { record });

            //Header read
            //var headerBytes = new byte[0x100];

            //fs.Read(headerBytes, 0, 0x100);

            //res.Header = headerBytes.ToString();

            //var today = new DateTime(recordingDay.Value.Year, recordingDay.Value.Month, recordingDay.Value.Day);

            //var records = new List<GpsRecord>();
            var data4bytes = new byte[4];
            var data2bytes = new byte[2];
            //while (true)
            //{
                //convert time
                data4bytes = bytes.Skip(0x100).Take(4).ToArray();

                var time_str_source = BitConverter.ToInt32(data4bytes[0..4].Reverse().ToArray());
                var timestr = time_str_source.ToString("0000000");

            Assert.IsTrue(timestr == "1030264");

            ////convert longitude
            data4bytes = bytes.Skip(0x104).Take(4).ToArray();

            Assert.IsTrue(0x03D7EFA7 == BitConverter.ToInt32(data4bytes[0..4].Reverse().ToArray())) ;
            data4bytes = bytes.Skip(0x108).Take(4).ToArray();
            Assert.IsTrue(0x0103FADF == BitConverter.ToInt32(data4bytes[0..4].Reverse().ToArray()));

            data2bytes = bytes.Skip(0x10C).Take(2).ToArray();
            Assert.IsTrue(9 == BitConverter.ToInt16(data2bytes[0..2].Reverse().ToArray()));



            //}
        }
    }
}