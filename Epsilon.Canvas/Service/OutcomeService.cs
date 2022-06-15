using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Response;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Abstractions.Json;

namespace Epsilon.Canvas.Service;

public class OutcomeService : HttpService, IOutcomeService
{
    public OutcomeService(HttpClient client) : base(client)
    {
    }

    public async Task<Outcome?> Find(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/outcomes/{id}");
        var (_, value) = await Client.SendAsync<Outcome>(request);

        return value;
    }

    public async Task<IEnumerable<OutcomeResult>?> AllResults(int courseId, int count = 1000)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/outcome_results?per_page={count}");
        var (_, value) = await Client.SendAsync<OutcomeResultResponse>(request);

        return value?.OutcomeResults;
    }
}