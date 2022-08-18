namespace ProcessCsv.FileProcessors;

public abstract class CsvFileProcessor : ICsvFileProcessor
{
    public abstract Task ProcessFile(string filePath);
}