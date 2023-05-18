using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record SubmissionsHistoriesConnectionNode(
    [property: JsonPropertyName("attempt")] int? Attempt,
    [property: JsonPropertyName("submittedAt")] DateTime? SubmittedAt,
    [property: JsonPropertyName("assignment")] Assignment? Assignment,
    [property: JsonPropertyName("rubricAssessmentsConnection")] RubricAssessmentsConnection? RubricAssessments
);