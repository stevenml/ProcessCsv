namespace ProcessCsv.Helpers;

using System.Globalization;
using CsvHelper;
using Dasync.Collections;

public static class CsvReadHelper
{
    public static async Task<IReadOnlyCollection<T>> ReadCsvFileAsync<T>(string filePath) where T: class
    {
        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecordsAsync<T>();
            return await records.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}