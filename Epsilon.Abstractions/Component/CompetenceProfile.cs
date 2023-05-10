using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component;

public record CompetenceProfile(
    IHboIDomain HboIDomain,
    IEnumerable<ProfessionalTaskResult> ProfessionalTaskOutcomes,
    IEnumerable<ProfessionalSkillResult> ProfessionalSkillOutcomes,
    IEnumerable<EnrollmentTerm> Terms,
    IEnumerable<DecayingAveragePerLayer> DecayingAveragesPerTask,
    IEnumerable<DecayingAveragePerSkill> DecayingAveragesPerSkill
) : IEpsilonComponent;