using Epsilon.Abstractions.Component;

namespace Epsilon.Abstractions.Service;

public interface ICompetenceComponentService
{
    IAsyncEnumerable<IEpsilonComponent> GetComponents();

    IAsyncEnumerable<TComponent> GetComponents<TComponent>() where TComponent : IEpsilonComponent;

    Task<IEpsilonComponent?> GetComponent(string name);

    Task<TComponent?> GetComponent<TComponent>(string name) where TComponent : class, IEpsilonComponent;
}