using DocumentFormat.OpenXml;

namespace Epsilon.Abstractions.Component;

public interface IComponentWordConverter<in TComponent> : IComponentConverter<OpenXmlElement, TComponent>
{
}