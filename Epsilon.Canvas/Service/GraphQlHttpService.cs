using System.Net.Http.Json;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Http;

namespace Epsilon.Canvas.Service;

public class GraphQlHttpService : HttpService, IGraphQlHttpService
{
    public GraphQlHttpService(HttpClient client)
        : base(client)
    {
    }

    public async Task<T?> Query<T>(string query)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/graphql")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {
                    "query", query
                },
            }),
        };

        var response = await Client.SendAsync(request);

        return await response.Content.ReadFromJsonAsync<T>();
    }
}