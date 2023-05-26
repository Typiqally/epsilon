using System.Reflection;
using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Service;

namespace Epsilon.Service;

public class CompetenceComponentService : ICompetenceComponentService
{
    private readonly IEnumerable<ICompetenceComponentFetcher> _componentFetchers;

    public CompetenceComponentService(IEnumerable<ICompetenceComponentFetcher> componentFetchers)
    {
        _componentFetchers = componentFetchers;
    }

    public async IAsyncEnumerable<ICompetenceComponent> GetComponents(DateTime startDate, DateTime endDate)
    {
        foreach (var componentFetcher in _componentFetchers)
        {
            yield return await componentFetcher.FetchUnknown(startDate, endDate);
        }
    }

    public async IAsyncEnumerable<TComponent> GetComponents<TComponent>(DateTime startDate, DateTime endDate) where TComponent : ICompetenceComponent
    {
        await foreach (var component in GetComponents(startDate, endDate))
        {
            if (component is TComponent componentOfT)
            {
                yield return componentOfT;
            }
        }
    }

    public async Task<ICompetenceComponent?> GetComponent(string name, DateTime startDate, DateTime endDate)
    {
        var fetcher = _componentFetchers.SingleOrDefault(f =>
        {
            var componentType = f.GetType().BaseType?.GetGenericArguments()[0];
            if (componentType != null)
            {
                var componentNameAttribute = componentType.GetCustomAttribute<CompetenceComponentNameAttribute>(false);
                return componentNameAttribute?.Name == name;
            }

            return false;
        });
        return fetcher == null
            ? null
            : await fetcher.FetchUnknown(startDate, endDate);
    }

    public async Task<TComponent?> GetComponent<TComponent>(string name, DateTime startDate, DateTime endDate) where TComponent : class, ICompetenceComponent
    {
        return await GetComponent(name, startDate, endDate) as TComponent;
    }
}