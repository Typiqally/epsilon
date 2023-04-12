namespace Epsilon.Abstractions.Model;

public record CompetenceProfile(
    HboIDomain HboIDomain,
    IEnumerable<ProfessionalTaskOutcome> ProfessionalTaskOutcomes,
    IEnumerable<ProfessionalSkillOutcome> ProfessionalSkillOutcomes
);