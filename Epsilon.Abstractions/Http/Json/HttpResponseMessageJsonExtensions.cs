using System.Text.Json;

namespace Epsilon.Abstractions.Http.Json;

public static class HttpResponseMessageJsonExtensions
{
    public static object? Deserialize(this HttpResponseMessage response, Type type, JsonSerializerOptions? serializerOptions = null)
    {
        using var stream = response.Content.ReadAsStream();
        var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();

        return JsonSerializer.Deserialize(content, type, serializerOptions);
    }

    public static T? Deserialize<T>(this HttpResponseMessage response, JsonSerializerOptions? serializerOptions = null)
    {
        return (T?) response.Deserialize(typeof(T), serializerOptions);
    }

    public static async Task<object?> DeserializeAsync(this HttpResponseMessage response, Type type, JsonSerializerOptions? serializerOptions = null)
    {
        await using var contentStream = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync(contentStream, type, serializerOptions);
    }

    public static async Task<T?> DeserializeAsync<T>(this HttpResponseMessage response, JsonSerializerOptions? serializerOptions = null)
    {
        return (T?) await response.DeserializeAsync(typeof(T), serializerOptions);
    }
}