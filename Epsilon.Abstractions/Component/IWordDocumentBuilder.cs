using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

public interface IWordDocumentBuilder
{
    public Document Build(MainDocumentPart mainDocumentPart, IEnumerable<ICompetenceWordComponent> components);
}