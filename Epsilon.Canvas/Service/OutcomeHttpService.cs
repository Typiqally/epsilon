using System.Net.Http.Json;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Http;

namespace Epsilon.Canvas.Service;

public class OutcomeHttpService : HttpService, IOutcomeHttpService
{
    private readonly IPaginatorHttpService _paginator;

    public OutcomeHttpService(HttpClient client, IPaginatorHttpService paginator)
        : base(client)
    {
        _paginator = paginator;
    }

    public async Task<Outcome?> Find(int id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/outcomes/{id}");
        var response = await Client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<Outcome>();
    }

    public async Task<OutcomeResultCollection?> GetResults(int courseId, IEnumerable<string> include)
    {
        var url = $"v1/courses/{courseId}/outcome_results?include[]={string.Join("&include[]=", include)}";
        var requestUri = new Uri(url, UriKind.Relative);

        var responses = await _paginator.GetAllPages<OutcomeResultCollection>(HttpMethod.Get, requestUri);
        var responsesArray = responses.ToArray();

        return new OutcomeResultCollection(
            responsesArray.SelectMany(static r => r.OutcomeResults),
            new OutcomeResultCollectionLink(
                responsesArray.SelectMany(static r => r.Links?.Outcomes ?? Array.Empty<Outcome>()),
                responsesArray.SelectMany(static r => r.Links?.Alignments ?? Array.Empty<Alignment>())));
    }
}