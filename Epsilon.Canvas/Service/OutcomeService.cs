using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;
using Epsilon.Canvas.Response;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Abstractions.Json;
using Microsoft.Extensions.Logging;

namespace Epsilon.Canvas.Service;

public class OutcomeService : HttpService, IOutcomeService
{
    private readonly ILogger<OutcomeService> _logger;

    public OutcomeService(HttpClient client, ILogger<OutcomeService> logger) : base(client)
    {
        _logger = logger;
    }

    public async Task<Outcome?> Find(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/outcomes/{id}");
        var (response, value) = await Client.SendAsync<Outcome>(request);

        _logger.LogDebug("Fetching outcome #{OutcomeId}", id);

        return value;
    }

    public async Task<IEnumerable<OutcomeResult>?> AllResults(int courseId, int count = 1000)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/outcome_results?per_page={count}");
        var (response, value) = await Client.SendAsync<OutcomeResultResponse>(request);

        _logger.LogDebug("Fetching outcome results from course #{CourseId}", courseId);

        return value?.OutcomeResults;
    }
}