using DocumentFormat.OpenXml;

namespace Epsilon.Abstractions.Component;

public interface IEpsilonComponentWordConverter<in TComponent> : IEpsilonComponentConverter<OpenXmlElement, TComponent>
{
}