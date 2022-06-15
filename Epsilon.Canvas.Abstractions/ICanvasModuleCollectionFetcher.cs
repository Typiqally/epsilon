using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions;

public interface ICanvasModuleCollectionFetcher
{
    public Task<IEnumerable<Module>> Fetch(int courseId);
}