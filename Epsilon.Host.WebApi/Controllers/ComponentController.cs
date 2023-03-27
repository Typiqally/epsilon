using Epsilon.Host.WebApi.Models;
using Epsilon.Host.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("api/component")]
public class ComponentController : Controller
{
    private static readonly KpiMatrix _kpiMatrix = new KpiMatrix
    {
        Id = 1,
        Type = "KpiMatrix",
        Title = "KPI Matrix",
        Description = "KPI Matrix",
        Config = new KpiMatrixConfig
        {
            columns = new[] { "Column 1", "Column 2", "Column 3" },
            rows = new[] { "Row 1", "Row 2", "Row 3" }
        },
        Data = new KpiMatrixData
        {
            colors = new[] { "Red", "Green", "Blue" }
        }
    };
    
    [HttpGet("{componentId:int}")]
    public IActionResult GetDocumentComponent(int componentId)
    {
        return Ok(new GetComponentResponse
        {
            
        });
    }
}