using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Http;

namespace Epsilon.Canvas.Service;

public class PageHttpService : HttpService, IPageHttpService
{
    public PageHttpService(HttpClient client)
        : base(client)
    {
    }

    public async Task<Page?> UpdateOrCreatePage(int courseId, string pageName, string pageContent)
    {
        var existingPage = await GetPageByName(courseId, pageName);

        if (existingPage == null)
        {
            return await CreatePage(courseId, pageName, pageContent);
        }

        return await UpdatePage(courseId, pageName, pageContent);
    }
    
    public async Task<Page?> CreatePage(int courseId, string pageName, string pageContent)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/courses/{courseId}/pages");
        var pageCreation = new { wiki_page = new { title = pageName, body = pageContent } };
        request.Content = new StringContent(JsonSerializer.Serialize(pageCreation), Encoding.UTF8, "application/json");

        using var response = await Client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? (await response.Content.ReadFromJsonAsync<Page?>()) 
            : null;
    }

    public async Task<Page?> UpdatePage(int courseId, string pageName, string pageContent)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"v1/courses/{courseId}/pages/{pageName}");
        var pageUpdate = new { wiki_page = new { title = pageName, body = pageContent } };
        request.Content = new StringContent(JsonSerializer.Serialize(pageUpdate), Encoding.UTF8, "application/json");

        using var response = await Client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? (await response.Content.ReadFromJsonAsync<Page?>())
            : null;
    }

    public async Task<string?> GetPageByName(int courseId, string pageName)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages/{pageName}");
        using var response = await Client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? (await response.Content.ReadFromJsonAsync<Page>())?.Body
            : null;
    }

    public async Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages");
        using var response = await Client.SendAsync(request);

        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadFromJsonAsync<IEnumerable<Page>>()
            : null;
    }
}