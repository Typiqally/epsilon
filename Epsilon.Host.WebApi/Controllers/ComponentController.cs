using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.QueryResponse;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Service;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("component")]
public class ComponentController : ControllerBase
{
    private readonly IGraphQlHttpService _graphQlService;
    private readonly IConfiguration _configuration;
    private readonly ICompetenceProfileConverter _competenceProfileConverter;
    private readonly IAccountHttpService _accountHttpService;
    
    public ComponentController(IGraphQlHttpService graphQlService, IConfiguration configuration, ICompetenceProfileConverter competenceProfileConverter, IAccountHttpService accountHttpService)
    {
        _graphQlService = graphQlService;
        _configuration = configuration;
        _competenceProfileConverter = competenceProfileConverter;
        _accountHttpService = accountHttpService;
    }

    [HttpGet("competence_profile")]
    public ActionResult<CompetenceProfile> GetCompetenceProfile()
    {
        var studentId = _configuration["Canvas:StudentId"];
        var query = QueryConstants.GetAllUserCoursesSubmissionOutcomes.Replace("$studentIds", $"{studentId}");
        var queryResult = _graphQlService.Query<GetAllUserCoursesSubmissionOutcomes>(query).Result!;
        
        var terms = _accountHttpService.GetAllTerms(1).Result.Where(t => t.StartAt != null && t.EndAt != null);
        
        var competenceProfile = _competenceProfileConverter.ConvertFrom(queryResult, terms);
        return competenceProfile;
    }

    [HttpGet("competence_profile_mock")]
    public ActionResult<CompetenceProfile> GetMockCompetenceProfile([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var professionalTaskOutcomes = new List<ProfessionalTaskOutcome>();
        var professionalSkillOutcomes = new List<ProfessionalSkillOutcome>();
        var terms = new List<EnrollmentTerm>();

        for (var i = 0; i < 5; i++)
        {
            professionalTaskOutcomes.Add(GetRandomProfessionalTaskOutcome());
            professionalSkillOutcomes.Add(GetRandomProfessionalSkillOutcome());
        }

        return new CompetenceProfile(
            HboIDomain.HboIDomain2018,
            professionalTaskOutcomes,
            professionalSkillOutcomes,
            terms
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