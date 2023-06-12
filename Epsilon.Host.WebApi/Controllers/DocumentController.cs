using Epsilon.Abstractions.Service;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : Controller
{
    private readonly ICompetenceDocumentService _competenceDocumentService;

    public DocumentController(ICompetenceDocumentService competenceDocumentService)
    {
        _competenceDocumentService = competenceDocumentService;
    }

    [HttpGet("word")]
    public async Task<IActionResult> DownloadWordDocument(string name, DateTime startDate, DateTime endDate)
    {
        var stream = new MemoryStream();
        await _competenceDocumentService.WriteDocument(stream, name, startDate, endDate);

        return File(
            stream,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "CompetenceDocument.docx"
        );
    }
}