using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

public interface IWordDocumentBuilder
{
    public Document Build(IEnumerable<ICompetenceWordComponent> components);
}