using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record Course(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("submissionsConnection")] SubmissionsConnection? SubmissionsConnection
);