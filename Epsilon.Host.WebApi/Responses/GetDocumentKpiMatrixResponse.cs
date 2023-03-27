using Epsilon.Host.WebApi.Models;

namespace Epsilon.Host.WebApi.Responses;

public record GetDocumentKpiMatrixResponse
{
    public int DocumentId { get; set; }
    public KpiMatrix KpiMatrix { get; set; }
}