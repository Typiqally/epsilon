using Epsilon.Host.WebApi.Models;

namespace Epsilon.Host.WebApi.Responses;

public record GetDocumentHomePageResponse
{
    public int DocumentId { get; set; }
    public HomePage HomePage { get; set; }
}