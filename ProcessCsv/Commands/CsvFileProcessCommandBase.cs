namespace ProcessCsv.Commands;

using Autofac;
using FileProcessors;
using Utils;

public abstract class CsvFileProcessCommandBase
{
    protected readonly IComponentContext IcoContext;
    
    protected CsvFileProcessCommandBase(IComponentContext icoContext)
    {
        IcoContext = icoContext;
    }

    protected async Task ProcessFolderOrFileSequential(string directoryPath, string filePath)
    {
        if (!string.IsNullOrWhiteSpace(directoryPath) && !string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("You can only specify directory or file path");
        }
        
        if (string.IsNullOrWhiteSpace(directoryPath) && string.IsNullOrWhiteSpace(filePath))
        {
            directoryPath = Directory.GetCurrentDirectory();
        }

        if (Directory.Exists(directoryPath))
        {
            string [] fileEntries = Directory.GetFiles(directoryPath);
            foreach (var file in fileEntries)
            {
                await CsvProcessor.ProcessFile(file);
            }
        }
        else if (File.Exists(filePath))
        {
            await CsvProcessor.ProcessFile(filePath);
        }
        else
        {
            Console.WriteLine("Wrong directory or file path");
        }
    }
    
    protected async Task ProcessFolderOrFileInParallel(string directoryPath, string filePath)
    {
        if (!string.IsNullOrWhiteSpace(directoryPath) && !string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("You can only specify directory or file path");
        }
        
        if (string.IsNullOrWhiteSpace(directoryPath) && string.IsNullOrWhiteSpace(filePath))
        {
            directoryPath = Directory.GetCurrentDirectory();
        }

        if (Directory.Exists(directoryPath))
        {
            var fileEntries = Directory.GetFiles(directoryPath).ToList();
            await ParallelProcessor<string, object>.Create(
                fileEntries,
                null,
                BatchProcessor,
                null,
                null,
                batchSize: 1, // Here we only support 1 per batch as example
                degreeOfParallelism: 3 // just 3 as example
            ).Start();
        }
        else if (File.Exists(filePath))
        {
            await CsvProcessor.ProcessFile(filePath);
        }
        else
        {
            Console.WriteLine("Wrong directory or file path");
        }
    }

    protected abstract ICsvFileProcessor CsvProcessor { get; }

    private Func<IEnumerable<string>, object?, Task> BatchProcessor => (filePaths, _) =>
    {
        // only support 1 per batch as example
        var fileName = filePaths.First();
        return CsvProcessor.ProcessFile(fileName);
    };
}