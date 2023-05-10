using Epsilon.Abstractions.Component;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly IEpsilonComponentFetcher<CompetenceProfile> _competenceProfileManager;

    public ComponentController(IConfiguration configuration, IEpsilonComponentFetcher<CompetenceProfile> competenceProfileManager)
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