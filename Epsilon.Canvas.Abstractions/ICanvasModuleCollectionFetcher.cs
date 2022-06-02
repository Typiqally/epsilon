using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Canvas.Abstractions;

public interface ICanvasModuleCollectionFetcher
{
    public Task<IEnumerable<Module>> Fetch(int courseId);
}