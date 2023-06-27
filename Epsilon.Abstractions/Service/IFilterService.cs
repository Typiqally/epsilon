using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;

namespace Epsilon.Abstractions.Service;

public interface IFilterService
{
    Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms();

    Task<IEnumerable<User>> GetAccessibleStudents();
}