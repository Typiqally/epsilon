namespace Epsilon.Abstractions.Model;

public record ArchitectureLayer(
    int Id,
    string Name,
    string ShortName,
    string? Color = null
);