using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Abstractions.Component;

public interface ICompetenceWordComponent : ICompetenceComponent
{
    public OpenXmlElement ToWord(MainDocumentPart mainDocumentPart);
}