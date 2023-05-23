namespace Epsilon.Abstractions.Component;

public abstract class CompetenceComponentFetcher<TComponent> : ICompetenceComponentFetcher<TComponent> where TComponent : ICompetenceComponent
{
    public async Task<ICompetenceComponent> FetchUnknown(DateTime? startDate = null, DateTime? endDate = null) => await Fetch(startDate, endDate);

    public abstract Task<TComponent> Fetch(DateTime? startDate = null, DateTime? endDate = null);
}