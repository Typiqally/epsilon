using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Converter;

public interface ILinkHeaderConverter : IConverter<LinkHeader, string>
{
    public LinkHeader ConvertFrom(HttpResponseMessage response);
}