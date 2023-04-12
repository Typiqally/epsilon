namespace Epsilon.Abstractions.Model;

public record CompetenceProfile(
    HboIDomain HboIDomain,
    IEnumerable<CompetenceProfileOutcome> CompetenceProfileOutcomes,
    IEnumerable<ProfessionalDevelopmentProfileOutcome> ProfessionalDevelopmentProfileOutcomes
);