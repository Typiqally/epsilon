using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

public interface IWordDocumentGenerator
{
    public Task<Document> Generate(IEnumerable<IEpsilonComponent> components, IEnumerable<IEpsilonComponentConverter<OpenXmlElement>> wordConverters);
}