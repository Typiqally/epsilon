using DocumentFormat.OpenXml;
using Epsilon.Abstractions.Service;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Configuration;

namespace Epsilon.Service;

public class FilterService : IFilterService
{
    private const string GetUserSubmissionsQuery = @"
        query GetUserSubmissions {
          allCourses {
            submissionsConnection(studentIds: $studentIds) {
              nodes {
                submittedAt
              }
            }
          }
        }
    ";

    private readonly IConfiguration _configuration;
    private readonly IAccountHttpService _accountHttpService;
    private readonly IGraphQlHttpService _graphQlService;

    public FilterService(IConfiguration configuration, IAccountHttpService accountHttpService, IGraphQlHttpService graphQlHttpService)
    {
        _configuration = configuration;
        _accountHttpService = accountHttpService;
        _graphQlService = graphQlHttpService;
    }

    public async Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms()
    {
        var allTerms = await _accountHttpService.GetAllTerms(1);

        var studentId = _configuration["Canvas:StudentId"];
        var submissionsQuery = GetUserSubmissionsQuery.Replace("$studentIds", $"{studentId}", StringComparison.InvariantCultureIgnoreCase);

        var response = await _graphQlService.Query<CanvasGraphQlQueryResponse>(submissionsQuery);

        if (response?.Data == null)
        {
            return Enumerable.Empty<EnrollmentTerm>();
        }

        var submissions = response.Data.Courses!.SelectMany(c => c.SubmissionsConnection!.Nodes);

        var participatedTerms = allTerms!
            .Where(static term => term is { StartAt: not null, EndAt: not null, })
            .Where(term => submissions.Any(submission => submission.SubmittedAt >= term.StartAt && submission.SubmittedAt <= term.EndAt))
            .Distinct()
            .OrderByDescending(static term => term.StartAt);

        return participatedTerms;
    }
}