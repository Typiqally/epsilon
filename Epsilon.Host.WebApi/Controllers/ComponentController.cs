using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Service;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly ICompetenceComponentService _competenceComponentService;

    public ComponentController(ICompetenceComponentService competenceComponentService)
    {
        _competenceComponentService = competenceComponentService;
    }

    [HttpGet("{componentName}")]
    [Produces(typeof(CompetenceProfile))]
    public async Task<ActionResult<ICompetenceComponent>> GetCompetenceProfile(string componentName)
    {
        var component = await _competenceComponentService.GetComponent(componentName);
        if (component == null)
        {
            return NotFound();
        }

        return Ok(component);
    }
}