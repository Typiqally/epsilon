using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

public interface IWordDocumentBuilder
{
    public Task<Document> Build(IEnumerable<IEpsilonComponentFetcher> fetchers, IEnumerable<IEpsilonComponentConverter<OpenXmlElement>> converters);
}