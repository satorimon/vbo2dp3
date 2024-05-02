using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vbo2dp3.GPSLogLib;

namespace dp3Alignment
{
    /// <summary>
    /// 1Lap毎に分離されたdp3の開始位置を揃える
    /// </summary>
    public class dp3Alignment
    {
        static string AddOffsetToFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
            }

            // パスからファイル名を取得
            string fileName = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);

            // 新しいファイル名を作成
            string newFileName = $"{fileName}_offset{extension}";

            // 新しいパスを構築して返す
            string directory = Path.GetDirectoryName(path)!;
            return Path.Combine(directory, newFileName);
        }

        public static void DoAlignment(IEnumerable<string> paths)
        {
            var dp3s = new List<IEnumerable<GpsRecord>>();
            foreach (var file in paths)
            {
                if(!File.Exists(file))
                {
                    throw new InvalidDataException("ファイルが存在しません");
                }
                var records = dp3Reader.Read(file);
                if(records?.Any() != true)
                {
                    throw new InvalidDataException("ログが存在しません");
                }
                dp3s.Add(records);
            }

            var startSpeedMaxElement = dp3s.Select(item => item.First()).OrderBy(item => item.Speed).Last();

            for(int i = 0; i < dp3s.Count; i++)
            {
                var element = dp3s[i].First(item => item.Speed >= startSpeedMaxElement.Speed);
                var offsetLongitude = startSpeedMaxElement.Longitude - element.Longitude;
                var offsetLatitude = startSpeedMaxElement.Latitude - element.Latitude;
                var offsetHeight = startSpeedMaxElement.Height - element.Height;

                foreach(var x in dp3s[i]) 
                {
                    x.Longitude += offsetLongitude;
                    x.Latitude += offsetLatitude;
                    x.Height += offsetHeight;
                }

                var bytes = dp3Converter.Vbo2dp3(dp3s[i]);
                using (var stream = File.Open(AddOffsetToFileName(paths.ElementAt(i)), FileMode.Create))
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(bytes.ToArray());
                    writer.Flush();
                    writer.Close();

                }
            }


        }
    }
}
