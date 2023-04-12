using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record SubmissionsConnectionNode(
    [property: JsonPropertyName("updatedAt")] DateTime? UpdatedAt,
    [property: JsonPropertyName("postedAt")] DateTime? PostedAt,
    [property: JsonPropertyName("assignment")] Assignment? Assignment, 
    [property: JsonPropertyName("rubricAssessmentsConnection")] RubricAssessmentsConnection? RubricAssessmentsConnection
);