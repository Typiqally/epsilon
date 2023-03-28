using Epsilon.Host.WebApi.Interfaces;

namespace Epsilon.Host.WebApi.Responses;

public record GetDocumentComponentResponse
{
    public IComponent Component { get; set; }
}