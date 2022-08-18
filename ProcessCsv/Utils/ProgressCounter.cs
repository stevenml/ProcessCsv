namespace ProcessCsv.Utils;

using System.Diagnostics;
using Microsoft.Extensions.Logging;

public class ProgressCounter
{
    private readonly int _progressBatchSize;
    private int _countTotalProcessed;
    private int _countTotalSuccessful;
    private int _countTotalFailed;
    private readonly object _counterLock = new object();
    private readonly ILogger? _logger;
    private readonly Stopwatch? _stopwatch;

    private int CountTotalToProcess { get; }
    private int CountProcessedInBatch => _countTotalProcessed % _progressBatchSize; 

    public ProgressCounter(int countTotalToProcess, ILogger? logger, Stopwatch? stopwatch, int progressBatchSize = 100)
    {
        CountTotalToProcess = countTotalToProcess;
        _progressBatchSize = progressBatchSize;
        _logger = logger;
        _stopwatch = stopwatch;
    }

    public void IncrementProcessed()
    {
        lock (_counterLock)
        {
            Interlocked.Increment(ref _countTotalProcessed);
            LogProgressInternal();
        }
    }

    public void IncrementSuccessful()
    {
        Interlocked.Increment(ref _countTotalSuccessful);
    }
    
    public void IncrementFailed()
    {
        Interlocked.Increment(ref _countTotalFailed);
    }

    private void LogProgressInternal()
    {
        if (_logger == null) 
            return;
        
        if (CountProcessedInBatch != 0)
            return;

        var message = $"{_countTotalProcessed} items processed.";
        message += _stopwatch?.IsRunning == true
            ? $", time elapsed {_stopwatch?.ElapsedMilliseconds} ms"
            : string.Empty;
        
        _logger.LogInformation(message);
    }
    
    public void LogProgressSummary()
    {
        if (_logger == null) 
            return;

        var message = $@"Total number of items to process: {CountTotalToProcess}
                   Processed: {_countTotalProcessed}
                   Successful: {_countTotalSuccessful}
                   Failed: {_countTotalFailed}
                   Unprocessed: {CountTotalToProcess - _countTotalProcessed}
                ";
        message += _stopwatch?.IsRunning == true
            ? $" Time elapsed: {_stopwatch?.ElapsedMilliseconds} ms"
            : string.Empty;
        
        _logger.LogInformation(message);
    }
}
