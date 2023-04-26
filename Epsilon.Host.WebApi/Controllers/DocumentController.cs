using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Epsilon.Abstractions.Component;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : Controller
{
    private readonly IWordDocumentGenerator _wordDocumentGenerator;
    private readonly IEnumerable<IEpsilonComponent> _components;
    private readonly IEnumerable<IEpsilonComponentConverter<OpenXmlElement>> _wordConverters;

    public DocumentController(
        IWordDocumentGenerator wordDocumentGenerator,
        IEnumerable<IEpsilonComponent> components,
        IEnumerable<IEpsilonComponentConverter<OpenXmlElement>> wordConverters
    )
    {
        _wordDocumentGenerator = wordDocumentGenerator;
        _components = components;
        _wordConverters = wordConverters;
    }

    [HttpGet("word")]
    public async Task<IActionResult> DownloadWordDocument()
    {
        var stream = new MemoryStream();
        using var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);

        document.AddMainDocumentPart();
        document.MainDocumentPart!.Document = await _wordDocumentGenerator.Generate(_components, _wordConverters);

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