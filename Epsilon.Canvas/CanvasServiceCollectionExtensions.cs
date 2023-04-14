using System.Net.Http.Headers;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Converter;
using Epsilon.Canvas.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Epsilon.Canvas;

public static class CanvasServiceCollectionExtensions
{
    private const string CanvasHttpClient = "CanvasHttpClient";

    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddCanvas(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CanvasSettings>(config);
        services.AddHttpClient(
            CanvasHttpClient, static (provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptions<CanvasSettings>>().Value;

                client.BaseAddress = settings.ApiUrl;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.AccessToken);
            });

        services.AddHttpClient<IPaginatorHttpService, PaginatorHttpService>(CanvasHttpClient);
        services.AddHttpClient<IModuleHttpService, ModuleHttpService>(CanvasHttpClient);
        services.AddHttpClient<IAssignmentHttpService, AssignmentHttpService>(CanvasHttpClient);
        services.AddHttpClient<IOutcomeHttpService, OutcomeHttpService>(CanvasHttpClient);
        services.AddHttpClient<ISubmissionHttpService, SubmissionHttpService>(CanvasHttpClient);
        services.AddHttpClient<IGraphQlHttpService, GraphQlHttpService>(CanvasHttpClient);
        services.AddHttpClient<IAccountHttpService, AccountHttpService>(CanvasHttpClient);

        services.AddScoped<ILinkHeaderConverter, LinkHeaderConverter>();
        
        services.AddScoped<ICanvasModuleCollectionFetcher, CanvasModuleCollectionFetcher>();

        return services;
    }
}