namespace ProcessCsv.Models;

using CsvHelper.Configuration.Attributes;

public class CommFileModel
{
    public string Date { get; set; } = null!;

    [Name("Price SOD")]
    public double PriceSod { get; set; }
}