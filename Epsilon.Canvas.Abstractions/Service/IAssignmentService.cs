using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IAssignmentService
{
    Task<Assignment?> Find(int courseId, int id);
}