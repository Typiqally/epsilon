namespace Epsilon.Abstractions.Component.Converter;

public interface IComponentWordConverter<in TData>
{
    public Task<Stream> Convert(TData data);
}