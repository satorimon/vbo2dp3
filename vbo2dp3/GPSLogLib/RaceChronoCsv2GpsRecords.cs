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
                var heightIndex = f("Altitude (m)");

                DateTime lastDate = new DateTime();

                double lastLatitude = 0.0, lastLongitude = 0.0, lastSpeed = 0.0, lastHeight = 0.0;

                do
                {

                        line = sr.ReadLine() ?? string.Empty;
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        var lineSplited = line.Split(",", StringSplitOptions.None);

                        var record = new GpsRecord();
                    try
                    {
                        var time_ms = double.Parse(lineSplited[timeIndex]);

                        var dto = DateTimeOffset.FromUnixTimeMilliseconds((long)(time_ms * 1000)).LocalDateTime;
                        var tempDate = new DateTime(dto.Year, dto.Month, dto.Day, dto.Hour, dto.Minute, dto.Second, dto.Millisecond);
                        if (tempDate - lastDate == TimeSpan.Zero)
                        {
                            continue;
                        }
                        record.Date = tempDate;
                        lastDate = tempDate;
                    }
                    catch(Exception e) when (e is ArgumentOutOfRangeException 
                    || e is ArgumentNullException
                    || e is FormatException
                    || e is OverflowException)
                    {
                        Console.Write("GPSレコードの変換に失敗しました : ");
                        Console.WriteLine(e);

                        continue;
                    }



                        Func<int, Tuple<double, double>> setFunc = (index) =>
                        {
                            double value = 0.0, lastValue = 0.0;
                            var tempStr = lineSplited[index];
                            double temp = 0.0;
                            if (double.TryParse(tempStr, out temp))
                            {
                                value = temp;
                                lastValue = temp;
                            }
                            else
                            {
                                value = lastValue;
                            }

                            return new Tuple<double, double>(value, lastValue);
                        };

                        (record.Latitude, lastLatitude) = setFunc(latIndex);
                        (record.Longitude, lastLongitude) = setFunc(longIndex);
                        (record.Speed, lastSpeed) = setFunc(vIndex);
                        (record.Height, lastHeight) = setFunc(heightIndex);


                        record.Speed = record.Speed * 3600.0 / 1000.0;

                        rtnList.Add(record);
 
                } while (!sr.EndOfStream);
                return rtnList;

            }

        }
    }
}
