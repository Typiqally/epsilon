using System.Net.Http.Json;
using System.Web;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Service;

namespace Epsilon.Canvas.Service;

public class PaginatorHttpService : HttpService, IPaginatorHttpService
{
    private const int Limit = 100;
    private readonly ILinkHeaderConverter _headerConverter;

    public PaginatorHttpService(HttpClient client, ILinkHeaderConverter headerConverter)
        : base(client)
    {
        _headerConverter = headerConverter;
    }

    public async Task<IEnumerable<TResult>> GetAllPages<TResult>(HttpMethod method, Uri uri)
    {
        var pages = new List<TResult>();
        var page = "1";

        var uriString = uri.OriginalString;
        uriString += !uriString.Contains('?', StringComparison.InvariantCulture)
            ? "?"
            : "&";

        do
        {
            var offset = pages.Count * Limit;
            using var request = new HttpRequestMessage(method, $"{uriString}per_page={Limit}&offset={offset}&page={page}");
            var response = await Client.SendAsync(request);
            var value = await response.Content.ReadFromJsonAsync<TResult>();

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
        }
        while (pages.Count * Limit % Limit == 0);

        return pages;
    }
}