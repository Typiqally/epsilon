using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Abstractions.Component;

public interface IWordCompetenceComponent : ICompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart);
}