using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Canvas.Abstractions.Services;

public interface ISubmissionService
{
    public Task<IEnumerable<Submission>> GetAllFromStudent(int courseId, IEnumerable<string> include, int limit = 100);
}