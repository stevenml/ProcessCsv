﻿namespace ProcessCsv.Models;

using CsvHelper.Configuration.Attributes;

public class ModFileModel
{
    public string Date { get; set; }

    [Name("MOD Duration")]
    public double ModDuration { get; set; }
}