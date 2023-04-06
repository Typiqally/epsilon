namespace Epsilon.Abstractions.Model;

public record HboIDomain(
    IEnumerable<string> ArchitectureLayers,
    IEnumerable<string> Activities,
    IEnumerable<int> MasteryLevels
) 
{
    public static readonly HboIDomain HboIDomain2018 = new(
        new[] { "Hardware Interfacing", "Infrastructure", "Organisational Processes", "User Interaction", "Software" },
        new[] { "Manage & Control", "Analysis", "Advise", "Design", "Realisation" },
        new[] { 1, 2, 3, 4 }
    );
}