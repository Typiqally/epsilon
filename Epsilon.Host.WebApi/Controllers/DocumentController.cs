using DocumentFormat.OpenXml;
using Epsilon.Abstractions.Component;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : Controller
{
    private readonly IWordDownloader _wordDownloader;

    public DocumentController(
        IWordDownloader wordDownloader
    )
    {
        _wordDownloader = wordDownloader;
    }

    [HttpGet("word")]
    public async Task<IActionResult> DownloadWordDocument()
    {
        var fileStream = await _wordDownloader.Download();
        return File(fileStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "CompetenceDocument.docx");
    }
}