using DocumentFormat.OpenXml;

namespace Epsilon.Abstractions.Component;

public interface ICompetenceWordComponent : ICompetenceComponent
{
    public OpenXmlElement ToWord();
}