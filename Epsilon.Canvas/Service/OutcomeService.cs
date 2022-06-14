using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;
using Epsilon.Canvas.Response;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Abstractions.Json;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
	      IEnumerable<OutcomeResult>? res = null;
	      var page = 1;
	      do {
            _logger.LogDebug("Fetching outcome results from course #{res} #{len} #{links}", JsonSerializer.Serialize(value?.OutcomeResults), value?.OutcomeResults.Count(), links.NextLink);
        
            var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/outcome_results?per_page={count}&offset={res?.Count() ?? 0}&page={page}");
            var (response, value) = await Client.SendAsync<OutcomeResultResponse>(request);
	          var links = LinkHeader.LinksFromHeader(response);

	          res = res == null ? value?.OutcomeResults : res.Concat(value.OutcomeResults);
            
	          if (links.NextLink == null) {
	            break;
            }
            
	          page += 1;
	      } while (res.Count() % 100 == 0);
        
        return res;
    }
}
