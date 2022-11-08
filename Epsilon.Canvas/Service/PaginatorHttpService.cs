using System.Web;
using Epsilon.Abstractions.Http;
using Epsilon.Abstractions.Http.Json;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class PaginatorHttpService : HttpService, IPaginatorHttpService
{
    private const int Limit = 100;
    private readonly ILinkHeaderConverter _headerConverter;

    public PaginatorHttpService(HttpClient client, ILinkHeaderConverter headerConverter) : base(client)
    {
        _headerConverter = headerConverter;
    }

    public async Task<IEnumerable<TResult>> GetAllPages<TResult>(HttpMethod method, string uri)
    {
        var pages = new List<TResult>();
        var page = "1";

        uri += !uri.Contains('?') ? "?" : "&";

        do
        {
            var offset = pages.Count * Limit;
            var request = new HttpRequestMessage(method, $"{uri}per_page={Limit}&offset={offset}&page={page}");
            var (response, value) = await Client.SendAsync<TResult>(request);
            var links = _headerConverter.ConvertFrom(response);

            if (value != null)
            {
                pages.Add(value);
            }

            if (links.NextLink == null)
            {
                break;
            }

            var query = HttpUtility.ParseQueryString(new Uri(links.NextLink).Query);
            page = query["page"];
        } while (pages.Count * Limit % Limit == 0);

        return pages;
    }
}