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

    public async Task<IEnumerable<OutcomeResult>?> AllResults(int courseId, int limit = 100)
    {
        IEnumerable<OutcomeResult>? res = null;
        var page = 1;
        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/outcome_results?per_page={limit}&offset={res?.Count() ?? 0}&page={page}");
            var (response, value) = await Client.SendAsync<OutcomeResultResponse>(request);
            var links = LinkHeader.LinksFromHeader(response);

            res = res == null ? value?.OutcomeResults : res.Concat(value.OutcomeResults);

            if (links.NextLink == null)
            {
                break;
            }

            page += 1;
        } while (res.Count() % 100 == 0);

        return res;
    }
}