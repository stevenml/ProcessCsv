namespace ProcessCsv.FileProcessors;

public interface ICsvFileProcessor
{
    Task ProcessFile(string filePath);
}