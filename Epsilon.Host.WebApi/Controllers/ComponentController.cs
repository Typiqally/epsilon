using Epsilon.Abstractions.Model;
using Epsilon.Canvas;
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
        var professionalTaskOutcomes = new List<ProfessionalTaskOutcome>();
        var professionalSkillOutcomes = new List<ProfessionalSkillOutcome>();

        for (var i = 0; i < 5; i++)
        {
            professionalTaskOutcomes.Add(GetRandomProfessionalTaskOutcome());
            professionalSkillOutcomes.Add(GetRandomProfessionalSkillOutcome());
        }

        return new CompetenceProfile(
            HboIDomain.HboIDomain2018,
            professionalTaskOutcomes,
            professionalSkillOutcomes
        );
    }

    private static ProfessionalTaskOutcome GetRandomProfessionalTaskOutcome()
    {
        return new ProfessionalTaskOutcome(
            GetRandom(HboIDomain.HboIDomain2018.ArchitectureLayers.Keys),
            GetRandom(HboIDomain.HboIDomain2018.Activities.Keys),
            GetRandom(HboIDomain.HboIDomain2018.MasteryLevels.Keys),
            GetRandom(new[] { 0, 3, 4, 5 }),
            DateTime.Now
        );
    }

    private static ProfessionalSkillOutcome GetRandomProfessionalSkillOutcome()
    {
        return new ProfessionalSkillOutcome(
            GetRandom(HboIDomain.HboIDomain2018.ProfessionalSkills.Keys),
            GetRandom(HboIDomain.HboIDomain2018.MasteryLevels.Keys),
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