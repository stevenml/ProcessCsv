namespace ProcessCsv.Helpers;

using System.Globalization;
using CsvHelper;

public static class CsvReadHelper
{
    public static IAsyncEnumerable<T> ReadCsvFileAsync<T>(string filePath) where T: class
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecordsAsync<T>();
    }
    
    public static IReadOnlyCollection<T> ReadCsvFile<T>(string filePath) where T: class
    {
        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<T>();
            return records.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}