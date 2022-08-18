namespace ProcessCsv.Utils;

using System.Diagnostics;
using Dasync.Collections;
using Extensions;
using Microsoft.Extensions.Logging;

public class ParallelProcessor<TItem, TContext>
{
    private readonly IEnumerable<TItem> _collection;
    private readonly Func<IEnumerable<TItem>, TContext?, Task> _processAction;
    private readonly ProgressCounter _progressCounter;
    private readonly int _batchSize;
    private readonly int _degreeOfParallelism;
    private readonly ILogger? _logger;
    private readonly Stopwatch? _stopwatch;
    private readonly TContext? _context;

    public ParallelProcessor(
        IReadOnlyCollection<TItem> collection,
        TContext? context,
        Func<IEnumerable<TItem>, TContext?, Task> processAction,
        ILogger? logger,
        Stopwatch? stopwatch,
        int batchSize = 1,
        int degreeOfParallelism = 8)
    {
        _collection = collection;
        _stopwatch = stopwatch ?? new Stopwatch();
        _progressCounter = new ProgressCounter(collection.Count(), logger, stopwatch);
        _processAction = processAction;
        _context = context;
        _batchSize = batchSize;
        _degreeOfParallelism = degreeOfParallelism;
        _logger = logger;
    }

    public static ParallelProcessor<TItem, TContext> Create(IReadOnlyCollection<TItem> collection,
        TContext? context,
        Func<IEnumerable<TItem>, TContext?, Task> processAction,
        ILogger? logger,
        Stopwatch? stopwatch,
        int batchSize = 1,
        int degreeOfParallelism = 8)
    {
        return new ParallelProcessor<TItem, TContext>(
            collection,
            context,
            processAction,
            logger,
            stopwatch,
            batchSize,
            degreeOfParallelism);
    }

    public async Task Start()
    {
        if (_stopwatch != null && _stopwatch.IsRunning != true)
            _stopwatch.Start();
        
        try
        {
            await _collection.BatchBy(_batchSize).ParallelForEachAsync(async items =>
            {
                _progressCounter.IncrementProcessed();
                await _processAction(items, _context);
                _progressCounter.IncrementSuccessful();
            }, _degreeOfParallelism);
        }
        catch (Exception e)
        {
            _progressCounter.IncrementFailed();
            _logger?.LogError(e.ToString());
            throw;
        }
        finally
        {
            _progressCounter.LogProgressSummary();
        }
    }
}
