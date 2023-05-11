using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Component;

namespace Epsilon.Component;

public class WordDocumentBuilder : IWordDocumentBuilder
{
    public Document Build(IEnumerable<ICompetenceWordComponent> components)
    {
        var document = new Document(new Body());

        foreach (var component in components)
        {
            var element = component.ToWord();
            document.Append(element);
        }

        return document;
    }
}