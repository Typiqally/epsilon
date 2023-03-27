using Epsilon.Host.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("api/document/{documentId:int}")]
public class DocumentController : Controller
{
    [HttpGet("structure")]
    public IActionResult GetDocumentStructure(int documentId)
    {
        return Ok(new GetDocumentStructureResponse
        {
            DocumentId = documentId,
            ComponentIds = new[] { 1, 2, 3 }
        });
    }
}