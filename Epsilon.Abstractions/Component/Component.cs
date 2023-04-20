namespace Epsilon.Abstractions.Component;

public abstract class Component<T> : IComponent<T>
{
    public async Task<object?> FetchObject()
    {
        return await Fetch();
    }

    public abstract Task<T> Fetch();
}