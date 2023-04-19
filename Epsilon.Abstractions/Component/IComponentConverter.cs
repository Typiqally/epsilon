namespace Epsilon.Abstractions.Component;

public interface IComponentConverter<in T>
{
    string ConvertToJson(T data);
    Task<Stream> ConvertToWord(T data);
}