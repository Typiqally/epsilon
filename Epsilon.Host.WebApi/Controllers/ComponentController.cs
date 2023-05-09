using Epsilon.Abstractions.Component;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly IComponentFetcher<CompetenceProfile> _competenceProfileManager;
    private readonly IComponentFetcher<KpiMatrixCollection> _kpiMatrixManager;

    public ComponentController(
        IConfiguration configuration,
        IComponentFetcher<CompetenceProfile> competenceProfileManager,
        IComponentFetcher<KpiMatrixCollection> kpiMatrixManager
    )
    {
        _competenceProfileManager = competenceProfileManager;
        _kpiMatrixManager = kpiMatrixManager;
    }

    [HttpGet("competence_profile")]
    public async Task<ActionResult<CompetenceProfile>> GetCompetenceProfile()
    {
        return await _competenceProfileManager.Fetch();
    }

    [HttpGet("kpi_matrix")]
    public async Task<ActionResult<KpiMatrixCollection>> GetKpiMatrix()
    {
        return await _kpiMatrixManager.Fetch();
    }
}