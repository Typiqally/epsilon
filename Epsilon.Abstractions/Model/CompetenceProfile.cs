using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Model;

public record CompetenceProfile(
    IHboIDomain HboIDomain,
    IEnumerable<ProfessionalTaskResult> ProfessionalTaskOutcomes,
    IEnumerable<ProfessionalSkillResult> ProfessionalSkillOutcomes,
    IEnumerable<EnrollmentTerm> Terms,
    IEnumerable<DecayingAveragePerLayer> DecayingAveragesPerTask,
    IEnumerable<DecayingAveragePerSkill> DecayingAveragesPerSkill
);