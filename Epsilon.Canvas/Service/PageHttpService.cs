﻿using System.Net;
using System.Net.Http.Json;
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

    public async Task<string?> GetPageByName(int courseId, string pageName)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/{pageName}");
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