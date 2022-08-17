namespace ProcessCsv.FileProcessors;

public abstract class CsvFileProcessor : ICsvFileProcessor
{
    public abstract void ProcessFile(string filePath);
}