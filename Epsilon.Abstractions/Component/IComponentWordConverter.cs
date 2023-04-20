using DocumentFormat.OpenXml;

namespace Epsilon.Abstractions.Component;

public interface IComponentWordConverter<in TData>
{
    public Task<OpenXmlElement> Convert(TData data);
}