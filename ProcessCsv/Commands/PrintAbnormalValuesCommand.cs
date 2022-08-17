namespace ProcessCsv.Commands;

using Cocona;
using FileProcessors;
using Helpers;

public class PrintAbnormalValuesCommand
{
    private int _normalRangePercentage;
    public PrintAbnormalValuesCommand ()
    {
    }

    [Command("print-abnormal-values", Description = "Print abnormal values n% above or below the median")]
    public void PrintAbnormalValues(
        [Option("normal-range-percentage", new[] { 'p' }, Description = "Please specify the normal value range percentage")]
        int normalRangePercent,
        [Option("path", new[] { 'd' }, Description = "Please specify the file directory path, or by default process all the files in current directory")]
        string directoryPath = "",
        [Option("file", new[] { 'f' }, Description = "Please specify the file path, or by default process all the files in current directory")]
        string filePath = ""
        )
    {
        _normalRangePercentage = normalRangePercent;
        
        if (!string.IsNullOrWhiteSpace(directoryPath) && !string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("You can only specify directory or file path");
        }
        
        if (string.IsNullOrWhiteSpace(directoryPath) && string.IsNullOrWhiteSpace(filePath))
        {
            directoryPath = Directory.GetCurrentDirectory();
        }

        var heading = $"Abnormal values {normalRangePercent}% above or below the median";
        PrintHeadingHelper.PrintHeading(heading);

        if (Directory.Exists(directoryPath))
        {
            DirectoryProcessHelper.ProcessFilesInDirectory(directoryPath, GetCsvProcessor);
        }
        else if (File.Exists(filePath))
        {
            GetCsvProcessor().ProcessFile(filePath);
        }
        else
        {
            Console.WriteLine("Wrong directory or file path");
        }
    }
    
    private ICsvFileProcessor GetCsvProcessor()
    {
        return new PrintAbnormalValues(_normalRangePercentage);
    }
}