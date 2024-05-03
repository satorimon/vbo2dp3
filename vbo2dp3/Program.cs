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
                Console.WriteLine("RaceChronoから出力したVBO、CSV(Version2)をLAP+(dp3)形式に変換します。");
                string[] files;
                if(args.Length > 0)
                {
                    files = args;
                }
                else
                {
                    Console.WriteLine("ファイルのパス、複数の場合はフォルダのパスを入力してください。");
                    Console.Write(">");
                    var line = Console.ReadLine();
                    if(line == null) 
                    {
                        Console.WriteLine("パスが空です。処理を中止します。");
                        return;
                    }
                    if (File.Exists(line))
                    {
                        files = new string[] { line };
                    }
                    else if(Directory.Exists(line))
                    {
                        files = Directory.GetFiles(line)
                            .Where(file => file.EndsWith(".vbo") || file.EndsWith(".csv"))
                            .ToArray();
                    }
                    else
                    {
                        Console.WriteLine("入力されたパスは存在しません。処理を中止します。");
                        return;
                    }

                }
                if (files.Length < 1)
                {
                    Console.WriteLine("処理対象がありません。処理を終了します。");
                    return;
                }
                foreach (var s in files)
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
                    var bytes = dp3Converter.GpsRecord2dp3(records);
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