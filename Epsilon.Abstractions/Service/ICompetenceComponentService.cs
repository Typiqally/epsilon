using Epsilon.Abstractions.Component;

namespace Epsilon.Abstractions.Service;

public interface ICompetenceComponentService
{
    IAsyncEnumerable<ICompetenceComponent> GetComponents(DateTime startDate, DateTime endDate);

    IAsyncEnumerable<TComponent> GetComponents<TComponent>(DateTime startDate, DateTime endDate) where TComponent : ICompetenceComponent;

    Task<ICompetenceComponent?> GetComponent(string name, DateTime startDate, DateTime endDate);

    Task<TComponent?> GetComponent<TComponent>(string name, DateTime startDate, DateTime endDate) where TComponent : class, ICompetenceComponent;
}