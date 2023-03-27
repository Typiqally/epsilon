using System.Net;
using System.Net.Http.Json;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class PageHttpService : HttpService, IPageHttpService
{
    private readonly ILinkHeaderConverter _headerConverter;

    public PageHttpService(HttpClient client, ILinkHeaderConverter headerConverter) : base(client)
    {
        _headerConverter = headerConverter;
    }

    public async Task<string?> GetPageByName(int courseId, string pageName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/{pageName}");
        var response = await Client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new Exception("Not found");

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new Exception("Forbidden");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new Exception("Unauthorized");

        if (response.StatusCode == HttpStatusCode.OK)
            return (await response.Content.ReadFromJsonAsync<Page>()).Body;

        return null;
    }

    public async Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages");
        var response = await Client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new Exception("Not found");

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new Exception("Forbidden");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new Exception("Unauthorized");

        if (response.StatusCode == HttpStatusCode.OK)
            return await response.Content.ReadFromJsonAsync<IEnumerable<Page>>();

        return null;
    }
}