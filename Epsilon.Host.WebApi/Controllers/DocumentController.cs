using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Component;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : Controller
{
    private readonly IEnumerable<IComponent> _components;
    private readonly IEnumerable<IComponentConverter<OpenXmlElement>> _wordConverters;

    public DocumentController(
        IEnumerable<IComponent> components,
        IEnumerable<IComponentConverter<OpenXmlElement>> wordConverters
    )
    {
        _components = components;
        _wordConverters = wordConverters;
    }

    [HttpGet("word")]
    public async Task<IActionResult> DownloadWordDocument()
    {
        // TODO: Move this logic to a separate class, don't put it in the controller like this
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

        return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "CompetenceDocument.docx");
    }
}