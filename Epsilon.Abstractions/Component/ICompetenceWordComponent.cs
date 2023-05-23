using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Abstractions.Component;

public interface ICompetenceWordComponent : ICompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart);
}