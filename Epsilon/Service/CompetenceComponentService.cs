using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Service;

namespace Epsilon.Service;

public class CompetenceComponentService : ICompetenceComponentService
{
    private readonly IDictionary<string, ICompetenceComponentFetcher> _componentFetchers;

    public CompetenceComponentService(IDictionary<string, ICompetenceComponentFetcher> componentFetchers)
    {
        _componentFetchers = componentFetchers;
    }

    public async IAsyncEnumerable<ICompetenceComponent> GetComponents(string name, DateTime startDate, DateTime endDate)
    {
        foreach (var componentFetcher in _componentFetchers.Values)
        {
            yield return await componentFetcher.FetchUnknown(name, startDate, endDate);
        }
    }

    public async IAsyncEnumerable<TComponent> GetComponents<TComponent>(string name, DateTime startDate, DateTime endDate) where TComponent : ICompetenceComponent
    {
        await foreach (var component in GetComponents(name,startDate, endDate))
        {
            if (component is TComponent componentOfT)
            {
                yield return componentOfT;
            }
        }
    }

    public async Task<ICompetenceComponent?> GetComponent(string name, DateTime startDate, DateTime endDate)
    {
        if (_componentFetchers.TryGetValue(name, out var fetcher))
        {
            return await fetcher.FetchUnknown(name, startDate, endDate);
        }

        return null;
    }

    public async Task<TComponent?> GetComponent<TComponent>(string name, DateTime startDate, DateTime endDate) where TComponent : class, ICompetenceComponent
    {
        return await GetComponent(name, startDate, endDate) as TComponent;
    }
}