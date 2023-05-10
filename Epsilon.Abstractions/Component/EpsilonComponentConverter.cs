namespace Epsilon.Abstractions.Component;

public abstract class EpsilonComponentConverter<TData, TComponent> : IEpsilonComponentConverter<TData, TComponent>
    where TComponent : class, IEpsilonComponent
{
    public bool Validate(Type? type) => type == typeof(TComponent);

    public Task<TData> ConvertObject(object component)
    {
        return Convert(component as TComponent ?? throw new InvalidOperationException());
    }

    public abstract Task<TData> Convert(TComponent component);
}