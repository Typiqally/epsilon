using Epsilon.Host.WebApi.Models;

namespace Epsilon.Host.WebApi.Responses;

public class GetDocumentStructureResponse
{
    public int DocumentId { get; set; }
    public IEnumerable<int> ComponentIds { get; set; }
}