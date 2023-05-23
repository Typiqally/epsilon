namespace Epsilon.Abstractions.Component;

public interface ICompetenceComponentFetcher
{
    public Task<ICompetenceComponent> FetchUnknown(DateTime? startDate = null, DateTime? endDate = null);
}

public interface ICompetenceComponentFetcher<TComponent> : ICompetenceComponentFetcher
    where TComponent : ICompetenceComponent
{
    public Task<TComponent> Fetch(DateTime? startDate = null, DateTime? endDate = null);
}