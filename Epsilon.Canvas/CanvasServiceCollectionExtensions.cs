using System.Net.Http.Headers;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Services;
using Epsilon.Canvas.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Epsilon.Canvas;

public static class CanvasServiceCollectionExtensions
{
    private const string CanvasHttpClient = "CanvasHttpClient";

    public static IServiceCollection AddCanvas(this IServiceCollection collection, IConfiguration config)
    {
        collection.Configure<CanvasSettings>(config);
        collection.AddHttpClient(
            CanvasHttpClient, static (provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptions<CanvasSettings>>().Value;

                client.BaseAddress = settings.ApiUrl;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.AccessToken);
            });

        collection.AddHttpClient<IModuleService, ModuleService>(CanvasHttpClient);
        collection.AddHttpClient<IAssignmentService, AssignmentService>(CanvasHttpClient);
        collection.AddHttpClient<IOutcomeService, OutcomeService>(CanvasHttpClient);

        return collection;
    }
}