namespace ProcessCsvTests.FileProcessorsTests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using ProcessCsv.FileProcessors;
using ProcessCsv.Models;
using ProcessCsv.Services;
using Xunit;

public class PrintAbnormalValuesProcessorTests
{
    [Fact]
    public void TestFindAbnormalRows()
    {
        var commFileReaderMock = new Mock<ICsvReaderService<CommFileModel>>();
        commFileReaderMock.Setup(x =>
            x.ReadCsvFileAsync(It.IsAny<string>())).Returns(Task.FromResult<IReadOnlyCollection<CommFileModel>>(new[]
        {
            new CommFileModel { Date = "01/08/2022", PriceSod = 0.1 },
            new CommFileModel { Date = "01/08/2022", PriceSod = 17.56 },
            new CommFileModel { Date = "01/08/2022", PriceSod = 18.56 },
            new CommFileModel { Date = "01/08/2022", PriceSod = 18.56 },
            new CommFileModel { Date = "01/08/2022", PriceSod = 18.56 },
            new CommFileModel { Date = "01/08/2022", PriceSod = 18.56 },
            new CommFileModel { Date = "01/08/2022", PriceSod = 19.56 },
            new CommFileModel { Date = "01/08/2022", PriceSod = 18.56 },
            new CommFileModel { Date = "01/08/2022", PriceSod = -1.22 },
            new CommFileModel { Date = "01/08/2022", PriceSod = 100.89 },
        }.ToList()));
        
        var modFileReaderMock = new Mock<ICsvReaderService<ModFileModel>>();
        modFileReaderMock.Setup(x =>
            x.ReadCsvFileAsync(It.IsAny<string>())).Returns(Task.FromResult<IReadOnlyCollection<ModFileModel>>(new[]
        {
            new ModFileModel { Date = "01/08/2022", ModDuration = 0.1 },
            new ModFileModel { Date = "01/08/2022", ModDuration = 17.56 },
            new ModFileModel { Date = "01/08/2022", ModDuration = 18.56 },
            new ModFileModel { Date = "01/08/2022", ModDuration = 18.56 },
            new ModFileModel { Date = "01/08/2022", ModDuration = 18.56 },
            new ModFileModel { Date = "01/08/2022", ModDuration = 18.56 },
            new ModFileModel { Date = "01/08/2022", ModDuration = 19.56 },
            new ModFileModel { Date = "01/08/2022", ModDuration = 18.56 },
            new ModFileModel { Date = "01/08/2022", ModDuration = -1.22 },
            new ModFileModel { Date = "01/08/2022", ModDuration = 100.89 },
        }.ToList()));

        // Use mocked IPrintService to collect readable data to compare with the expected data
        var printToStringListService = new PrintToStringListService();

        var mock = AutoMock.GetLoose(builder =>
        {
            builder.RegisterInstance(commFileReaderMock.Object).As<ICsvReaderService<CommFileModel>>();
            builder.RegisterInstance(modFileReaderMock.Object).As<ICsvReaderService<ModFileModel>>();
            builder.RegisterInstance(printToStringListService).As<IPrintService>();
        });


        var printAbnormalValuesProcessorForCommFileMock =
            mock.Create<PrintAbnormalValuesProcessor>(new NamedParameter("normalRangePercentage", 30));

        printAbnormalValuesProcessorForCommFileMock.ProcessFile("comm_01.csv");

        var printedRows = printToStringListService.PrintedStringList;
        var expectedRowsToPrint = new List<string>
        {
            "comm_01.csv 01/08/2022 0.1 18.56",
            "comm_01.csv 01/08/2022 -1.22 18.56",
            "comm_01.csv 01/08/2022 100.89 18.56"
        };
        printedRows.Should().Equal(expectedRowsToPrint);
    }
}