namespace ProcessCsv.Commands;

using Cocona;
using Helpers;

public class PrintAbnormalValuesCommand
{
    public PrintAbnormalValuesCommand ()
    {
    }

    [Command(
        "print-abnormal-values",
        Description = "Print abnormal values n% above or below the median")]
    public async Task PrintAbnormalValues(
        [Option(
            "normal-range-percentage",
            new[] { 'p' },
            Description = "Please specify the normal value range percentage")]
        int normalRangePercentage)
    {
        var heading = $"Abnormal values {normalRangePercentage}% above or below the median";
        PrintHeadingHelper.PrintHeading(heading);
        Console.WriteLine("normalRangePercentage");
        await Task.CompletedTask;
    }
}