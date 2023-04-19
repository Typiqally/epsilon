using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.QueryResponse;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    private readonly IGraphQlHttpService _graphQlService;
    private readonly IConfiguration _configuration;
    private readonly ICompetenceProfileConverter _competenceProfileConverter;
    private readonly IAccountHttpService _accountHttpService;

    public ComponentController(
        IGraphQlHttpService graphQlService, 
        IConfiguration configuration, 
        ICompetenceProfileConverter competenceProfileConverter, 
        IAccountHttpService accountHttpService
    )
    {
        _graphQlService = graphQlService;
        _configuration = configuration;
        _competenceProfileConverter = competenceProfileConverter;
        _accountHttpService = accountHttpService;
    }

    [HttpGet("competence_profile")]
    public async Task<ActionResult<CompetenceProfile>> GetCompetenceProfile()
    {
        var studentId = _configuration["Canvas:StudentId"];
        var query = QueryConstants.GetAllUserCoursesSubmissionOutcomes.Replace("$studentIds", $"{studentId}");
        var queryResult = await _graphQlService.Query<GetAllUserCoursesSubmissionOutcomes>(query);

        var terms = await _accountHttpService.GetAllTerms(1);

        var competenceProfile = _competenceProfileConverter.ConvertToComponent(queryResult, new HboIDomain2018(), terms);
        return competenceProfile;
    }
}