using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Canvas.Abstractions.Services;

public interface IAssignmentService
{
    Task<Assignment?> Find(int courseId, int id);
}