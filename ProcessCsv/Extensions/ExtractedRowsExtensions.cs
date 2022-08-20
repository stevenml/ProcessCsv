namespace ProcessCsv.Extensions;

using Models;

public static class ExtractedRowsExtensions
{
    public static IReadOnlyCollection<ExtractedRowInfo> FindAbnormalRows(
        this IEnumerable<ExtractedRowInfo> extractedRows, double medianValue, int normalRangePercentage)
    {
        double upperRange = medianValue * (100 + normalRangePercentage) / 100;
        double lowerRange = medianValue * (100 - normalRangePercentage) / 100;
        
        var abnormalRows = new List<ExtractedRowInfo>();
        foreach (var row in extractedRows)
        {
            if (row.Value > upperRange || row.Value < lowerRange)
            {
                abnormalRows.Add(row);
            }
        }

        return abnormalRows;
    }
}