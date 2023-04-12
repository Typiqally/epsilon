namespace Epsilon.Abstractions.Model;

public record HboIDomain(
    IEnumerable<ArchitectureLayer> ArchitectureLayers,
    IEnumerable<Activity> Activities,
    IEnumerable<ProfessionalSkills> ProfessionalSkills,
    IEnumerable<MasteryLevel> MasteryLevels
)
{
    public static readonly HboIDomain HboIDomain2018 = new(
        new[]
        {
            new ArchitectureLayer("Hardware Interfacing", "H", "#8D9292"),
            new ArchitectureLayer("Infrastructure", "I", "#6EA7D4"),
            new ArchitectureLayer("Organisational Processes", "O", "#D16557"),
            new ArchitectureLayer("User Interaction", "U", "#E29C53"),
            new ArchitectureLayer("Software", "S", "#96B9C0"),
        },
        new[]
        {
            new Activity("Manage & Control"),
            new Activity("Analysis"),
            new Activity("Advise"),
            new Activity("Design"),
            new Activity("Realisation"),
        },
        new[]
        {
            new ProfessionalSkills("Future-Oriented Organisation", "FOO"),
            new ProfessionalSkills("Investigative Problem Solving", "IPS"),
            new ProfessionalSkills("Personal Leadership", "PL"),
            new ProfessionalSkills("Targeted Interaction", "TI"),
        },
        new[]
        {
            new MasteryLevel(1, "#00B0F0"),
            new MasteryLevel(2, "#00B050"),
            new MasteryLevel(3, "#FFFC00"),
            new MasteryLevel(4),
        }
    );
}