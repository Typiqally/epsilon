using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IAssignmentHttpService
{
    Task<IEnumerable<Assignment>?> GetAll(int courseId, IEnumerable<string> include);
    Task<Assignment?> GetById(int courseId, int id);
}