using Epsilon.Host.WebApi.Interfaces;
using Epsilon.Host.WebApi.Models;
using Epsilon.Host.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("api/component")]
public class ComponentController : Controller
{
    private readonly List<IComponent> _components = new()
    {
        new KpiMatrix
        {
            Id = 1,
            Type = "kpi-matrix",
            Title = "KPI Matrix",
            Description = "A matrix of KPIs",
            Config = new Dictionary<string, string>
            {
                { "KpiConfigRecord", "Sven" },
            },
            Data = new KpiMatrixData
            {
                SomeKpiMatrixDataArray = new[] { "red", "green", "blue" }
            }
        },
        new HomePage
        {
            Id = 2,
            Type = "homepage",
            Title = "Homepage",
            Description = "The homepage of the website",
            Config = new Dictionary<string, string>
            {
                { "HomePageRecord", "Jelle" },
            },
            Data = new HomePageData
            {
                SomeHomePageDataArray = new[] { "red", "green", "blue" }
            }
        }
    };
    
    [HttpGet("random")]
    public IActionResult GetRandomComponent()
    {
        return Ok(new GetDocumentComponentResponse
        {
            Component = _components[new Random().Next(0, _components.Count)]
        });
    }
}