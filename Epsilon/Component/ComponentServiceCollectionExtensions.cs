using Epsilon.Abstractions.Component;
using Microsoft.Extensions.DependencyInjection;

namespace Epsilon.Component;

public static class ComponentServiceCollectionExtensions
{
    public static IServiceCollection AddComponentFetcher<TComponent, TFetcher>(this IServiceCollection services)
        where TFetcher : class, ICompetenceComponentFetcher<TComponent>, ICompetenceComponentFetcher
        where TComponent : ICompetenceComponent
    {
        services.AddScoped<ICompetenceComponentFetcher, TFetcher>();
        services.AddScoped<ICompetenceComponentFetcher<TComponent>, TFetcher>();

        return services;
    }
}