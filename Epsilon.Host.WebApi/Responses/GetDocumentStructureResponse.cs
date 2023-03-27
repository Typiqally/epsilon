namespace Epsilon.Host.WebApi.Responses;

public record GetDocumentStructureResponse
{
    public int DocumentId { get; set; }
    public IEnumerable<int> ComponentIds { get; set; }
}