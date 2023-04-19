using Epsilon.Abstractions.Component.Manager;
using Epsilon.Abstractions.Model;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ICompetenceProfileManager _competenceProfileManager;
    
    public ComponentController(IConfiguration configuration, ICompetenceProfileManager competenceProfileManager)
    {
        _configuration = configuration;
        _competenceProfileManager = competenceProfileManager;
    }

    [HttpGet("competence_profile")]
    public async Task<ActionResult<CompetenceProfile>> GetCompetenceProfile()
    {
        var studentId = _configuration["Canvas:StudentId"];
        var competenceProfile = await _competenceProfileManager.GetComponent(studentId);
        
        return competenceProfile;
    }
}