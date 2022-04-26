namespace Epsilon.Http.Abstractions;

public abstract class HttpService
{
    protected HttpService(HttpClient client) => Client = client;

    protected HttpClient Client { get; }
}