using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;

namespace Epsilon.Component;

public class FilterEnrollmentTerms
{
    public IEnumerable<EnrollmentTerm> FilterEnrollmentTermsByOutcome(IEnumerable<EnrollmentTerm> terms, IEnumerable<ProfessionalDevelopmentProfileOutcome> outcomes)
    {
        var filteredTerms = terms.Where(term => term.StartAt.HasValue)
            .Where(term => outcomes.Any(outcome =>
                outcome.AssessedAt > term.StartAt.Value && outcome.AssessedAt < term.EndAt))
            .Distinct();

        return filteredTerms.OrderBy(term => term.StartAt);
    }
}
