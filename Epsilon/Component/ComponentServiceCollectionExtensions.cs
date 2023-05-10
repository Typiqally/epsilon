using Epsilon.Abstractions.Component;
using Microsoft.Extensions.DependencyInjection;

namespace Epsilon.Component;

public static class ComponentServiceCollectionExtensions
{
    public static IServiceCollection AddComponentFetcher<TComponent, TFetcher>(this IServiceCollection services)
        where TFetcher : class, IComponentFetcher<TComponent>, IComponentFetcher
        where TComponent : IEpsilonComponent
    {
        services.AddScoped<IComponentFetcher, TFetcher>();
        services.AddScoped<IComponentFetcher<TComponent>, TFetcher>();

        return services;
    }
}