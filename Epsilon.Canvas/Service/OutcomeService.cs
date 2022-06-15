using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Response;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Abstractions.Json;

namespace Epsilon.Canvas.Service;

public class OutcomeService : HttpService, IOutcomeService
{
    private readonly IPaginatorService _paginator;

    public OutcomeService(HttpClient client, IPaginatorService paginator) : base(client)
    {
        _paginator = paginator;
    }

    public async Task<Outcome?> Find(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/outcomes/{id}");
        var (_, value) = await Client.SendAsync<Outcome>(request);

        return value;
    }

    public async Task<IEnumerable<OutcomeResult>?> AllResults(int courseId)
    {
        var responses = await _paginator.FetchAll<OutcomeResultResponse>(HttpMethod.Get, $"v1/courses/{courseId}/outcome_results");
        return responses.SelectMany(static r => r.OutcomeResults);
    }
}