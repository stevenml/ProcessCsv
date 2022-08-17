namespace ProcessCsv.Helpers;

using FileProcessors;

public static class DirectoryProcessHelper
{
    public static void ProcessFilesInDirectory(string targetDirectory, Func<ICsvFileProcessor> getCsvProcessor)
    {
        // Process the list of files found in the directory, skip the directories
        string [] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
        {
            if (!File.Exists(fileName))
                continue;
            getCsvProcessor().ProcessFile(fileName);
        }
    } 
}