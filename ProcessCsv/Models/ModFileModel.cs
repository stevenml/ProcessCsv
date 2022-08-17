namespace ProcessCsv.Models;

using CsvHelper.Configuration.Attributes;

public class ModFileModel
{
    public DateTime Date { get; set; }

    [Name("MOD Duration")]
    public decimal ModDuration { get; set; }
}