namespace Epsilon;

public record HboIDomain(
    IEnumerable<string> ArchitecturalLayers,
    IEnumerable<string> Activities,
    IEnumerable<int> Levels)
{
    public static HboIDomain HboIDomain_2018 = new(
        new[] { "U", "S", "..." },
        new[] { "Analyse", "Advice", "Realise", "...." },
        new[] { 1, 2, 3, 4 }
    );
}