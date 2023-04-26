using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Component;

namespace Epsilon.Component;

public class WordDocumentGenerator : IWordDocumentGenerator
{
    public async Task<Document> Generate(IEnumerable<IComponent> components, IEnumerable<IComponentConverter<OpenXmlElement>> wordConverters)
    {
        var document = new Document(new Body());

        foreach (var component in components)
        {
            var componentData = await component.FetchObject();
            var converter = wordConverters.FirstOrDefault(c => c.Validate(componentData?.GetType()));

            if (componentData != null && converter != null)
            {
                var element = await converter.ConvertObject(componentData);
                document.Append(element);
            }
        }

        return document;
    }
}