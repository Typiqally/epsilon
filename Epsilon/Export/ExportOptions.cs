namespace Epsilon.Export;

public class ExportOptions
{
    public string OutputName { get; set; } = $"{Constants.ProjectName}-Export-{{DateTime}}";

    public string Formats { get; set; } = "console";
    
    public string Modules { get; set; } = "";

    public string FormattedOutputName => OutputName
        .Replace("{DateTime}", DateTime.Now.ToString("ddMMyyyyHHmmss"));
}