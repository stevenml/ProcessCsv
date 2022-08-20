namespace ProcessCsvTests.FileProcessorsTests;

using System.Collections.Generic;
using ProcessCsv.Services;

public class PrintToStringListService : IPrintService
{
    public readonly List<string> PrintedStringList = new List<string>();
    public void PrintLine(string str)
    {
        PrintedStringList.Add(str);
    }
}