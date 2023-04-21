using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Component;

namespace Epsilon.Component;

public class WordDownloader : IWordDownloader
{
    private readonly IEnumerable<IComponent> _components;
    private readonly IEnumerable<IComponentConverter<OpenXmlElement>> _wordConverters;
    
    public WordDownloader(
        IEnumerable<IComponent> components,
        IEnumerable<IComponentConverter<OpenXmlElement>> wordConverters
    )
    {
        _components = components;
        _wordConverters = wordConverters;
    }
    
    public async Task<Stream> Download()
    {
        var stream = new MemoryStream();
        using var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = new Document(new Body());

        var documentPart = document.MainDocumentPart.Document;

        foreach (var component in _components)
        {
            var componentData = await component.FetchObject();
            var converter = _wordConverters.FirstOrDefault(c => c.Validate(componentData?.GetType()));

            if (componentData != null && converter != null)
            {
                var element = await converter.ConvertObject(componentData);
                documentPart.Append(element);
            }
        }

        document.Save();
        document.Close();

        stream.Position = 0;

        return stream;
    }
}