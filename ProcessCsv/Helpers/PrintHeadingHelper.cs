namespace ProcessCsv.Helpers;

public static class PrintHeadingHelper
{
    public static void PrintHeading(string heading)
    {
        Console.WriteLine($@"
#############################################################
  {heading}
#############################################################
        ");
    }
}