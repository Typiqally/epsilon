using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

public interface IWordDocumentGenerator
{
    public Task<Document> Generate(IEnumerable<IComponent> components, IEnumerable<IEpsilonComponentConverter<OpenXmlElement>> wordConverters);
}