using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("components")]
public class ComponentController : ControllerBase
{
    [HttpGet("competence_profile")]
    public ActionResult<CompetenceProfile> GetCompetenceProfile(DateTime startDate, DateTime endDate)
    {
        return new CompetenceProfile(
            HboIDomain.HboIDomain_2018,
            new[]
            {
                new CompetenceProfileKpi(
                    HboIDomain.HboIDomain_2018.ArchitecturalLayers.First(),
                    HboIDomain.HboIDomain_2018.Activities.First(),
                    1,
                    2,
                    DateTime.Now
                ),
                new CompetenceProfileKpi(
                    HboIDomain.HboIDomain_2018.ArchitecturalLayers.First(),
                    HboIDomain.HboIDomain_2018.Activities.First(),
                    1,
                    2,
                    DateTime.Now
                ),
                new CompetenceProfileKpi(
                    HboIDomain.HboIDomain_2018.ArchitecturalLayers.First(),
                    HboIDomain.HboIDomain_2018.Activities.First(),
                    1,
                    2,
                    DateTime.Now
                ),
                new CompetenceProfileKpi(
                    HboIDomain.HboIDomain_2018.ArchitecturalLayers.First(),
                    HboIDomain.HboIDomain_2018.Activities.First(),
                    1,
                    2,
                    DateTime.Now
                ),
            }
        );
    }
}