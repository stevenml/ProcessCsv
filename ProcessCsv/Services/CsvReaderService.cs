namespace ProcessCsv.Services;

using System.Globalization;
using CsvHelper;
using Dasync.Collections;

public class CsvReaderService<T> : ICsvReaderService<T> where T: class 
{
    public async Task<IReadOnlyCollection<T>> ReadCsvFileAsync(string filePath)
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