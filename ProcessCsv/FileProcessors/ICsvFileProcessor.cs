namespace ProcessCsv.FileProcessors;

public interface ICsvFileProcessor
{
    void ProcessFile(string filePath);
}