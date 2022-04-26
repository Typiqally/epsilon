using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Json;

namespace Epsilon.Canvas;

public class OutcomeService : HttpService, IOutcomeService
{
    public OutcomeService(HttpClient client) : base(client)
    {
    }

    public async Task<Outcome?> Find(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/outcomes/{id}");
        var (response, value) = await Client.SendAsync<Outcome>(request);

        return value;
    }

    public async Task<IEnumerable<OutcomeResult>?> AllResults(int courseId, int count = 1000)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/outcome_results?per_page={count}");
        var (response, value) = await Client.SendAsync<IEnumerable<OutcomeResult>>(request);

        return value;
    }
}