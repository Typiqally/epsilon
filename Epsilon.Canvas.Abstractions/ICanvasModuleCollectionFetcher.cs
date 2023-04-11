using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions;

public interface ICanvasModuleCollectionFetcher
{
    public IAsyncEnumerable<ModuleOutcomeResultCollection> GetAll(int courseId, IEnumerable<string>? allowedModules);
}