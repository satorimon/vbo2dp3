// See https://aka.ms/new-console-template for more information


try
{
    if (args.Length < 1)
    {
        Console.WriteLine("引数が有りません。処理を終了します。");
        return;
    }
    dp3Concatenator.dp3Concatenator.DoConcatenator(args);

}
catch (Exception ex)
{
    Console.WriteLine("例外が発生したため処理を中止しました。");
    Console.WriteLine(ex);
    Console.WriteLine("処理を中断するには何かキーを押してください。");
    Console.ReadKey();

}
