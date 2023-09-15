using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace vbo2dp3.GPSLogLib
{
    public static class Vbo2GpsRecord
    {

        public static IEnumerable<GpsRecord> Read(string path)
        {
            FileCheckUtil.CheckExistsAndExtension(path, ".vbo");
            var year = DateTime.Today.Year;
            var month = DateTime.Today.Month;
            var day = DateTime.Today.Day;
            var reg = new Regex("^session_\\d{8}_\\d{6}.*");
            if(reg.Match(path).Success)
            {
                year = int.Parse(path.Substring(8, 4));
                month = int.Parse(path.Substring(12, 2));
                day = int.Parse(path.Substring(14, 2));
            }
            
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
                while (!line.StartsWith("[column names]"));
                var columnNames = sr.ReadLine() ?? string.Empty;
                var splitNames = columnNames.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                Func<string,int> f = (name) =>
                {
                    return splitNames.Select((item, index) => (item, index))
                    .Where(pare => string.Compare(pare.item, name, true) == 0)
                    .FirstOrDefault().index;
                };

                var timeIndex = f("time");

                var latIndex = f("lat");

                var longIndex = f("long");

                var vIndex = splitNames.Select((item, index) => (item, index))
                    .Where(pare => string.Compare(pare.item, "velocity", true) == 0)
                    .FirstOrDefault().index;

                while (sr.ReadLine()?.StartsWith("[data]") == false) ;

                do
                {
                    line = sr.ReadLine() ?? string.Empty;
                    if(string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    var lineSplited = line.Split(" ", StringSplitOptions.None);

                    var record = new GpsRecord();


                    var timeStr = lineSplited[timeIndex];

                    var time = double.Parse(timeStr);
                    int hour = int.Parse(timeStr.Substring(0, 2))  + 9;
                    if(hour > 23)
                    {
                        hour = hour % 24;
                    }
                    int min = int.Parse(timeStr.Substring(2, 2));
                    int sec = int.Parse(timeStr.Substring(4, 2));
                    int millisec = int.Parse(timeStr.Substring(7, 2))*10;
                    
                    record.Date = new DateTime(year, month, day, hour, min, sec, millisec);


                    var latStr = lineSplited[latIndex];
                    var lat = double.Parse(latStr);
                    record.Latitude = lat / 60.0;


                    var longStr = lineSplited[longIndex];
                    var longitude = double.Parse(longStr);
                    record.Longitude = longitude / -60.0;


                    var vStr = lineSplited[vIndex];
                    var v = double.Parse(vStr);
                    record.Speed = v;

                    rtnList.Add(record);
                } while (!sr.EndOfStream);
                return rtnList;

            }

        }
    }
}
