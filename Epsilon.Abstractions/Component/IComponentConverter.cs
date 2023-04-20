namespace Epsilon.Abstractions.Component;

public interface IComponentConverter<TData>
{
    public bool Validate(Type? type);

    public Task<TData> ConvertObject(object component);
}

public interface IComponentConverter<TData, in TComponent> : IComponentConverter<TData>
{
    public Task<TData> Convert(TComponent component);
}