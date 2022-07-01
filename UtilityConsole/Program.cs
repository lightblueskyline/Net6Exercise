namespace UtilityConsole;
class Program
{
    static void Main(string[] args)
    {
        #region MyRegion
        Console.WriteLine(String.Format("{0:X4}", 2000));
        Console.WriteLine(0x07E4.ToString("N"));
        #endregion
    }
}
