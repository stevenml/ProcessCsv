namespace ProcessCsvTests;

using System;
using MathNet.Numerics.Statistics;
using System.Linq;
using FluentAssertions;
using ProcessCsv.Extensions;
using ProcessCsv.Models;
using Xunit;

public class AlgorithmTests
{
    [Fact]
    public void TestFindAbnormalRows()
    {
        var extractedRows = new ExtractedRowInfo[]
            { 
                new() { Date = "", Value = 1 },
                new() { Date = "", Value = 2 },
                new() { Date = "", Value = 333 },
                new() { Date = "", Value = 4 },
                new() { Date = "", Value = 5 },
                new() { Date = "", Value = 6 },
                new() { Date = "", Value = 7 },
                new() { Date = "", Value = 888 },
            };
        
        var medianValue = extractedRows.Select(x => x.Value).Median();
        var abNormalRows = extractedRows.FindAbnormalRows(medianValue, 20);
        abNormalRows.Select(x => x.Value).ToArray().Should().Equal(1, 2, 333, 4, 7, 888);
    }
}