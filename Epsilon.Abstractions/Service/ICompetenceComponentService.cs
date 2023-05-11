using Epsilon.Abstractions.Component;

namespace Epsilon.Abstractions.Service;

public interface ICompetenceComponentService
{
    IAsyncEnumerable<ICompetenceComponent> GetComponents();

    IAsyncEnumerable<TComponent> GetComponents<TComponent>() where TComponent : ICompetenceComponent;

    Task<ICompetenceComponent?> GetComponent(string name);

    Task<TComponent?> GetComponent<TComponent>(string name) where TComponent : class, ICompetenceComponent;
}