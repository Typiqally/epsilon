using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Converter;
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
        var request = new HttpRequestMessage();
        try
        {
            request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/{pageName}");
            var response = await Client.SendAsync(request);
            request.Dispose();

            if (response.StatusCode == HttpStatusCode.OK)
                return (await response.Content.ReadFromJsonAsync<Page>()).Body;
        }
        catch (Exception e)
        {
            throw new Exception($"Error in GetPageByName: {e.Message}");
        }
        finally
        {
            request?.Dispose();
        }

        return null;
    }

    public async Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include)
    {
        var request = new HttpRequestMessage();
        try
        {
            request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages");
            var response = await Client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<IEnumerable<Page>>();
        }
        catch (Exception e)
        {
            throw new Exception($"Error in GetAll: {e.Message}");
        }
        finally
        {
            request?.Dispose();
        }

        return null;
    }
}