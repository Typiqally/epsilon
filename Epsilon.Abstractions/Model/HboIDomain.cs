namespace Epsilon.Abstractions.Model;

public record HboIDomain(
    IDictionary<int, ArchitectureLayer> ArchitectureLayers,
    IDictionary<int, Activity> Activities,
    IDictionary<int, ProfessionalSkill> ProfessionalSkills,
    IDictionary<int, MasteryLevel> MasteryLevels
)
{
    public static readonly HboIDomain HboIDomain2018 = new(
        new Dictionary<int, ArchitectureLayer>
        {
            { 0, new ArchitectureLayer("Hardware Interfacing", "H", "#8D9292") },
            { 1, new ArchitectureLayer("Infrastructure", "I", "#6EA7D4") },
            { 2, new ArchitectureLayer("Organisational Processes", "O", "#D16557") },
            { 3, new ArchitectureLayer("User Interaction", "U", "#E29C53") },
            { 4, new ArchitectureLayer("Software", "S", "#96B9C0") },
        },
        new Dictionary<int, Activity>
        {
            { 0, new Activity("Manage & Control") },
            { 1, new Activity("Analysis") },
            { 2, new Activity("Advise") },
            { 3, new Activity("Design") },
            { 4, new Activity("Realisation") },
        },
        new Dictionary<int, ProfessionalSkill>
        {
            { 0, new ProfessionalSkill("Future-Oriented Organisation", "FOO") },
            { 1, new ProfessionalSkill("Investigative Problem Solving", "IPS") },
            { 2, new ProfessionalSkill("Personal Leadership", "PL") },
            { 3, new ProfessionalSkill("Targeted Interaction", "TI") },
        },
        new Dictionary<int, MasteryLevel>
        {
            { 0, new MasteryLevel(1, "#00B0F0") },
            { 1, new MasteryLevel(2, "#00B050") },
            { 2, new MasteryLevel(3, "#FFFC00") },
            { 3, new MasteryLevel(4) },
        }
    );
}