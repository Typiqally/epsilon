using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Service;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly ICompetenceComponentService _competenceComponentService;
    private readonly IPageHttpService _pageHttpService;

    public ComponentController(ICompetenceComponentService competenceComponentService, IPageHttpService pageHttpService)
    {
        _competenceComponentService = competenceComponentService;
        _pageHttpService = pageHttpService;
    }

    [HttpGet("{componentName}")]
    [Produces(typeof(CompetenceProfile))]
    public async Task<ActionResult<ICompetenceComponent>> GetCompetenceProfile(string componentName, DateTime startDate, DateTime endDate)
    {
        var component = await _competenceComponentService.GetComponent(componentName, startDate, endDate);
        if (component == null)
        {
            return NotFound();
        }

        return Ok(component);
    }
    
    [HttpPost("{componentName}")]
    // [Produces(typeof(CompetenceProfile))]
    public async Task<ActionResult<ICompetenceComponent>> UpdateCompetenceComponent(string componentName, int courseId, string pageContent)
    {
        var page = await _pageHttpService.UpdateOrCreatePage(courseId, componentName, pageContent);

        if (page == null)
        {
            return NotFound();
        }
        
        return Ok(page);
    }
}