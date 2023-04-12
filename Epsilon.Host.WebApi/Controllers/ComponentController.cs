using Epsilon.Abstractions.Model;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.QueryResponse;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("component")]
public class ComponentController : ControllerBase
{
    private readonly IGraphQlHttpService _graphQlService;
    private readonly IConfiguration _configuration;

    public ComponentController(IGraphQlHttpService graphQlService, IConfiguration configuration)
    {
        _graphQlService = graphQlService;
        _configuration = configuration;
    }

    [HttpGet("competence_profile")]
    public ActionResult<GetUserSubmissionOutcomes> GetCompetenceProfile([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var courseId = _configuration["Canvas:CourseId"];
        var query = QueryConstants.GetUserSubmissionOutcomes.Replace("$courseId", courseId);

        return _graphQlService.Query<GetUserSubmissionOutcomes>(query).Result!;
    }

    [HttpGet("competence_profile_mock")]
    public ActionResult<CompetenceProfile> GetMockCompetenceProfile([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var competenceProfileOutcomes = new List<CompetenceProfileOutcome>();
        for (var i = 0; i < 5; i++)
        {
            competenceProfileOutcomes.Add(GetRandomCompetenceProfileOutcome());
        }

        return new CompetenceProfile(
            HboIDomain.HboIDomain2018,
            competenceProfileOutcomes
        );
    }

    private static CompetenceProfileOutcome GetRandomCompetenceProfileOutcome()
    {
        return new CompetenceProfileOutcome(
            GetRandom(HboIDomain.HboIDomain2018.ArchitectureLayers).Name,
            GetRandom(HboIDomain.HboIDomain2018.Activities).Name,
            GetRandom(HboIDomain.HboIDomain2018.MasteryLevels).Level,
            GetRandom(new[] { 0, 3, 4, 5 }),
            DateTime.Now
        );
    }

    private static T GetRandom<T>(IEnumerable<T> items)
    {
        var random = new Random();
        var itemsArray = items.ToArray();
        var index = random.Next(0, itemsArray.Length);
        return itemsArray.ElementAt(index);
    }
}