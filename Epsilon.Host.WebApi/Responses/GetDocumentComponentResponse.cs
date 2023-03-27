using Epsilon.Host.WebApi.Interfaces;

namespace Epsilon.Host.WebApi.Responses;

public record GetComponentResponse
{
    public IComponent Component { get; set; }
}