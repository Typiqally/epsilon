using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Component;

namespace Epsilon.Component;

public class WordDocumentBuilder : IWordDocumentBuilder
{
    public async Task<Document> Build(IEnumerable<IEpsilonComponentFetcher> fetchers, IEnumerable<IEpsilonComponentConverter<OpenXmlElement>> converters)
    {
        var document = new Document(new Body());
        var convertersArray = converters.ToArray();

        foreach (var component in fetchers)
        {
            var componentData = await component.FetchObject();
            var converter = convertersArray.FirstOrDefault(c => c.Validate(componentData?.GetType()));

            if (componentData != null && converter != null)
            {
                var element = await converter.ConvertObject(componentData);
                document.Append(element);
            }
        }

        return document;
    }
}