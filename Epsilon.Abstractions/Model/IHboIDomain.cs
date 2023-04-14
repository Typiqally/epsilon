namespace Epsilon.Abstractions.Model;

public interface IHboIDomain
{
    IEnumerable<ArchitectureLayer> ArchitectureLayers { get; }
    IEnumerable<Activity> Activities { get; }
    IEnumerable<ProfessionalSkill> ProfessionalSkills { get; }
    IEnumerable<MasteryLevel> MasteryLevels { get; }
}