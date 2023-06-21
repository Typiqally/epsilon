namespace Epsilon.Abstractions.Model;

public record Activity(
    int Id,
    string Name,
    string ShortName,
    string? Color = null
);