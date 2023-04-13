using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface IAccountHttpService
{
    public Task<IEnumerable<EnrollmentTerm>>? GetAllTerms(int accountId);
}