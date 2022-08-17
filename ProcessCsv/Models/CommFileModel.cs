namespace ProcessCsv.Models;

using CsvHelper.Configuration.Attributes;

public class CommFileModel
{
    public DateTime Date { get; set; }

    [Name("Price SOD")]
    public decimal PriceSod { get; set; }
}