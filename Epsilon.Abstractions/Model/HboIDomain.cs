namespace Epsilon.Abstractions.Model;

public record HboIDomain(
    IEnumerable<ArchitectureLayer> ArchitectureLayers,
    IEnumerable<Activity> Activities,
    IEnumerable<MasteryLevel> MasteryLevels
) 
{
    public static readonly HboIDomain HboIDomain2018 = new(
        new[]
        {
            new ArchitectureLayer("Hardware Interfacing", "#8D9292"), 
            new ArchitectureLayer("Infrastructure", "#6EA7D4"), 
            new ArchitectureLayer("Organisational Processes", "#D16557"), 
            new ArchitectureLayer("User Interaction", "#E29C53"),
            new ArchitectureLayer("Software", "#96B9C0")
        },
        new[] 
        {
            new Activity("Manage & Control"), 
            new Activity("Analysis"), 
            new Activity("Advise"), 
            new Activity("Design"),
            new Activity("Realisation")
        },
        new[] 
        {
            new MasteryLevel(1, "#00B0F0"), 
            new MasteryLevel(2, "#00B050"), 
            new MasteryLevel(3, "#FFFC00"),
            new MasteryLevel(4)
        }
    );
}

public record ArchitectureLayer(string Value, string? Color = null, string? Abbreviation = null);
public record Activity(string Value, string? Color = null);
public record MasteryLevel(int Value, string? Color = null);