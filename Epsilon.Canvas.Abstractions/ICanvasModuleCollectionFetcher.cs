using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions;

public interface ICanvasModuleCollectionFetcher
{
    public Task<IEnumerable<Module>> GetAll(int courseId);
}