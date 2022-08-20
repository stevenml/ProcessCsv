namespace ProcessCsv.FileProcessors;

using Extensions;
using Models;
using MathNet.Numerics.Statistics;
using Services;

public class PrintAbnormalValuesProcessor: CsvFileProcessor
{
    private readonly int _normalRangePercentage;
    private readonly ICsvReaderService<CommFileModel> _commFileReaderService;
    private readonly ICsvReaderService<ModFileModel> _modFileReaderService;
    
    public PrintAbnormalValuesProcessor(int normalRangePercentage, 
        ICsvReaderService<CommFileModel> commFileReaderService,
        ICsvReaderService<ModFileModel> modFileReaderService)
    {
        _normalRangePercentage = normalRangePercentage;
        _commFileReaderService = commFileReaderService;
        _modFileReaderService = modFileReaderService;
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

    private async Task<IReadOnlyCollection<ExtractedRowInfo>> ReadCsvByFileName(string filePath, string fileName)
    {
        if (fileName.StartsWith("comm_"))
        {
            return (await _commFileReaderService.ReadCsvFileAsync(filePath)).Select(x => new ExtractedRowInfo
            {
                Date = x.Date,
                Value = x.PriceSod
            }).ToList();
        }
        if (fileName.StartsWith("mod_"))
        {
            return (await _modFileReaderService.ReadCsvFileAsync(filePath)).Select(x => new ExtractedRowInfo
            {
                Date = x.Date,
                Value = x.ModDuration
            }).ToList();
        }

        return Enumerable.Empty<ExtractedRowInfo>().ToList();
    }
}