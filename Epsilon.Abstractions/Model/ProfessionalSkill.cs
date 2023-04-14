namespace Epsilon.Abstractions.Model;

public record ProfessionalSkill(
    int Id,
    string Name,
    string ShortName,
    string? Color = null
);