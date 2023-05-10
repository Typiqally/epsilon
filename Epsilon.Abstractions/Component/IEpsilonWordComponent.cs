using DocumentFormat.OpenXml;

namespace Epsilon.Abstractions.Component;

public interface IEpsilonWordComponent : IEpsilonComponent
{
    public OpenXmlElement ToWord();
}