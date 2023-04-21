namespace Epsilon.Abstractions.Component;

public interface IWordDownloader
{
    public Task<Stream> Download();
}