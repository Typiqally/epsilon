using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Response;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IOutcomeHttpService
{
    Task<Outcome?> Find(int id);
    Task<OutcomeResultCollection?> GetResults(int courseId, IEnumerable<string> include);
}