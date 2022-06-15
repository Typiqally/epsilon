using System.Web;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Http.Abstractions;
using Epsilon.Http.Abstractions.Json;

namespace Epsilon.Canvas.Service;

public class PaginatorService : HttpService, IPaginatorService
{
    private readonly ILinkHeaderConverter _headerConverter;

    public PaginatorService(HttpClient client, ILinkHeaderConverter headerConverter) : base(client)
    {
        _headerConverter = headerConverter;
    }

    public async Task<IEnumerable<TResult>> FetchAll<TResult>(HttpMethod method, string uri)
    {
        var pages = new List<TResult>();
        var page = "first";
        const int limit = 100;

        if (!uri.Contains('?'))
        {
            uri += "?";
        }
        else if (uri.Contains('&'))
        {
            uri += "&";
        }

        do
        {
            var offset = pages.Count * limit;
            var request = new HttpRequestMessage(method, $"{uri}per_page={limit}&offset={offset}&page={page}");
            var (response, value) = await Client.SendAsync<TResult>(request);
            var links = _headerConverter.ConvertFrom(response);

            pages.Add(value);

            if (links.NextLink == null)
            {
                break;
            }

            var query = HttpUtility.ParseQueryString(new Uri(links.NextLink).Query);
            page = query["page"];
        } while (pages.Count * limit % limit == 0);

        return pages;
    }
}