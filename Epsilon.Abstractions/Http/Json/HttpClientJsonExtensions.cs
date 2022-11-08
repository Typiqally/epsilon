namespace Epsilon.Abstractions.Http.Json;

public static class HttpClientJsonExtensions
{
    public static Tuple<HttpResponseMessage, T?> Send<T>(
        this HttpClient client,
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        var response = client.Send(request, cancellationToken);
        var data =  response.Deserialize<T>();

        return new Tuple<HttpResponseMessage, T?>(response, data);
    }

    public static Tuple<HttpResponseMessage, T?> Send<T>(
        this HttpClient client,
        HttpMethod httpMethod,
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(httpMethod, requestUri);

        return client.Send<T>(request, cancellationToken);
    }

    public static async Task<Tuple<HttpResponseMessage, T?>> SendAsync<T>(
        this HttpClient client,
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        var response = await client.SendAsync(request, cancellationToken);
        var data = await response.DeserializeAsync<T>();

        return new Tuple<HttpResponseMessage, T?>(response, data);
    }

    public static async Task<Tuple<HttpResponseMessage, T?>> SendAsync<T>(
        this HttpClient client,
        HttpMethod httpMethod,
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(httpMethod, requestUri);

        return await client.SendAsync<T>(request, cancellationToken);
    }
}