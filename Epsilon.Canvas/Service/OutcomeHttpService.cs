using System.Text;
using Epsilon.Abstractions.Http;
using Epsilon.Abstractions.Http.Json;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class OutcomeHttpService : HttpService, IOutcomeHttpService
{
    private readonly IPaginatorHttpService _paginator;

    public OutcomeHttpService(HttpClient client, IPaginatorHttpService paginator) : base(client)
    {
        _paginator = paginator;
    }

    public async Task<Outcome?> Find(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/outcomes/{id}");
        var (_, value) = await Client.SendAsync<Outcome>(request);

        return value;
    }

    public async Task<OutcomeResultCollection?> GetResults(int courseId, IEnumerable<string> include)
    {
        var url = new StringBuilder($"v1/courses/{courseId}/outcome_results");
        var query = $"?include[]={string.Join("&include[]=", include)}";

        var responses = await _paginator.GetAllPages<OutcomeResultCollection>(HttpMethod.Get, url + query);
        return new OutcomeResultCollection(
            responses.SelectMany(static r => r.OutcomeResults),
            new OutcomeResultCollectionLink(
                responses.SelectMany(static r => r.Links.Outcomes),
                responses.SelectMany(static r => r.Links.Alignments)
            )
        );
    }
}