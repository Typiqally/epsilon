using System.Net;
using System.Net.Http.Json;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using HtmlAgilityPack;

namespace Epsilon.Canvas.Service;

public class FileHttpService : HttpService, IFileHttpService
{
    private readonly ILinkHeaderConverter _headerConverter;

    public FileHttpService(HttpClient client, ILinkHeaderConverter headerConverter) : base(client)
    {
        _headerConverter = headerConverter;
    }

    public async Task<byte[]> GetFileByteArray(string url)
    {
        if (url == null)
            throw new ArgumentNullException(nameof(url));

        return await Client.GetByteArrayAsync(url);
    }
}