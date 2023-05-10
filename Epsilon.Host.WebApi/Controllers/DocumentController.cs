using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Epsilon.Abstractions.Component;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : Controller
{
    private readonly IWordDocumentBuilder _wordDocumentBuilder;
    private readonly IEnumerable<IComponentFetcher> _componentFetchers;

    public DocumentController(
        IWordDocumentBuilder wordDocumentBuilder,
        IEnumerable<IComponentFetcher> componentFetchers
    )
    {
        _wordDocumentBuilder = wordDocumentBuilder;
        _componentFetchers = componentFetchers;
    }

    [HttpGet("word")]
    public async Task<IActionResult> DownloadWordDocument()
    {
        var stream = new MemoryStream();

        var components = new List<IEpsilonWordComponent>();
        foreach (var componentFetcher in _componentFetchers)
        {
           var component = await componentFetcher.FetchUnknown();
           if (component is IEpsilonWordComponent wordComponent)
           {
               components.Add(wordComponent);
           }
        }
        
        using var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = _wordDocumentBuilder.Build(components);

        document.Save();
        document.Close();

        stream.Position = 0;

        return File(
            stream,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "CompetenceDocument.docx"
        );
    }
}