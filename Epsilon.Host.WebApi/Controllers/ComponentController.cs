using System.Globalization;
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

    //
    // DateTime.ParseExact(startDate ?? "", "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US")),
    // DateTime.Parse(endDate ?? "", CultureInfo.CreateSpecificCulture("en-US"))
    
    [HttpGet("{componentName}")]
    [Produces(typeof(CompetenceProfile))]
    public async Task<ActionResult<ICompetenceComponent>> GetCompetenceProfile(string componentName, DateTime? startDate, DateTime? endDate)
    {
        {
            var component = await _competenceComponentService.GetComponent(componentName, startDate, endDate);
            if (component == null)
            {
                return NotFound();
            }

            return Ok(component);
        }

        return NotFound();
    }
}