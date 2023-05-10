using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Service;

namespace Epsilon.Service;

public class CompetenceComponentService : ICompetenceComponentService
{
    private readonly IEnumerable<IComponentFetcher> _componentFetchers;

    public CompetenceComponentService(IEnumerable<IComponentFetcher> componentFetchers)
    {
        _componentFetchers = componentFetchers;
    }

    public async IAsyncEnumerable<IEpsilonComponent> GetComponents()
    {
        foreach (var componentFetcher in _componentFetchers)
        {
            yield return await componentFetcher.FetchUnknown();
        }
    }

    public async IAsyncEnumerable<TComponent> GetComponents<TComponent>() where TComponent : IEpsilonComponent
    {
        await foreach (var component in GetComponents())
        {
            if (component is TComponent componentOfT)
            {
                yield return componentOfT;
            }
        }
    }

    public async Task<IEpsilonComponent?> GetComponent(string name)
    {
        var fetcher = _componentFetchers.SingleOrDefault(f => f.ComponentName == name);
        return fetcher == null ? null : await fetcher.FetchUnknown();
    }

    public async Task<TComponent?> GetComponent<TComponent>(string name) where TComponent : class, IEpsilonComponent
    {
        return await GetComponent(name) as TComponent;
    }
}