using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Service;

public interface IFilterService
{
    Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms();
}