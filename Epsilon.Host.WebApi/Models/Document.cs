namespace Epsilon.Host.WebApi.Models;

public record CompetenceDocument
{
    public HomePage HomePage { get; set; }
    public KpiMatrix KpiMatrix { get; set; }
}