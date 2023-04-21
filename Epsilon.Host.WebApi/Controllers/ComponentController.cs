using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly IComponentFetcher<CompetenceProfile> _competenceProfileManager;

    public ComponentController(IConfiguration configuration, IComponentFetcher<CompetenceProfile> competenceProfileManager)
    {
        _competenceProfileManager = competenceProfileManager;
        _kpiMatrixManager = kpiMatrixManager;
    }

    [HttpGet("competence_profile")]
    public async Task<ActionResult<CompetenceProfile>> GetCompetenceProfile()
    {
        var competenceProfile = await _competenceProfileManager.Fetch();

        return competenceProfile;
    }
    
    [HttpGet("kpi_matrix")]
    public async Task<ActionResult<KpiMatrix>> GetKpiMatrix()
    {
        var kpiMatrix = await _kpiMatrixManager.Fetch();
        
        return kpiMatrix;
    }
}