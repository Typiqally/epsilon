using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IModuleHttpService
{
    Task<IEnumerable<CourseModule>?> GetAll(int courseId, IEnumerable<string> include);

    Task<CourseModule?> GetById(int courseId, int id);

    Task<IEnumerable<ModuleItem>?> GetAllItems(int courseId, int moduleId, int limit = 100);
}