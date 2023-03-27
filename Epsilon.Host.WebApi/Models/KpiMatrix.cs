using Epsilon.Host.WebApi.Interfaces;

namespace Epsilon.Host.WebApi.Models;

public record KpiMatrix : IComponent<KpiMatrixData>
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IDictionary<string, string> Config { get; set; }
    public KpiMatrixData Data { get; set; }
}

public record KpiMatrixData
{
    public string[] SomeKpiMatrixDataArray { get; set; }
}