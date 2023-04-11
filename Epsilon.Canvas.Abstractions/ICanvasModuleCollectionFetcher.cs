using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Canvas.Abstractions;

public interface ICanvasModuleCollectionFetcher
{
    public IAsyncEnumerable<ModuleOutcomeResultCollection> GetAll(int courseId, IEnumerable<string>? allowedModules);
}