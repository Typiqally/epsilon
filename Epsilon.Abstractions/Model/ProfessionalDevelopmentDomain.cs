namespace Epsilon.Abstractions.Model;

public record ProfessionalDevelopmentDomain(    
    IEnumerable<ArchitectureLayer> ArchitectureLayers,
    IEnumerable<MasteryLevel> MasteryLevels
) 
{
    public static readonly ProfessionalDevelopmentDomain ProfessionalDevelopmentDomainBsc2020 = new(
        new[]
        {
            new ArchitectureLayer("Future-Oriented Organisation", null, "FOO"), 
            new ArchitectureLayer("Investigative Problem Solving", null, "IPS"), 
            new ArchitectureLayer("Personal Leadership", null, "PL"), 
            new ArchitectureLayer("Targeted Interaction", null, "TI")
        },
        new[] 
        {
            new MasteryLevel(1, "#00B0F0"), 
            new MasteryLevel(2, "#00B050"), 
            new MasteryLevel(3, "#FFFC00")
        }
    );
}
