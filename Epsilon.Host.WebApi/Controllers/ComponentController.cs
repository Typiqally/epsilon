using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly IComponent<CompetenceProfile> _competenceProfileManager;

    public ComponentController(IConfiguration configuration, IComponent<CompetenceProfile> competenceProfileManager)
    {
        _competenceProfileManager = competenceProfileManager;
    }

    [HttpGet("competence_profile")]
    public async Task<ActionResult<CompetenceProfile>> GetCompetenceProfile()
    {
        var competenceProfile = await _competenceProfileManager.Fetch();

        return competenceProfile;
    }
}