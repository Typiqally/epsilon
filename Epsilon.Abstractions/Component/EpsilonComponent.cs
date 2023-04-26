namespace Epsilon.Abstractions.Component;

public abstract class EpsilonComponent<T> : IEpsilonComponent<T>
{
    public async Task<object?> FetchObject()
    {
        return await Fetch();
    }

    public abstract Task<T> Fetch();
}