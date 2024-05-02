using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbo2dp3.GPSLogLib
{
    public static class dp3Converter
    {
        public static IEnumerable<GpsRecord> ExtractSequences(IEnumerable<GpsRecord> records)
        {
            var array = records.ToArray();
            var sequences = new List<GpsRecord>();

            int startIndex = -1;
            DateTime stopDate = DateTime.MinValue;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Speed > 20 && startIndex == -1)
                {
                    startIndex = i;

                    // 5を超えたところから戻る
                    while (startIndex > 0 && array[startIndex].Speed > 5.0)
                    {
                        startIndex--;
                    }
                }
                else if (startIndex > 0 && array[i].Speed < 1.0 && stopDate == DateTime.MinValue)
                {
                    stopDate = array[i].Date;
                }
                else if (startIndex > 0 && array[i].Speed < 1.0)
                {
                    var elapsed = array[i].Date - stopDate;
                    if (elapsed.TotalSeconds > 10)
                    {
                        sequences.AddRange(array.Skip(startIndex + 1).Take(i - startIndex - 1));
                        startIndex = -1;
                        stopDate = DateTime.MinValue;
                    }
                }
            }

            if (startIndex != -1)
            {
                sequences.AddRange(array.Skip(startIndex));
            }

            return sequences;
        }

        public static IEnumerable<byte> Vbo2dp3(IEnumerable<GpsRecord> records)
        {
            var rtnList = new List<byte>();
            //MEMO : Header write

            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,
                0x30, 0x30, 0x30, 0x30, 0x30, 0x30 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x70, 0x52, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            rtnList.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x30, 0x30,
                0x30, 0x30, 0x30, 0x30, 0x30, 0x30 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            rtnList.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            int lastTime = 0;

            var skipRecords = ExtractSequences(records).ToArray();
            foreach (var rec in skipRecords)
            {
                
                int time = rec.Date.Hour * 100000 + rec.Date.Minute * 1000 + rec.Date.Second * 10 + (int)(rec.Date.Millisecond / 100);
                int longitude = (int)(rec.Longitude * 460800.0);
                int latitude = (int)(rec.Latitude * 460800.0);
                int speed = (int)(rec.Speed * 10.0);
                int height = (int)(rec.Height * 10.0);

                Action<int> writeBytes4 = (data) =>
                {
                    var array4 = new byte[4];
                    var bytes = BitConverter.GetBytes(data) ?? new byte[] { 0 };
                    array4[0] = bytes.Length > 3 ? bytes[3] : (byte)0;
                    array4[1] = bytes.Length > 2 ? bytes[2] : (byte)0;
                    array4[2] = bytes.Length > 1 ? bytes[1] : (byte)0;
                    array4[3] = bytes.Length > 0 ? bytes[0] : (byte)0;
                    rtnList.AddRange(array4);
                };

                Action<int> writeBytes2 = (data) =>
                {
                    var array2 = new byte[2];
                    var bytes = BitConverter.GetBytes(data) ?? new byte[] { 0 };

                    array2[0] = bytes.Length > 1 ? bytes[1] : (byte)0;
                    array2[1] = bytes.Length > 0 ? bytes[0] : (byte)0;
                    rtnList.AddRange(array2);
                };

                if(time < lastTime + 2)
                {
                    continue;
                }
                lastTime = time;
                writeBytes4(time);
                writeBytes4(longitude);
                writeBytes4(latitude);
                writeBytes2(speed);
                writeBytes2(height);
                



            }

            return rtnList;

        }
    }
}
