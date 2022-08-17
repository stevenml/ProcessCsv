namespace ProcessCsv.FileProcessors;

using Helpers;
using Models;
using MathNet.Numerics.Statistics;

public class PrintAbnormalValues: CsvFileProcessor
{
    private readonly int _normalRangePercentage;
    public PrintAbnormalValues(int normalRangePercentage)
    {
        this._normalRangePercentage = normalRangePercentage;
    }
    
    public override void ProcessFile(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var extractedRows = ReadCsvByFileName(filePath, fileName);
        double medianValue = extractedRows.Select(x => x.Value).Median();
        double upperRange = medianValue * (100 + _normalRangePercentage) / 100;
        double lowerRange = medianValue * (100 - _normalRangePercentage) / 100;
        
        foreach (var row in extractedRows)
        {
            if (row.Value > upperRange || row.Value < lowerRange)
            {
                var date = row.Date;
                Console.WriteLine($"{fileName} {date} {row.Value} {medianValue}");
            }
        }
    }

    private static IReadOnlyCollection<ExtractedRowInfo> ReadCsvByFileName(string filePath, string fileName)
    {
        if (fileName.StartsWith("comm_"))
        {
            return CsvReadHelper.ReadCsvFile<CommFileModel>(filePath).Select(x => new ExtractedRowInfo
            {
                Date = x.Date,
                Value = x.PriceSod
            }).ToList();
        }
        if (fileName.StartsWith("mod_"))
        {
            return CsvReadHelper.ReadCsvFile<ModFileModel>(filePath).Select(x => new ExtractedRowInfo
            {
                Date = x.Date,
                Value = x.ModDuration
            }).ToList();
        }

        return Enumerable.Empty<ExtractedRowInfo>().ToList();
    }
}