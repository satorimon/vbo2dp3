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
            var concateFiles = paths.Skip(1).ToArray();
            var concateBytes = new List<byte>();
            foreach (string path in concateFiles)
            {
                FileCheckUtil.CheckExistsAndExtension(path, ".dp3");



                using (var fs = new FileStream(path, FileMode.Open))
                using (var reader = new BinaryReader(fs))
                {
                    fs.Seek(0x100, SeekOrigin.Begin);
                    concateBytes.AddRange(reader.ReadBytes((int)(fs.Length - 0x100)));

                }
            }

            byte[] data = File.ReadAllBytes(paths.First());
            byte[] combinedArray = data.Concat(concateBytes).ToArray();

            File.WriteAllBytes(AddConcatToFileName(paths.First()), combinedArray);

        }
    }
}
