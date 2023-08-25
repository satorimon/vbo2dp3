using System;
using System.IO;
using System.Text;
using vbo2dp3.GPSLogLib;

namespace vbo2dp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("引数が有りません。処理を終了します。");
                    return;
                }
                foreach (var s in args)
                {
                    Console.WriteLine(s);
                    var inputPath = s;
                    var filename = Path.GetFileNameWithoutExtension(inputPath);
                    var outputPath = Path.GetDirectoryName(inputPath) + Path.DirectorySeparatorChar
                        + filename + ".dp3";
                    IEnumerable<GpsRecord> records;

                    var ext = Path.GetExtension(inputPath);
                    if ( ext == ".vbo" || ext == ".VBO")
                    {
                        records = Vbo2GpsRecord.Read(inputPath);
                    }
                    else if(ext == ".csv" || ext == ".CSV")
                    {
                        records = RaceChronoCsv2GpsRecords.Read(inputPath);
                    }
                    else
                    {
                        Console.WriteLine("拡張子は.vboまたは.csvのファイルのみ受け付けます。");
                        return;
                    }
                    var bytes = dp3Converter.Vbo2dp3(records);
                    using (var stream = File.Open(outputPath, FileMode.Create))
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(bytes.ToArray());
                        writer.Flush();
                        writer.Close();

                    }

                }
            }catch(Exception ex)
            {
                Console.WriteLine("例外が発生したため処理を中止しました。");
                Console.WriteLine(ex);
                Console.WriteLine("処理を中断するには何かキーを押してください。");
                Console.ReadKey();

            }
        }
    }
}