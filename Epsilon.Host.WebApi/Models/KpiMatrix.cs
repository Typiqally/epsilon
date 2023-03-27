namespace Epsilon.Host.WebApi.Models;

public class KpiMatrix : IDocumentComponent<KpiMatrixData>
{
    public string Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IDictionary<string, string> Config { get; set; }
    public KpiMatrixData Data { get; set; }
}

public class KpiMatrixData
{
    public string[] Colors { get; set; }
}