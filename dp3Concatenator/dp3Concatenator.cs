using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vbo2dp3.GPSLogLib;

namespace dp3Concatenator
{
    public static class dp3Concatenator
    {
        static string AddConcatToFileName(string? path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
            }

            // パスからファイル名を取得
            string fileName = Path.GetFileNameWithoutExtension(path)!; // null 許容型から null 非許容型に変換

            string extension = Path.GetExtension(path)!; // null 許容型から null 非許容型に変換

            // 新しいファイル名を作成
            string newFileName = $"{fileName}_concat{extension}";

            // 新しいパスを構築して返す
            string directory = Path.GetDirectoryName(path)!; // null 許容型から null 非許容型に変換
            return Path.Combine(directory, newFileName);
        }
        public static void DoConcatenator(IEnumerable<string> paths)
        {
            var gpsRecords = new List<IEnumerable<GpsRecord>>();
            foreach(var path in paths)
            {
                gpsRecords.Add(dp3Reader.Read(path));
            }
            gpsRecords = gpsRecords.OrderBy(x => x.First().Date).ToList();
            var concatRecords = new List<GpsRecord>();
            foreach(var gpsRecord in gpsRecords)
            {
                concatRecords.AddRange(gpsRecord);
            }
            var concateBytes = dp3Converter.GpsRecord2dp3(concatRecords);

            File.WriteAllBytes(AddConcatToFileName(paths.First()), concateBytes.ToArray());

        }
    }
}
