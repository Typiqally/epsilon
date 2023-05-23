using Epsilon.Abstractions.Component;

namespace Epsilon.Abstractions.Service;

public interface ICompetenceComponentService
{
    IAsyncEnumerable<ICompetenceComponent> GetComponents(DateTime? startDate = null, DateTime? endDate = null);

    IAsyncEnumerable<TComponent> GetComponents<TComponent>(DateTime? startDate = null, DateTime? endDate = null) where TComponent : ICompetenceComponent;

    Task<ICompetenceComponent?> GetComponent(string name, DateTime? startDate = null, DateTime? endDate = null);

    Task<TComponent?> GetComponent<TComponent>(string name, DateTime? startDate = null, DateTime? endDate = null) where TComponent : class, ICompetenceComponent;
}