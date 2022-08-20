namespace ProcessCsv.Services;

public class PrintToConsoleService : IPrintService 
{
    public void PrintLine(string str)
    {
        Console.WriteLine(str);
    }
}