namespace ProcessCsv.Services;

public interface ICsvReaderService<T> where T: class
{
    public Task<IReadOnlyCollection<T>> ReadCsvFileAsync(string filePath);
}