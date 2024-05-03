// See https://aka.ms/new-console-template for more information


try
{
    Console.WriteLine("複数のLAP+の1LAPデータを一つのファイルにまとめます。");
    string[] filepathes;
    if (args.Length > 0)
    {
        filepathes = args;
    }
    else
    {
        Console.Write("処理するフォルダのパスを入力して下さい: ");
        var line = Console.ReadLine();
        if (line != null && Directory.Exists(line))
        {
            filepathes = Directory.GetFiles(line, "*.dp3");
        }
        else
        {
            Console.WriteLine("入力が不正です。処理を終了します。");
            return;
        }
    }

    if (filepathes.Length < 1)
    {
        Console.WriteLine("処理するファイルが有りません。処理を終了します。");
        return;
    }
    dp3Concatenator.dp3Concatenator.DoConcatenator(filepathes);

}
catch (Exception ex)
{
    Console.WriteLine("例外が発生したため処理を中止しました。");
    Console.WriteLine(ex);
    Console.WriteLine("処理を中断するには何かキーを押してください。");
    Console.ReadKey();

}
