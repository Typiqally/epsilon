namespace Epsilon.Export;

public class ExportOptions
{
    public string OutputName { get; set; } = "Epsilon-Export-{DateTime}";

    public List<string> Formats { get; } = new();

    public string FormattedOutputName => OutputName
        .Replace("{DateTime}", DateTime.Now.ToString("ddMMyyyyHHmmss"));
}