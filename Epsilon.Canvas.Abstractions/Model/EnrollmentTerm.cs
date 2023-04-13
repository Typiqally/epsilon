using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record EnrollmentTerm(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("start_at")] DateTime? StartAt,
    [property: JsonPropertyName("end_at")] DateTime? EndAt
);