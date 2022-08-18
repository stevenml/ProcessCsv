namespace ProcessCsvTests;

using Xunit;
using MathNet.Numerics.Statistics;
using System.Linq;
using FluentAssertions;

public class MathNetTest
{
    [Theory]
    [InlineData(new double[] { 9, 10, 12, 13, 13, 13, 15, 15, 16, 16, 18, 22, 23, 24, 24, 25 }, 15.5)]
    [InlineData(new double[] { 0, 23, 24, 24, 25, 26, 26, 27, 100 }, 25)]
    [InlineData(new double[] { 34, 56, 34, 4, 23, 45, 678, 34, 54, 45, 23, 32, 43, 54, 65, 76 }, 44)]
    [InlineData(new [] { 4.9, 23.7, 23.9, 32.8, 34.6, 34.7, 34.8, 43, 45.9, 45.9, 56.7, 678.7 }, 34.75)]
    [InlineData(new double[] { -1, 0, 1 }, 0)]
    public void TestMedian(double[] list, double expectedMedian)
    {
        // We don't usually direct test third party functions, especially this popular one
        // but it's good to also test some of them in case they got broken by a breaking release or misusing
        var calculatedMedian = list.Select(x => x).Median();
        calculatedMedian.Should().Be(expectedMedian);
    }
}