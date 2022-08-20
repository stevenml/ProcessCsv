namespace ProcessCsv.FileProcessors;

using Extensions;
using Helpers;
using Models;
using MathNet.Numerics.Statistics;

public class PrintAbnormalValues: CsvFileProcessor
{
    private readonly int _normalRangePercentage;
    public PrintAbnormalValues(int normalRangePercentage)
    {
        _normalRangePercentage = normalRangePercentage;
    }
    
    public override async Task ProcessFile(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var extractedRows = await ReadCsvByFileName(filePath, fileName);
        double medianValue = extractedRows.Select(x => x.Value).Median();
        
        var abnormalRows = extractedRows.FindAbnormalRows(medianValue, _normalRangePercentage);
        
        foreach (var row in abnormalRows)
        {
            Console.WriteLine($"{fileName} {row.Date} {row.Value} {medianValue}");
        }
        
        await Task.CompletedTask;
    }

    private static async Task<IReadOnlyCollection<ExtractedRowInfo>> ReadCsvByFileName(string filePath, string fileName)
    {
        if (fileName.StartsWith("comm_"))
        {
            return (await CsvReadHelper.ReadCsvFileAsync<CommFileModel>(filePath)).Select(x => new ExtractedRowInfo
            {
                Date = x.Date,
                Value = x.PriceSod
            }).ToList();
        }
        if (fileName.StartsWith("mod_"))
        {
            return (await CsvReadHelper.ReadCsvFileAsync<ModFileModel>(filePath)).Select(x => new ExtractedRowInfo
            {
                Date = x.Date,
                Value = x.ModDuration
            }).ToList();
        }

        return Enumerable.Empty<ExtractedRowInfo>().ToList();
    }
}