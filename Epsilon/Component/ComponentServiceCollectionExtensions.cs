using Epsilon.Abstractions.Component;
using Microsoft.Extensions.DependencyInjection;

namespace Epsilon.Component;

public static class ComponentServiceCollectionExtensions
{
    public static IServiceCollection AddComponentFetcher<TComponent, TFetcher>(this IServiceCollection services)
        where TFetcher : class, IEpsilonComponentFetcher<TComponent>
    {
        services.AddScoped<IEpsilonComponentFetcher, TFetcher>();
        services.AddScoped<IEpsilonComponentFetcher<TComponent>, TFetcher>();

        return services;
    }

    public static IServiceCollection AddComponentConverter<TComponent, TData, TConverter>(this IServiceCollection services)
        where TConverter : class, IEpsilonComponentConverter<TData, TComponent>
    {
        services.AddScoped<IEpsilonComponentConverter<TData>, TConverter>();
        services.AddScoped<IEpsilonComponentConverter<TData, TComponent>, TConverter>();

        return services;
    }
}