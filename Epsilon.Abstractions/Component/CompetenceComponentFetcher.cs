namespace Epsilon.Abstractions.Component;

public abstract class CompetenceComponentFetcher<TComponent> : ICompetenceComponentFetcher<TComponent> where TComponent : ICompetenceComponent
{
    public async Task<ICompetenceComponent> FetchUnknown(DateTime startDate, DateTime endDate) => await Fetch(startDate, endDate);

    public abstract Task<TComponent> Fetch(DateTime startDate, DateTime endDate);
}