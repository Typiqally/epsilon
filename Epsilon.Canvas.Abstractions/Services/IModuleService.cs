using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Canvas.Abstractions.Services;

public interface IModuleService
{
    Task<IEnumerable<Module>?> All(int courseId);
    Task<Module?> Find(int courseId, int id);
    Task<IEnumerable<ModuleItem>?> AllItems(int courseId, int moduleId, int count = 1000);
}