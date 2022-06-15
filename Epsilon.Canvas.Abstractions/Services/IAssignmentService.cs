using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Services;

public interface IAssignmentService
{
    Task<Assignment?> Find(int courseId, int id);
}