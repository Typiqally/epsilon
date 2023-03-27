using System.Net;
using System.Net.Http.Json;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class PageHttpService : HttpService, IPageHttpService
{
    public PageHttpService(HttpClient client) : base(client)
    {
    }

    public async Task<string?> GetPageByName(int courseId, string pageName)
    {
        try
        {
            using var httpClient = new HttpClient();
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/{pageName}");
                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                    return (await response.Content.ReadFromJsonAsync<Page>()).Body;
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error in GetPageByName: {e.Message}");
        }

        return null;
    }

    public async Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include)
    {
        try
        {
            using var httpClient = new HttpClient();
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages");
                var response = await Client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadFromJsonAsync<IEnumerable<Page>>();
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error in GetAll: {e.Message}");
        }

        return null;
    }
}