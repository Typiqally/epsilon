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
    public FileHttpService(HttpClient client) : base(client)
    {
    }

    public async Task<byte[]> GetFileByteArray(string url)
    {
        return await Client.GetByteArrayAsync(url);
    }
}