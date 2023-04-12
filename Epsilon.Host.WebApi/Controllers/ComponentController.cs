using Epsilon.Abstractions.Component;
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
    private readonly ICompetenceProfileConverter _competenceProfileConverter;
    
    public ComponentController(IGraphQlHttpService graphQlService, IConfiguration configuration, ICompetenceProfileConverter competenceProfileConverter)
    {
        _graphQlService = graphQlService;
        _configuration = configuration;
        _competenceProfileConverter = competenceProfileConverter;
    }

    [HttpGet("competence_profile")]
    public ActionResult<CompetenceProfile> GetCompetenceProfile([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var studentId = _configuration["Canvas:StudentId"];
        var query = QueryConstants.GetAllUserCoursesSubmissionOutcomes.Replace("$studentIds", $"{studentId}");
        var queryResult = _graphQlService.Query<GetAllUserCoursesSubmissionOutcomes>(query).Result!;
        
        var competenceProfile = _competenceProfileConverter.ConvertFrom(queryResult);
        return competenceProfile;
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
            GetRandom(HboIDomain.HboIDomain2018.ArchitectureLayers).Value,
            GetRandom(HboIDomain.HboIDomain2018.Activities).Value,
            GetRandom(HboIDomain.HboIDomain2018.MasteryLevels).Value,
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