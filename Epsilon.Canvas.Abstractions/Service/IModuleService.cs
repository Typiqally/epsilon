using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IModuleService
{
    Task<IEnumerable<Module>?> All(int courseId);
    Task<Module?> Find(int courseId, int id);
    Task<IEnumerable<ModuleItem>?> AllItems(int courseId, int moduleId, int limit = 100);
}