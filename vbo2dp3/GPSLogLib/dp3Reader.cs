using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vbo2dp3.GPSLogLib
{
    public class dp3Reader
    {
        public static IEnumerable<GpsRecord> Read(string path)
        {
            FileCheckUtil.CheckExistsAndExtension(path, ".dp3");

            var rtnList = new List<GpsRecord>();

            using (var fs = new FileStream(path, FileMode.Open))
            using (var reader = new BinaryReader(fs))
            {
                fs.Seek(0x100, SeekOrigin.Begin);
                byte[] buffer = reader.ReadBytes((int)(fs.Length - 0x100));
                return ProcessBinaryData(buffer);
            }
        }
        public static IEnumerable<GpsRecord> ProcessBinaryData(byte[] data)
        {
            var records = new List<GpsRecord>();

            for(int i = 0; i < data.Length; i+=(4+4+4+2+2)) 
            {
                var record = new GpsRecord();
                record.Date = ConvertDate(data.Skip(i).Take(4).ToArray());
                record.Longitude = ConvertLongitudeAndLatitude(data.Skip(i + 4).Take(4).ToArray());
                record.Latitude = ConvertLongitudeAndLatitude(data.Skip(i + 8).Take(4).ToArray());
                record.Speed = ConvertSpeedAndHeight(data.Skip(i + 12).Take(2).ToArray());
                record.Height = ConvertSpeedAndHeight(data.Skip(i + 14).Take(2).ToArray());
                records.Add(record);
            }

            return records;

        }

        private static uint Convert4bytes(byte[] array)
        {
            if (array.Length != 4)
            {
                throw new InvalidDataException("データ数が不正です");
            }
            // バイト列を数値に変換して文字列にする
            Array.Reverse(array);
            return BitConverter.ToUInt32(array, 0);
        }

        private static uint Convert2bytes(byte[] array)
        {
            if (array.Length != 2)
            {
                throw new InvalidDataException("データ数が不正です");
            }
            // バイト列を数値に変換して文字列にする
            Array.Reverse(array);
            return BitConverter.ToUInt16(array, 0);
        }


        public static DateTime ConvertDate(byte[] data)
        {

            uint value = Convert4bytes(data);
            string stringValue = value.ToString().PadLeft(8, '0');
            // ルールに基づいて時間情報を抽出
            int millisecond = int.Parse(stringValue.Substring(stringValue.Length - 1)) * 100;
            int second = int.Parse(stringValue.Substring(stringValue.Length - 3, 2));
            int minute = int.Parse(stringValue.Substring(stringValue.Length - 5, 2));
            int hour = int.Parse(stringValue.Substring(stringValue.Length - 7, 2));

            // DateTime型に変換
            return new DateTime(1, 1, 1, hour, minute, second, millisecond);

        }

        public static double ConvertLongitudeAndLatitude(byte[] array)
        {
            uint value = Convert4bytes(array);
            return value / 460800.0;
        }

        public static double ConvertSpeedAndHeight(byte[] array)
        {
            uint value = Convert2bytes(array);
            return value / 10.0;
        }


    }
}
