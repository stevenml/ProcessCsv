namespace ProcessCsv.Commands;

using Autofac;
using Cocona;
using FileProcessors;
using Helpers;

public class PrintAbnormalValuesCommand: CsvFileProcessCommandBase
{
    private int _normalRangePercentage;
    
    public PrintAbnormalValuesCommand(IComponentContext icoContext) :
        base(icoContext)
    {
    }

    [Command("print-abnormal-values", Description = "Print abnormal values n% above or below the median")]
    public async Task PrintAbnormalValues(
        [Option("normal-range-percentage", new[] { 'p' }, Description = "Please specify the normal value range percentage")]
        int normalRangePercent,
        [Option("directory", new[] { 'd' }, Description = "Please specify the file directory path, or by default process all the files in current directory")]
        string directoryPath = "",
        [Option("file", new[] { 'f' }, Description = "Please specify the file path, or by default process all the files in current directory")]
        string filePath = "",
        [Option("parallel", new[] { 'l' }, Description = "Please specify if you want to process files in parallel")]
        bool isParallelProcess = false
    )
    {
        _normalRangePercentage = normalRangePercent;

        var heading = $"Abnormal values {normalRangePercent}% above or below the median";
        PrintHeadingHelper.PrintHeading(heading);
        if (isParallelProcess)
        {
            await ProcessFolderOrFileInParallel(directoryPath, filePath);
        }
        else
        {
            await ProcessFolderOrFileSequential(directoryPath, filePath);    
        }
    }

    protected override ICsvFileProcessor CsvProcessor => IcoContext.Resolve<PrintAbnormalValuesProcessor>(
        new NamedParameter("normalRangePercentage", _normalRangePercentage));
}