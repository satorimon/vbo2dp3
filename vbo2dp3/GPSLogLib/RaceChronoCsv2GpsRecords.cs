using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace vbo2dp3.GPSLogLib
{
    public class RaceChronoCsv2GpsRecords
    {

        public static IEnumerable<GpsRecord> Read(string path)
        {
            FileCheckUtil.CheckExistsAndExtension(path, ".csv");


            var rtnList = new List<GpsRecord>();

            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                var header = string.Empty;
                var line = string.Empty;
                do
                {
                    line = sr.ReadLine() ?? string.Empty;
                    header += line;
                    header += "\r\n";
                }
                while (!line.StartsWith("Time (s)"));
                var columnNames = line;
                var splitNames = columnNames.Split(",", StringSplitOptions.RemoveEmptyEntries);

                Func<string, int> f = (name) =>
                {
                    return splitNames.Select((item, index) => (item, index))
                    .Where(pare => string.Compare(pare.item, name, true) == 0)
                    .FirstOrDefault().index;
                };

                var timeIndex = f("Time (s)");

                var latIndex = f("Latitude (deg)");

                var longIndex = f("Longitude (deg)");

                var vIndex = f("Speed (m/s)");

                DateTime lastDate = new DateTime();

                do
                {
                    line = sr.ReadLine() ?? string.Empty;
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    var lineSplited = line.Split(",", StringSplitOptions.None);

                    var record = new GpsRecord();

                    var time_ms = double.Parse(lineSplited[timeIndex]);

                    var dto = DateTimeOffset.FromUnixTimeMilliseconds((long)(time_ms * 1000)).LocalDateTime;
                    var tempDate = new DateTime(dto.Year, dto.Month, dto.Day, dto.Hour, dto.Minute, dto.Second, dto.Millisecond);
                    if (tempDate - lastDate == TimeSpan.Zero)
                    {
                        continue;
                    }

                    record.Date = tempDate;
                    lastDate = tempDate;


                    var latStr = lineSplited[latIndex];
                    var lat = double.Parse(latStr);
                    record.Latitude = lat;


                    var longStr = lineSplited[longIndex];
                    var longitude = double.Parse(longStr);
                    record.Longitude = longitude;


                    var vStr = lineSplited[vIndex];
                    var v = double.Parse(vStr);
                    record.Speed = v * 3600.0 / 1000.0;

                    rtnList.Add(record);
                } while (!sr.EndOfStream);
                return rtnList;

            }

        }
    }
}
