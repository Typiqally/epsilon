using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IOutcomeService
{
    Task<Outcome?> Find(int id);
    Task<IEnumerable<OutcomeResult>?> AllResults(int courseId, int limit = 100);
}