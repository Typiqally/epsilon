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
    private static readonly IHboIDomain s_hboIDomain2018 = new HboIDomain2018();

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
    public async Task<ActionResult<CompetenceProfile>> GetCompetenceProfile()
    {
        var studentId = _configuration["Canvas:StudentId"];
        var query = QueryConstants.GetAllUserCoursesSubmissionOutcomes.Replace("$studentIds", $"{studentId}");

        var queryResult = _graphQlService.Query<GetAllUserCoursesSubmissionOutcomes>(query);
        var terms = _accountHttpService.GetAllTerms(1);

        await Task.WhenAll(queryResult, terms);

        var competenceProfile = _competenceProfileConverter.ConvertFrom(queryResult.Result, s_hboIDomain2018, terms.Result);
        return competenceProfile;
    }
}