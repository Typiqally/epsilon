namespace Epsilon.Abstractions.Component;

public interface IEpsilonComponentConverter<TData>
{
    public bool Validate(Type? type);

    public Task<TData> ConvertObject(object component);
}

public interface IEpsilonComponentConverter<TData, in TComponent> : IEpsilonComponentConverter<TData>
{
    public Task<TData> Convert(TComponent component);
}