namespace ProcessCsv.Models;

public class AbnormalValueRow
{
    public string Date { get; set; } = null!;
    public double MedianValue { get; set; }
    public double Value { get; set; }
}