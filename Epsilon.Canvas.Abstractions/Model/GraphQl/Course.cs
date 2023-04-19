using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Course(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("submissionsConnection")] SubmissionsConnection? SubmissionsConnection
);