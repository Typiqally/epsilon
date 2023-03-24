using System.Net;
using System.Net.Http.Json;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using HtmlAgilityPack;

namespace Epsilon.Canvas.Service;

public class PageHttpService : HttpService, IPageHttpService
{
    private readonly ILinkHeaderConverter _headerConverter;

    public PageHttpService(HttpClient client, ILinkHeaderConverter headerConverter) : base(client)
    {
        _headerConverter = headerConverter;
    }

    public async Task<Page?> GetPageByName(int courseId, string pageName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/{pageName}");
        var response = await Client.SendAsync(request); 
        
        if(response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new Exception("Page not found");
        }
        
        if(response.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new Exception("Page forbidden");
        }
        
        if(response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new Exception("Page unauthorized");
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var page = await response.Content.ReadFromJsonAsync<Page>();
            this.GetPageImages(page);
            return page;
        }

        return null;
    }

    public async Task<IEnumerable<Page>?> GetAll(int courseId, IEnumerable<string> include)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"v1/courses/{courseId}/pages");
        var response = await Client.SendAsync(request);
        
        if(response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new Exception("Not found");
        }
        
        if(response.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new Exception("Forbidden");
        }
        
        if(response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new Exception("Unauthorized");
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<Page>>();
        }
        
        return null;
    }

    private async Task<string> GetPageImages(Page page)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(page.Body);
        
        foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
        {
            string imageSrc = htmlDoc.DocumentNode
                .SelectNodes("//img")
                .First()
                .Attributes["src"].Value;

            byte[] imageBytes = await Client.GetByteArrayAsync(imageSrc);
            string imageBase64 = Convert.ToBase64String(imageBytes);

            node.SetAttributeValue("src", $"data:image/jpeg;base64,{imageBase64}");
        }

        return htmlDoc.DocumentNode.WriteTo();
    }
}
